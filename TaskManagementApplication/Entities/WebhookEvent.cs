namespace TaskManagementApplication.Entities
{
    public record WebhookEvent(string EventType, RunTaskWebhook Payload, DateTimeOffset Timestamp);
}
