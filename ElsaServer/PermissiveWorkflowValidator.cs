using Elsa.Workflows.Activities;
using Elsa.Workflows.Management;
using Elsa.Workflows.Management.Models;
using Elsa.Workflows.Models;

public class PermissiveWorkflowValidator : IWorkflowValidator
{
    public Task<IEnumerable<WorkflowValidationError>> ValidateAsync(
        Workflow workflow,
        CancellationToken cancellationToken = default)
    {
        // Always succeed by returning an empty error list.
        return Task.FromResult<IEnumerable<WorkflowValidationError>>(Array.Empty<WorkflowValidationError>());
    }
}
