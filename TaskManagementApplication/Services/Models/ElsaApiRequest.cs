namespace TaskManagementApplication.Services.Models
{
    public class ElsaApiRequest<T> where T : class
    {
        public T Input { get; set; } = null!;
    }
}
