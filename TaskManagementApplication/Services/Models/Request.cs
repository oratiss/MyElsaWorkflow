namespace TaskManagementApplication.Services.Models
{
    public class Request<T> where T : class
    {
        public T Input { get; set; } = null!;
    }
}
