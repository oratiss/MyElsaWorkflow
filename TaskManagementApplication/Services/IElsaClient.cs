
using Rts.Common;

namespace TaskManagementApplication.Services
{
    public interface IElsaClient
    {
        Task ReportTaskCompletedAsync(string taskId, object? result = null, CancellationToken cancellationToken = default);

        Task RunWorkflowAsync(string workflowDefinitionId, UserWorkflowConfig workflowConfig, CancellationToken cancellationToken = default);

    }
}