namespace TaskManagementApplication.Entities
{
    public record UserActivityConfig(PerformerGroup CurrentPerformerGroup, User CurrentPerformerUser, List<RequiredField> RequiredFields, object? Decision, string? NextActivityId);

    public record User(long Id, string FirstName, string LastName);
}
