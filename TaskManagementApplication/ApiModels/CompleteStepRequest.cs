namespace TaskManagementApplication.ApiModels
{
    public class CompleteStepRequest
    {
        public long StepId { get; set; }
        public string? NextActivityId { get; set; }
        public object? Result { get; set; }
    }
}
