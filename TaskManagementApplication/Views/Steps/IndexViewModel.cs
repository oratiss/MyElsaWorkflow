using TaskManagementApplication.Entities;

namespace TaskManagementApplication.Views.Steps
{
    public class IndexViewModel(ICollection<Step> steps)
    {
        public ICollection<Step> Steps { get; set; } = steps;
    }
}
