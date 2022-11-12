using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Models;

namespace ProjectManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUserSubtask>()
                  .HasKey(x => new { x.ApplicationUserId, x.SubtaskId });

            builder.Entity<ApplicationUser>()
                .Property(u => u.UserName)
                .HasMaxLength(15)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                 .Property(u => u.Email)
                 .HasMaxLength(30)
                 .IsRequired();

            builder
                .Entity<Status>()
                .HasData(new Status()
                {
                    Id = 1,
                    StatusTitle = "To do"
                },
                new Status()
                {
                    Id = 2,
                    StatusTitle = "In process"
                },
                new Status()
                {
                    Id = 3,
                    StatusTitle = "Waiting for approval"
                },
                new Status()
                {
                   Id = 4,
                   StatusTitle = "Finished"
                },
                new Status()
                {
                    Id = 5,
                    StatusTitle = "Cancelled"
                });


            base.OnModelCreating(builder);

        }
    }



}