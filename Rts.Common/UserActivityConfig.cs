namespace Rts.Common
{

    public class UserActivityConfig
    {
        private PerformerGroup performerGroup;
        private User user;
        private List<RequiredFieldValue>? requiredFieldValues;

        public UserActivityConfig(PerformerGroup performerGroup, User user, List<RequiredFieldValue>? requiredFieldValues)
        {
            this.performerGroup = performerGroup;
            this.user = user;
            this.requiredFieldValues = requiredFieldValues;
        }

        public PerformerGroup CurrentPerformerGroup { get; set; } = null!; //workflow runtime

        public User CurrentPerformerUser { get; set; } = null!; //workflow runtime

        public List<RequiredFieldValue>? RequiredFieldValues { get; set; }

        public object? PossibleRequiredData { get; set; }

    }

}
