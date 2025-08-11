namespace TaskManagementApplication.Entities
{
    public record StepWebhook(string WorkflowInstanceId, string TaskId, string TaskName, StepPayload StepPayload);
}
