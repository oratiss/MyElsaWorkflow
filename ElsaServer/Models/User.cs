namespace ElsaServer.Models
{
    public class User
    {

        public long Id { get; set; }

        public User(long id, string firstName, string lastName)
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
