namespace TaskManagementApplication.Entities
{
    public class WorkflowEntities
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Workflow> Workflows { get; set; } = new List<Workflow>();
    }

    public class Workflow
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string? ElsaWorkflowDefinitionId { get; set; }
    }
}
