
namespace TaskManagementApplication.Services
{
    public interface IElsaClient
    {
        Task ReportTaskCompletedAsync(string taskId, object? result = null, CancellationToken cancellationToken = default);
        Task ReportTaskCompletedAsync(string taskId, object? result = default, string? nextActivityId = null, CancellationToken cancellationToken = default);
    }
}