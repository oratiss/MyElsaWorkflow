using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rts.Common;
using System.Text.Json;
using TaskManagementApplication.Data;
using TaskManagementApplication.Entities;
using TaskManagementApplication.Services;

namespace TaskManagementApplication.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController(TaskManagementDbContext dbContext, IElsaClient elsaClient) : ControllerBase
    {

        [HttpPost("run-task")]
        public async Task<IActionResult> RunTask(WebhookEvent webhookEvent)
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
            var stepPayload = payload.StepPayload;
            var userWorkflowConfig = stepPayload.UserWorkflowConfig;
            var firstActivityConfig = userWorkflowConfig.FirstActivityConfig;
            var currentPerformerGroup = firstActivityConfig.CurrentPerformerGroup;
            var currentPerformerUser = firstActivityConfig.CurrentPerformerUser;

            var UserWorkflowConfig = new UserActivityConfig(
               performerGroup: new UserGroup(currentPerformerGroup.Id, currentPerformerGroup.Name),
               user: new User(currentPerformerUser.Id, currentPerformerUser.FirstName, currentPerformerUser.LastName),
               requiredFieldValues: firstActivityConfig.RequiredFieldValues
            );

            NextActivityTransistionType toBeSavedNextAtivityTransitionType = NextActivityTransistionType.None;

            switch (stepPayload.NextActivityTransistionType)
            {
                case (NextActivityTransistionType.None):
                    {
                        toBeSavedNextAtivityTransitionType = NextActivityTransistionType.None; //means this step Is with typeOf End
                        break;
                    }
                case (NextActivityTransistionType.Normal):
                    {
                        toBeSavedNextAtivityTransitionType = NextActivityTransistionType.Normal; //means this step has only one next activity 
                        break;
                    }
                case (NextActivityTransistionType.SelectByUser):
                    {
                        toBeSavedNextAtivityTransitionType = NextActivityTransistionType.SelectByUser;

                        var elsaClientResponse = elsaClient.GetWorkflowInstanceInformation(payload.WorkflowInstanceId);

                        //Todo: We should fetch next possible activities from stepWebhookEvent
                        //Todo: We should ask user which next activity should be selected
                        break;
                    }
                case (NextActivityTransistionType.SelectByLogic):
                    {
                        toBeSavedNextAtivityTransitionType = NextActivityTransistionType.SelectByLogic;
                        var elsaClientResponse = elsaClient.GetWorkflowInstanceInformation(payload.WorkflowInstanceId);
                        //Todo: We should fetch next possible activities from stepWebhookEvent
                        //Todo: We should decide what is next activity on logic after fetching inputs of user
                        break;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("No proper stepWebhookEvent with NextActivityTransistionType is passed.");
                    }
            }



            var step = new Step
            {
                ProcessId = payload.WorkflowInstanceId,
                ExternalId = payload.TaskId,
                Name = payload.TaskName,
                Description = stepPayload.Description,
                NextActivityTransistionType = toBeSavedNextAtivityTransitionType,
                CreatedAt = DateTimeOffset.UtcNow,
                UserWorkflowConfigSerialized = JsonSerializer.Serialize(UserWorkflowConfig)
            };




            await dbContext.Steps.AddAsync(step);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
