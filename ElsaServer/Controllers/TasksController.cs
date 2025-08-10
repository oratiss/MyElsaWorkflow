using Elsa.Alterations.AlterationTypes;
using Elsa.Alterations.Core.Contracts;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Management;
using Elsa.Workflows.Management.Entities;
using Elsa.Workflows.Runtime;
using Elsa.Workflows.Runtime.Filters;
using Microsoft.AspNetCore.Mvc;

namespace YourElsaServer.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TaskController : ControllerBase
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowInstanceVariableManager _variableManager;
        private readonly IAlteredWorkflowDispatcher _workflowDispatcher;
        private readonly IAlterationRunner _alterationRunner;
        private readonly ILogger<TaskController> _logger;
        //private readonly ElsaDbContext _dbContext;
        private readonly IBookmarkStore _bookmarkStore;


        public TaskController(
            IWorkflowInstanceStore workflowInstanceStore,
            ILogger<TaskController> logger,
            IWorkflowInstanceVariableManager variableManager,
            IAlteredWorkflowDispatcher workflowDispatcher,
            IAlterationRunner alterationRunner,
            IBookmarkStore bookmarkStore
            //ElsaDbContext dbContext
            )  
        {
            _workflowInstanceStore = workflowInstanceStore;
            _logger = logger;
            _variableManager = variableManager;
            _workflowDispatcher = workflowDispatcher;
            _alterationRunner = alterationRunner;
            _bookmarkStore = bookmarkStore;
            //_dbContext = dbContext;
        }

        [HttpPost("{taskId}/complete")]
        public async Task<IActionResult> CompleteTask(string taskId, [FromBody] TaskCompletionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Completing task {taskId} with NextActivityId: {request.NextActivityId}");

                // Find the workflow instance by taskId using bookmarks table
                var workflowInstance = await FindWorkflowByTaskId(taskId, cancellationToken);
                if (workflowInstance is null)
                {
                    return NotFound($"Task {taskId} not found");
                }

                var workflowInstanceId = workflowInstance.Id;

                var updates = new[]
                {
                    new VariableUpdateValue($"TaskResult_{taskId}", request.Result),
                    new VariableUpdateValue($"NextActivityId_{taskId}", request.NextActivityId)
                };

                await _variableManager.SetVariablesAsync(workflowInstanceId, updates, cancellationToken);

                var alterations = new List<IAlteration>
                {
                    new ModifyVariable
                    {
                        VariableId = $"TaskResult_{taskId}",
                        Value = request.Result!
                    },
                    new ModifyVariable
                    {
                        VariableId = $"NextActivityId_{taskId}",
                        Value = request.NextActivityId
                    }
                };

                // Apply alterations
                var results = await _alterationRunner.RunAsync(new List<string> { workflowInstanceId }, alterations, cancellationToken);

                // Dispatch altered workflows to resume execution
                await _workflowDispatcher.DispatchAsync(results, cancellationToken);

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error completing task {taskId}");
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<WorkflowInstance?> FindWorkflowByTaskId(string taskId, CancellationToken cancellationToken)
        {
            // Query bookmarks table using SQL Server JSON_VALUE to find bookmark with taskId
            //var sql = @"
            //    SELECT TOP 1 *
            //    FROM Bookmarks
            //    WHERE JSON_VALUE(SerializedPayload, '$.taskId') = {0}
            //";

            //var bookmark = await _dbContext.Bookmarks
            //    .FromSqlRaw(sql, taskId)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(cancellationToken);

            BookmarkFilter bookmarkFilter = new()
            {
                ActivityInstanceId = taskId,
            };

            var bookmark = await _bookmarkStore.FindAsync(bookmarkFilter, cancellationToken);

            if (bookmark is null) return null;

            // Now load workflow instance by WorkflowInstanceId from bookmark
            var workflowInstance = await _workflowInstanceStore.FindAsync(bookmark.WorkflowInstanceId, cancellationToken);

            return workflowInstance;
        }
    }

    public class TaskCompletionRequest
    {
        public object? Result { get; set; }
        public string? NextActivityId { get; set; }  // This is what you want
    }
}
