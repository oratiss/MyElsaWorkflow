namespace TaskManagementApplication.ApiControllers.ApiModels
{
    public class CompleteTaskRequest
    {
        public int TaskId { get; set; }
        public object? Result { get; set; }
    }
}
