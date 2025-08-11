namespace TaskManagementApplication.Entities
{
    public record UserWorkflowConfig(List<PerformerGroup>? PerformerGroups, UserActivityConfig FirstActivityConfig);

}
