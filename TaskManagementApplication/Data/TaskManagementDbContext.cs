using Microsoft.EntityFrameworkCore;
using TaskManagementApplication.Entities;

namespace TaskManagementApplication.Data
{
    public class TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : DbContext(options)
    {
        public DbSet<OnboardingTask> OnBoardingTasks { get; set; } = default!;
    }
}
