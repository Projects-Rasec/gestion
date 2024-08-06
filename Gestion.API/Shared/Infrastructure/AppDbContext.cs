using Gestion.API.Homework.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Gestion.API.Users.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gestion.API.Shared.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<users> Users { get; set; }
        public DbSet<homework> HomeworkTasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<users>(ConfigureUser);
        }

        private void ConfigureUser(EntityTypeBuilder<users> builder)
        {
            builder.Property(u => u.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserRole)Enum.Parse(typeof(UserRole), v));
        }
    }
}