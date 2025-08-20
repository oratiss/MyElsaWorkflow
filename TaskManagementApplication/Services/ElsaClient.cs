using Elsa.Expressions.Models;
using Rts.Common;
using System.Threading;
using System.Threading.Tasks;

using TaskManagementApplication.Services.Models;

namespace TaskManagementApplication.Services
{
    /// <summary>
    /// A client for the Elsa API.
    /// </summary>
    public class ElsaClient(IHttpClientFactory httpClientFactory) : IElsaClient
    {
        /// <summary>
        /// Reports a task as completed.
        /// </summary>
        /// <param name="taskId">The ID of the task to complete.</param>
        /// <param name="result">The result of the task.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public async Task ReportTaskCompletedAsync(string taskId, object? result = default, CancellationToken cancellationToken = default)
        {
            var httpclient = httpClientFactory.CreateClient("elsaHttpClient");
            var url = new Uri($"tasks/{taskId}/complete", UriKind.Relative);
            var request = new { Result = result };
            await httpclient.PostAsJsonAsync(url, request, cancellationToken);
        }
        
        /// <summary>
        /// Runs a workflow with new Instance in Elsa
        /// </summary>
        /// <param name="workflowDefinitionId"></param>
        /// <returns></returns>
        public async Task RunWorkflowAsync(string workflowDefinitionId, UserWorkflowConfig workflowConfig, CancellationToken cancellationToken = default)
        {
            var httpclient = httpClientFactory.CreateClient("elsaHttpClient");
            var url = new Uri($"workflow-definitions/{workflowDefinitionId}/execute", UriKind.Relative);
            var request = new ElsaApiRequest<object>()
            {
                Input = workflowConfig
            };
            await httpclient.PostAsJsonAsync(url, request, cancellationToken);
        }


    }
}
