using TaskManagementApplication.Entities;

namespace TaskManagementApplication.Views.Home
{
    public class IndexViewModel(ICollection<OnboardingTask> onboardingTasks)
    {
        public ICollection<OnboardingTask> OnboardingTasks { get; set; } = onboardingTasks;
    }
}
