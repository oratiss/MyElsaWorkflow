using System.Text.Json.Serialization;

namespace TaskManagementApplication.Services.Models.ElsaResponses
{
    public class WorkflowInstanceJournal
    {
        public List<JournalItem>? Items { get; set; }
    }

    public class JournalItem
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("activityInstanceId")]
        public string? ActivityInstanceId { get; set; }

        [JsonPropertyName("activityId")]
        public string? ActivityId { get; set; }

        [JsonPropertyName("activityType")]
        public string? ActivityType { get; set; }

        [JsonPropertyName("activityTypeVersion")]
        public int? ActivityTypeVersion { get; set; }

        [JsonPropertyName("nodeId")]
        public string? NodeId { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonPropertyName("sequence")]
        public int? Sequence { get; set; }

        [JsonPropertyName("eventName")]
        public string? EventName { get; set; }
    }
}
