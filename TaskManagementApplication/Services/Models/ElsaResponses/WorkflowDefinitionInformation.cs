using System.Text.Json.Serialization;

namespace TaskManagementApplication.Services.Models.ElsaResponses
{
    public class WorkflowDefinitionInformation
    {
        [JsonPropertyName("links")]
        public List<object>? Links { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("definitionId")]
        public string? DefinitionId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("variables")]
        public List<object>? Variables { get; set; }

        [JsonPropertyName("inputs")]
        public List<object>? Inputs { get; set; }

        [JsonPropertyName("outputs")]
        public List<object>? Outputs { get; set; }

        [JsonPropertyName("outcomes")]
        public List<object>? Outcomes { get; set; }

        [JsonPropertyName("customProperties")]
        public object? CustomProperties { get; set; }

        [JsonPropertyName("isReadonly")]
        public bool? IsReadonly { get; set; }

        [JsonPropertyName("isSystem")]
        public bool? IsSystem { get; set; }

        [JsonPropertyName("isLatest")]
        public bool? IsLatest { get; set; }

        [JsonPropertyName("isPublished")]
        public bool? IsPublished { get; set; }

        [JsonPropertyName("options")]
        public object? Options { get; set; }

        [JsonPropertyName("root")]
        public Root? Root { get; set; }
    }

    public class Connection
    {
        [JsonPropertyName("source")]
        public Source? Source { get; set; }

        [JsonPropertyName("target")]
        public Target? Target { get; set; }

        [JsonPropertyName("vertices")]
        public List<object>? Vertices { get; set; }
    }


    public class Root
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("nodeId")]
        public string? NodeId { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("customProperties")]
        public object? CustomProperties { get; set; }

        [JsonPropertyName("metadata")]
        public object? Metadata { get; set; }

        [JsonPropertyName("activities")]
        public List<object>? Activities { get; set; }

        [JsonPropertyName("variables")]
        public List<object>? Variables { get; set; }

        [JsonPropertyName("connections")]
        public List<Connection>? Connections { get; set; }
    }

    public class ActivityInfo
    {
        public string? Id { get; set; }

        public string? Name { get; set; }
    }

    public class Source
    {
        [JsonPropertyName("activity")]
        public string? Activity { get; set; }

        [JsonPropertyName("port")]
        public string? Port { get; set; }
    }

    public class Target
    {
        [JsonPropertyName("activity")]
        public string? Activity { get; set; }

        [JsonPropertyName("port")]
        public string? Port { get; set; }
    }

}
