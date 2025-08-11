using Elsa.Workflows;
using Elsa.Workflows.Management;

public class RtsActivityFinder : IRtsActivityFinder
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public RtsActivityFinder(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task<IActivity?> FindActivityAsync(string definitionId, string activityId, CancellationToken cancellationToken = default)
    {
        // Load and materialize the workflow into a blueprint
        var definition = (await _workflowDefinitionService.FindWorkflowDefinitionAsync(definitionId))!;
        var blueprint = await _workflowDefinitionService.MaterializeWorkflowAsync(definition, cancellationToken);

        if (blueprint is null) return null;

        // blueprint.Root is the root activity in the workflow graph
        return FindActivityRecursive(blueprint.Root.Activity, activityId);
    }

    //Todo: find a mechanism to find an activity which is inside a composite or container
    private IActivity? FindActivityRecursive(IActivity activity, string id)
    {
        if (activity.Id == id)
        {
            return activity;
        }

        return null;
    }
}
