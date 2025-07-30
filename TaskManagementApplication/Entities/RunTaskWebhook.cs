namespace TaskManagementApplication.Entities
{
    public record RunTaskWebhook(string WorkflowInstanceId, string TaskId, string TaskName, TaskPayload TaskPayload);
}
