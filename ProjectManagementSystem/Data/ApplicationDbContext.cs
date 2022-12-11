using Microsoft.AspNetCore.Identity;
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
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

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

            builder
                  .Entity<Category>()
                    .HasData(new Category()
                    {
                        Id = 1,
                        Name = "Fixing existing issue"
                    },
                     new Category()
                     {
                         Id = 2,
                         Name = "Improvement"
                     },
                    new Category()
                    {
                        Id = 3,
                        Name = "New feature"
                    });


            string ADMIN_ID = Guid.NewGuid().ToString();
            string MANAGER_ROLE_ID = Guid.NewGuid().ToString();
            string PROJECTMANAGER_ROLE_ID = Guid.NewGuid().ToString();
            string SPECIALIST_ROLE_ID = Guid.NewGuid().ToString();


            //seed roles
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Manager",
                NormalizedName = "MANAGER",
                Id = MANAGER_ROLE_ID,
                ConcurrencyStamp = MANAGER_ROLE_ID
            },
            new IdentityRole
            {
                Name = "ProjectManager",
                NormalizedName = "PROJECTMANAGER",
                Id = PROJECTMANAGER_ROLE_ID,
                ConcurrencyStamp = PROJECTMANAGER_ROLE_ID
            },
            new IdentityRole
            {
                Name = "Specialist",
                NormalizedName = "SPECIALIST",
                Id = SPECIALIST_ROLE_ID,
                ConcurrencyStamp = SPECIALIST_ROLE_ID
            });

            //create user
            var appUser = new ApplicationUser
            {
                Id = ADMIN_ID,
                Email = "admin@pms.bg",
                EmailConfirmed = true,
                Name = "Leeroy",
                Surname = "Jenkins",
                UserName = "pmsadmin",
             NormalizedUserName = "PMSADMIN"
            };

            //set user password
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, "Admin123");

            //seed user
            builder.Entity<ApplicationUser>().HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = MANAGER_ROLE_ID,
                UserId = ADMIN_ID
            });

            base.OnModelCreating(builder);
        }

    }



}