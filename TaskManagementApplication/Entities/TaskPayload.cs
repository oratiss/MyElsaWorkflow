namespace TaskManagementApplication.Entities
{
    public record TaskPayload(Employee Employee, string Description, Other Other);
    public record Other(decimal Amount);

}
