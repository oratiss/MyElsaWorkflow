namespace Rts.Common
{
    public class UserWorkflowConfig
    {
        public UserGroup[] AssignableUserGroups { get; set; } = null!;
        public UserActivityConfig ActivityConfig { get; set; } = null!;

        public UserWorkflowConfig()
        {
            
        }
    }

}
