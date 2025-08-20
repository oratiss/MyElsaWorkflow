namespace Rts.Common
{
    public class User
    {

        public Guid Id { get; set; }

        public User(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName
        {
            get => $"{FirstName} {LastName}";

        }
    }
}
