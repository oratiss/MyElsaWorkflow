using Microsoft.EntityFrameworkCore;
using TaskManagementApplication.Entities;

namespace TaskManagementApplication.Data
{
    public class TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : DbContext(options)
    {
        public DbSet<OnboardingTask> OnBoardingTasks { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure OnBoardingTask entity
            modelBuilder.Entity<OnboardingTask>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ExternalId).HasMaxLength(20);
                entity.Property(x => x.ProcessId).HasMaxLength(20);
                entity.Property(x => x.Name).HasMaxLength(300);
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.EmployeeName).HasMaxLength(200);
                entity.Property(x => x.EmployeeEmail).HasMaxLength(250);
                entity.Property(x => x.Result).HasMaxLength(30000);
            });
        }
    }
}
