namespace Rts.Common
{
    public class UserWorkflowConfig
    {
        public UserGroup[] AssignableUserGroups { get; set; } = null!;
        public UserActivityConfig FirstActivityConfig { get; set; } = null!;
    }

    
}
