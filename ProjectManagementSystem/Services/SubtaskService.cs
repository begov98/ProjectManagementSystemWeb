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
            };
        }
    }
}
