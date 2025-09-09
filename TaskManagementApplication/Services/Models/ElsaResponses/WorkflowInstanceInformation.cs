using System.Text.Json.Serialization;

namespace TaskManagementApplication.Services.Models.ElsaResponses
{
    public class WorkflowInstanceInformation
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("definitionId")]
        public string? DefinitionId { get; set; }

        [JsonPropertyName("definitionVersionId")]
        public string? DefinitionVersionId { get; set; }

        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("workflowState")]
        public WorkflowState? WorkflowState { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("subStatus")]
        public string? SubStatus { get; set; }

        [JsonPropertyName("isExecuting")]
        public bool? IsExecuting { get; set; }

        [JsonPropertyName("incidentCount")]
        public int? IncidentCount { get; set; }

        [JsonPropertyName("isSystem")]
        public bool? IsSystem { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

   
    public class WorkflowState
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("definitionId")]
        public string? DefinitionId { get; set; }

        [JsonPropertyName("definitionVersionId")]
        public string? DefinitionVersionId { get; set; }

        [JsonPropertyName("definitionVersion")]
        public int? DefinitionVersion { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("subStatus")]
        public string? SubStatus { get; set; }

        [JsonPropertyName("isExecuting")]
        public bool? IsExecuting { get; set; }

        [JsonPropertyName("bookmarks")]
        public List<Bookmark>? Bookmarks { get; set; }

        [JsonPropertyName("incidents")]
        public List<object>? Incidents { get; set; }

        [JsonPropertyName("isSystem")]
        public bool? IsSystem { get; set; }

        [JsonPropertyName("completionCallbacks")]
        public List<object>? CompletionCallbacks { get; set; }

        [JsonPropertyName("activityExecutionContexts")]
        public List<object>? ActivityExecutionContexts { get; set; }

        [JsonPropertyName("scheduledActivities")]
        public List<object>? ScheduledActivities { get; set; }

        [JsonPropertyName("executionLogSequence")]
        public int? ExecutionLogSequence { get; set; }

        [JsonPropertyName("input")]
        public object? Input { get; set; }

        [JsonPropertyName("output")]
        public object? Output { get; set; }

        [JsonPropertyName("properties")]
        public object? Properties { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }



    public class Bookmark
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("hash")]
        public string? Hash { get; set; }

        [JsonPropertyName("payload")]
        public object? Payload { get; set; }

        [JsonPropertyName("activityId")]
        public string? ActivityId { get; set; }

        [JsonPropertyName("activityNodeId")]
        public string? ActivityNodeId { get; set; }

        [JsonPropertyName("activityInstanceId")]
        public string? ActivityInstanceId { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("autoBurn")]
        public bool? AutoBurn { get; set; }

        [JsonPropertyName("callbackMethodName")]
        public string? CallbackMethodName { get; set; }

        [JsonPropertyName("autoComplete")]
        public bool? AutoComplete { get; set; }
    }



}
