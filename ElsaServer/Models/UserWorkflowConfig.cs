namespace ElsaServer.Models
{
    public class UserWorkflowConfig
    {
        //Todo: make it Iset or Hashset
        public List<PerformerGroup>? PerformerGroups { get; set; }
        public UserActivityConfig FirstActivityConfig { get; set; } = null!;
    }
}
