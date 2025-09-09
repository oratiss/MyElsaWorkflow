using Elsa.Workflows.Helpers;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rts.Common;
using System.Text.Json;
using System.Threading;
using TaskManagementApplication.Data;
using TaskManagementApplication.Entities;
using TaskManagementApplication.Services;
using TaskManagementApplication.Services.Models.ElsaResponses;

namespace TaskManagementApplication.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController(TaskManagementDbContext dbContext, IElsaClient elsaClient) : ControllerBase
    {
        private JsonSerializerOptions serializerOptions => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        [HttpPost("run-task")]
        public async Task<IActionResult> RunTask(WebhookEvent webhookEvent, CancellationToken cancellationToken = default)
        {
            if (webhookEvent is null)
                return BadRequest();

            var payload = webhookEvent.Payload;
            var taskPayload = payload.TaskPayload;
            var employee = taskPayload.Employee;




            //var nextTaskList = 
            var task = new OnboardingTask
            {
                ProcessId = payload.WorkflowInstanceId,
                ExternalId = payload.TaskId,
                Name = payload.TaskName,
                Description = taskPayload.Description,
                EmployeeEmail = employee.Email,
                EmployeeName = employee.Name,
                CreatedAt = DateTimeOffset.UtcNow,

            };

            await dbContext.OnBoardingTasks.AddAsync(task);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("step")]
        public async Task<IActionResult> Step(StepWebhookEvent stepWebhookEvent)
        {
            if (stepWebhookEvent is null)
                return BadRequest();

            var payload = stepWebhookEvent.Payload;
            var stepPayload = payload.TaskPayload;


            string concatenatedNextElsaActivities = await PrepareNextElsaActivitiesToBeSaved(payload);

            //todo: save task first
            var step = new Step
            {
                ProcessId = payload.WorkflowInstanceId,
                ExternalId = payload.TaskId,
                Name = payload.TaskName,
                Description = stepPayload.Description,
                CreatedAt = DateTimeOffset.UtcNow,
                UserWorkflowConfigSerialized = JsonSerializer.Serialize(stepPayload.UserWorkflowConfig),
                NextElsaActivities = concatenatedNextElsaActivities,
            };

            //todo: to be analayzed later:
            if (!step.IsFormFilled && !step.IsCompleted)
            {
                step.Result = null;
            }
            else
            {
                throw new Exception("Result should be indicated.");
            }


            await dbContext.Steps.AddAsync(step);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        private async Task<string> PrepareNextElsaActivitiesToBeSaved(StepWebhook payload)
        {
            var nextElsaActivities = await FetchNextActivitiesFromElsa(payload.WorkflowInstanceId);
            var denulledNextElsaActivities = nextElsaActivities!.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var concatenatedNextElsaActivities = string.Join("|", denulledNextElsaActivities.Select(x => x));
            return concatenatedNextElsaActivities;
        }

        private async Task<List<string?>?> FetchNextActivitiesFromElsa(string wfInstanceId)
        {
            var serializedActivityInstanceInfo = await elsaClient.GetWorkflowInstanceInformationAsync(wfInstanceId);
            var activityInstanceInfo = JsonSerializer.Deserialize<ActivityInstanceInformation>(serializedActivityInstanceInfo);
            var activityId = activityInstanceInfo!.WorkflowState!.Bookmarks!.OrderByDescending(bookmark => bookmark.CreatedAt).FirstOrDefault()!.ActivityId;

            var serializedWorkflowDefInfo = await elsaClient.GetWorkflowDefinitionInformationAsync(activityInstanceInfo!.DefinitionId);
            var wfDefInfo = JsonSerializer.Deserialize<WorkflowDefinitionInformation>(serializedWorkflowDefInfo);

            var nextIfActivityId = wfDefInfo!.Root!.Connections!.FirstOrDefault(x => x.Source!.Activity == activityId)!.Target!.Activity;
            var nextActivities = wfDefInfo!.Root!.Connections!.Where(x => x.Source!.Activity == nextIfActivityId).Select(connection => connection.Target!.Activity).ToList();

            var firstList = JsonSerializer.Deserialize<List<ActivityInfo>>(JsonSerializer.Serialize(wfDefInfo.Root.Activities, serializerOptions), serializerOptions);
            var secondList = firstList!.Select(x =>
            {
                var a = x.Adapt<ActivityInfo>();
                return new { a.Id, a.Name };
            }).ToList();
            List<string?> result = new();
            foreach (var activity in nextActivities)
            {
                var activityName = secondList.FirstOrDefault(x => x.Id == activity)?.Name;
                if (activity == "end")
                {
                    result.Add($"{activity}--{activity}");
                }
                else
                {
                    result.Add($"{activity}--{activityName}");
                }
            }
            return result;
        }
    }

    public class ACtiivtyBriefInfo
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
