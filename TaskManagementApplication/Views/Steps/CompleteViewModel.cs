namespace TaskManagementApplication.Views.Steps
{
    public class CompleteViewModel
    {
        public long StepId { get; set; }
        public List<NextStepButton>? NextButtons { get; set; }
        public string? SelectedNextButtonName { get; set; } // Changed to string
    }

    public class NextStepButton
    {
        public string ActivityId { get; set; } = null!;
        public string ActivityName { get; set; } = null!;
    }
}
