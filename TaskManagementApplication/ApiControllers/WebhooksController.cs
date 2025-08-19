using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rts.Common;
using System.Text.Json;
using TaskManagementApplication.Data;
using TaskManagementApplication.Entities;

namespace TaskManagementApplication.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController(TaskManagementDbContext dbContext) : ControllerBase
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

            var UserWorkflowConfig  = new UserActivityConfig(
               new PerformerGroup(currentPerformerGroup.Id, currentPerformerGroup.Name),
               new User(currentPerformerUser.Id, currentPerformerUser.FirstName, currentPerformerUser.LastName),
               firstActivityConfig.RequiredFieldValues

                );

            var step = new Step
            {
                ProcessId = payload.WorkflowInstanceId,
                ExternalId = payload.TaskId,
                Name = payload.TaskName,
                Description = stepPayload.Description,
                CreatedAt = DateTimeOffset.UtcNow,
                UserWorkflowConfigSerialized = JsonSerializer.Serialize(UserWorkflowConfig)
            };


            await dbContext.Steps.AddAsync(step);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
