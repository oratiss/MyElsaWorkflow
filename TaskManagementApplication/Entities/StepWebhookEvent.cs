namespace TaskManagementApplication.Entities
{
    public record StepWebhookEvent(string EventType, StepWebhook Payload, DateTimeOffset Timestamp);
}
