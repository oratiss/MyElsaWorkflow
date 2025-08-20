namespace Rts.Common
{

    public class UserActivityConfig(UserGroup performerGroup, User user, List<RequiredFieldValue>? requiredFieldValues, object? possibleRequiredData = null)
    {
        public UserGroup CurrentPerformerGroup { get; set; } = performerGroup;

        public User CurrentPerformerUser { get; set; } = user;

        public List<RequiredFieldValue>? RequiredFieldValues { get; set; } = requiredFieldValues;

        public object? PossibleRequiredData { get; set; } = possibleRequiredData;

    }

}
