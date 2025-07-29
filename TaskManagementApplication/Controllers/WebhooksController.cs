using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApplication.Data;
using TaskManagementApplication.Entities;

namespace TaskManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController(TaskManagementDbContext dbContext) : ControllerBase
    {
        [HttpPost("run-task")]
        public async Task<IActionResult> RunTask(WebhookEvent webhookEvent)
        {
            if (webhookEvent is null)
                return Ok();

            var payload = webhookEvent.Payload;
            var taskPayload = payload.TaskPayload;
            var employee = taskPayload.Employee;

            var task = new OnboardingTask
            {
                ProcessId = payload.WorkflowInstanceId,
                ExternalId = payload.TaskId,
                Name = payload.TaskName,
                Description = taskPayload.Description,
                EmployeeEmail = employee.Email,
                EmployeeName = employee.Name,
                CreatedAt = DateTimeOffset.Now
            };

            await dbContext.OnBoardingTasks.AddAsync(task);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
