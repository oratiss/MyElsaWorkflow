namespace Rts.Common
{
    public class UserGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public UserGroup(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
