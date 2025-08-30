
using Rts.Common;

namespace TaskManagementApplication.Services
{
    public interface IElsaClient
    {
        Task ReportTaskCompletedAsync(string taskId, object? result = null, CancellationToken cancellationToken = default);

        Task RunWorkflowAsync(string workflowDefinitionId, UserWorkflowConfig workflowConfig, CancellationToken cancellationToken = default);

        Task<string> GetWorkflowInstanceInformationAsync(string workflowInstanceId, CancellationToken cancellationToken = default);
        Task<string> GetWorkflowDefinitionInformationAsync(string? definitionId, CancellationToken cancellationToken = default);
    }
}