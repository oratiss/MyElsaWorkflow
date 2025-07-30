
namespace TaskManagementApplication.Services
{
    public interface IElsaClient
    {
        Task ReportTaskCompletedAsync(string taskId, object? result = null, CancellationToken cancellationToken = default);
    }
}