namespace TaskManagementApplication.Views.Steps
{
    public class CompleteViewModel
    {
        public long StepId { get; set; }
        public List<NextStepButton>? NextButtons { get; set; }

        public NextStepButton? SelectedNextButton { get; set; }
    }

    public class NextStepButton
    {
        public string ActivityId { get; set; } = null!;
        public string ActivityName { get; set; } = null!;
    }
}
