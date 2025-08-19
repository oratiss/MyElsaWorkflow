namespace Rts.Common
{
    public class PerformerGroup
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public PerformerGroup(long id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
