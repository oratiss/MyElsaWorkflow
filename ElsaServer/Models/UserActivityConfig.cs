namespace ElsaServer.Models
{

    public class UserActivityConfig
    {
        public PerformerGroup CurrentPerformerGroup { get; set; } = null!;
        
        public User CurrentPerformerUser { get; set; } = null!;

        public List<RequiredField>? RequiredFields { get; set; }

        public object? Decision { get; set; }

        public string? NextActivityId { get; set; }

    }
}
