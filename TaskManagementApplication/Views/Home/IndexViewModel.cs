using TaskManagementApplication.Entities;

namespace TaskManagementApplication.Views.Home
{
    //public class IndexViewModel(ICollection<OnboardingTask> onboardingTasks)
    //{
    //    public ICollection<OnboardingTask> OnboardingTasks { get; set; } = onboardingTasks;
    //}
    public class IndexViewModel(ICollection<Step> steps)
    {
        public ICollection<Step> Steps { get; set; } = steps;
    }
}
