namespace TaskManagementApplication.Entities
{
    public class Step
    {
        /// <summary>
        /// The ID of the Step.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// An external ID that can be used to reference the Step.
        /// </summary>
        public string ExternalId { get; set; } = default!;

        /// <summary>
        /// The ID of the onboarding process that the Step belongs to.
        /// </summary>
        public string ProcessId { get; set; } = default!;

        /// <summary>
        /// The name of the Step.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// The Step description.
        /// </summary>
        public string Description { get; set; } = default!;


        /// <summary>
        /// Whether the Step has been completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// The date and time when the Step was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the Step was completed.
        /// </summary>
        public DateTimeOffset? CompletedAt { get; set; }

        /// <summary>
        /// The possible outcome called Result we expect to keep in current Step
        /// </summary>
        public string? Result { get; set; }

        public string? NextActivityId { get; set; }
        
        public string UserWorkflowConfigSerialized { get; set; } = null!;
    }
}
