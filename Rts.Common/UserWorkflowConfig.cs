namespace Rts.Common
{
    public class UserWorkflowConfig
    {
        public List<PerformerGroup> AssignableUserGroups { get; set; } = null!;
        public UserActivityConfig FirstActivityConfig { get; set; } = null!;
    }
}
