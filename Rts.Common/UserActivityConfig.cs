namespace Rts.Common
{

    public class UserActivityConfig
    {

        public UserActivityConfig(PerformerGroup performerGroup, User user, List<RequiredFieldValue>? requiredFieldValues, object? possibleRequiredData = null)
        {
            CurrentPerformerGroup = performerGroup;
            CurrentPerformerUser = user;
            RequiredFieldValues = requiredFieldValues;
            PossibleRequiredData = possibleRequiredData;
        }

        public PerformerGroup CurrentPerformerGroup { get; set; } = null!; //workflow runtime

        public User CurrentPerformerUser { get; set; } = null!; //workflow runtime

        public List<RequiredFieldValue>? RequiredFieldValues { get; set; }

        public object? PossibleRequiredData { get; set; }

    }

}
