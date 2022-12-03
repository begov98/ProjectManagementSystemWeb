using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services
{
    public class SubtaskService : ISubtaskService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public SubtaskService(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager) 
        {
            context = _context;
            userManager = _userManager;

        }


        public async Task AddSubtaskAsync(AddSubtaskViewModel model)
        {
            var entity = new Subtask()
            {
                Name = model.Name,
                Description = model.Description,
                StatusId = model.StatusId,
                ProjectId = model.ProjectId
                
            };

            await context.Subtasks.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetEmployeesAsync()
        {
                return await userManager.GetUsersInRoleAsync("Specialist");

        }

        public async Task AddSpecialistsToSubtask(string specialistId, int subtaskId)
        {
            var specialist = await context.Users
                .Where(sp => sp.Id == specialistId)
                .Include(u => u.ApplicationUsersSubtasks)
                .FirstOrDefaultAsync();

            if (specialist == null)
            {
                throw new ArgumentException("Invalid specialist ID!");
            }

            var subtask = await context.Subtasks.FirstOrDefaultAsync(st => st.Id == subtaskId);

            if (subtask == null)
            {
                throw new ArgumentException("Invalid subtask ID!");
            }

            if (!specialist.ApplicationUsersSubtasks.Any(st => st.SubtaskId == subtaskId))
            {
                specialist.ApplicationUsersSubtasks.Add(new ApplicationUserSubtask()
                {
                    SubtaskId = subtask.Id,
                    ApplicationUserId = specialist.Id,
                    Subtask = subtask,
                    ApplicationUser = specialist
                });
            }

            await context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await context.Projects.ToListAsync();
        }

        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            return await context.Statuses.ToListAsync();
        }

        public async Task<SubtaskViewModel> GetSubtaskAsync(int subtaskId)
        {
            var subtask = await context.Subtasks
                .Include(st => st.Status)
                .Include(st => st.Project)
                .Include(st => st.ApplicationUsersSubtasks)
                .FirstOrDefaultAsync(s => s.Id == subtaskId);


            if (subtask == null)
            {
                throw new ArgumentException("Project not found... :("); //TODO: To implement some error message/page!
            }

            return new SubtaskViewModel()
            {
                Id = subtask.Id,
                Name = subtask.Name,
                Description = subtask.Description,
                StatusId = subtask.StatusId,
                Status = subtask?.Status?.StatusTitle,
                ProjectId = subtask.ProjectId,
                Project = subtask?.Project?.Name,
                SpecialistsIds = subtask?.ApplicationUsersSubtasks?.Select(u => u.ApplicationUserId)
            };
        }

        public async Task RemoveSpecialistsAsync(string specialistId, int subtaskId)
        {
            var subtask = await context.Subtasks
                .Where(st => st.Id == subtaskId)
                .Include(st => st.ApplicationUsersSubtasks)
                .FirstOrDefaultAsync();

            if (subtask == null)
            {
                throw new ArgumentException("Invalid user ID!");
            }

            var specialist = subtask.ApplicationUsersSubtasks.FirstOrDefault(sp => sp.ApplicationUserId == specialistId);

            if (specialist != null)
            {
                subtask.ApplicationUsersSubtasks.Remove(specialist);

                await context.SaveChangesAsync();
            }


        }

        public async Task ChangeStatusAsync(int statusId, int subtaskId)
        {
            var subtask = await context.Subtasks
                .Where(st => st.Id == subtaskId)
                .FirstOrDefaultAsync();

            subtask.StatusId = statusId;

            await context.SaveChangesAsync();
        }
    }
}
