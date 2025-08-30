namespace TaskManagementApplication.ApiControllers.ApiModels
{
    public class CompleteStepRequest
    {
        public long StepId { get; set; }
        public object? Result { get; set; }
    }
}
