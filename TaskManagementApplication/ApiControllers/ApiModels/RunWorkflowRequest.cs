using Rts.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace TaskManagementApplication.ApiControllers.ApiModels
{

    public class RunWorkflowRequest
    {
        [JsonPropertyName("assignableUserGroups")]
        public UserGroupRequest[] AssignableUserGroups { get; set; } = null!;

        [JsonPropertyName("firstActivityConfig")]
        public UserActivityConfigRequest FirstActivityConfig { get; set; } = null!;
    }

    public class UserActivityConfigRequest
    {

        public UserActivityConfigRequest(
            UserGroupRequest currentPerformerGroup, 
            UserRequest currentPerformerUser,   
            RequiredFieldValueRequest[]? requiredFieldValues,
            object? possibleRequiredData = null)
        {
            CurrentPerformerGroup = currentPerformerGroup;
            CurrentPerformerUser = currentPerformerUser;
            RequiredFieldValues = requiredFieldValues;
            PossibleRequiredData = possibleRequiredData;
        }

        [JsonPropertyName("currentPerformerGroup")]
        public UserGroupRequest CurrentPerformerGroup { get; set; }


        [JsonPropertyName("currentPerformerUser")]
        public UserRequest CurrentPerformerUser { get; set; }

        [JsonPropertyName("requiredFieldValues")]
        public RequiredFieldValueRequest[]? RequiredFieldValues { get; set; }


        [JsonPropertyName("possibleRequiredData")]
        public object? PossibleRequiredData { get; set; }

    }

    public class UserGroupRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        public UserGroupRequest(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

    }

    public class UserRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }


        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = null!;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = null!;


        public string FullName
        {
            get => $"{FirstName} {LastName}";

        }

        public UserRequest(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

    }

    public class RequiredFieldValueRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("value")]
        public required string Value { get; set; }

        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [SetsRequiredMembers]
        public RequiredFieldValueRequest(string name, string value, string type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
