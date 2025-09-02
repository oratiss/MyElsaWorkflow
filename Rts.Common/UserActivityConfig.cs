namespace Rts.Common
{

    public class UserActivityConfig
    {
        public UserGroup? CurrentPerformerGroup { get; set; }

        public User? CurrentPerformerUser { get; set; }

        public Dictionary<string, object>? RequiredFieldValues { get; set; }

        public object? PossibleRequiredData { get; set; }

        public UserActivityConfig(UserGroup? performerGroup, User? user, Dictionary<string, object>? requiredFieldValues, object? possibleRequiredData = null)
        {
            CurrentPerformerGroup = performerGroup;
            CurrentPerformerUser = user;
            RequiredFieldValues = requiredFieldValues;
            PossibleRequiredData = possibleRequiredData;
        }

        public UserActivityConfig()
        {

        }

    }

}
