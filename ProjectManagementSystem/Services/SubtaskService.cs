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

        /// <summary>
        /// This method receives Subtasks's required information from AddSubtaskViewModel and create entity in the DB.
        /// </summary>
        public async Task AddSubtaskAsync(AddSubtaskViewModel model)
        {
            var entity = new Subtask()
            {
                Name = model.Name,
                Description = model.Description,
                StatusId = model.StatusId,
                ProjectId = model.ProjectId,
                CategoryId = model.CategoryId
                
            };

            await context.Subtasks.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// This method returns all employees with Role "Specialist" as <IEnumerable>
        /// </summary>
        public async Task<IEnumerable<ApplicationUser>> GetEmployeesAsync()
        {
                return await userManager.GetUsersInRoleAsync("Specialist");

        }

        /// <summary>
        /// This method receives specialistId and subtaskId, find's the User and Subtask and assign the selected user
        /// to this Subtask. As result, saves this relation in "ApplicationUsersSubtasks" table
        /// </summary>
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

        /// <summary>
        /// This method returns all Projects as <IEnumerable>
        /// </summary>
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await context.Projects.ToListAsync();
        }

        /// <summary>
        /// This method returns all Statuses as <IEnumerable>
        /// </summary>
        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            return await context.Statuses.ToListAsync();
        }

        /// <summary>
        /// This method returns all Categories as <IEnumerable>
        /// </summary>
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        /// <summary>
        /// This method receives subtaskId, find the selected Subtask and returns it's data
        /// as SubtaskViewModel.
        /// </summary>
        public async Task<SubtaskViewModel> GetSubtaskAsync(int subtaskId)
        {
            var subtask = await context.Subtasks
                .Include(st => st.Status)
                .Include(st => st.Category)
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
                CategoryId = subtask.CategoryId,
                Category = subtask?.Category?.Name,
                SpecialistsIds = subtask?.ApplicationUsersSubtasks?.Select(u => u.ApplicationUserId)
            };
        }

        /// <summary>
        /// This method receives subtaskId and specialistId, finds the User and the Subtask and remove this relation 
        /// from ApplicationUsersSubtasks.
        /// </summary>
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

        /// <summary>
        /// This method receives subtaskId and statusId, finds the Status and the Subtask and change the Subtask's
        /// current status with the new selected one.
        /// </summary>
        public async Task ChangeStatusAsync(int statusId, int subtaskId)
        {
            var subtask = await context.Subtasks
                .Where(st => st.Id == subtaskId)
                .FirstOrDefaultAsync();

            if (subtask == null)
            {
                throw new ArgumentException("Invalid subtask ID!");
            }

            subtask.StatusId = statusId;

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// This method receives subtaskId, finds the Subtask in DB and deletes it.
        /// </summary>
        public async Task DeleteSubtaskAsync(int subtaskId)
        {
            var subtask = await context.Subtasks.FirstOrDefaultAsync(s => s.Id == subtaskId);

            if (subtask == null)
            {
                throw new ArgumentException("Project not found");
            }

            context.Remove(subtask);
            await context.SaveChangesAsync();

        }

        /// <summary>
        /// This method receives subtaskId, finds the Subtask and loads it's data as EditSubtaskViewModel.
        /// </summary>
        public async Task<EditSubtaskViewModel> GetSubtaskEditInfoAsync(int subtaskId)
        {
            var subtask = await context.Subtasks
                    .FirstOrDefaultAsync(p => p.Id == subtaskId);

            var categories = await GetCategoriesAsync();

            if (subtask == null)
            {
                throw new ArgumentException("Project not found... :("); //TODO: To implement some error message/page!

            }
            return new EditSubtaskViewModel()
            {
                Id = subtask.Id,
                Name = subtask.Name,
                Description = subtask.Description,
                Categories = categories
            };
        }

        /// <summary>
        /// This method receives subtaskId and data as EditSubtaskViewModel, finds the Subtask and updates its info according to the EditSubtaskViewModel's data.
        /// </summary>
        public async Task EditSubtaskAsync(EditSubtaskViewModel model, int subtaskId)
        {

            var subtask = await context.Subtasks.FirstOrDefaultAsync(s => s.Id == subtaskId);

            if (subtask == null)
            {
                throw new ArgumentException("Project not found");
            }

            if (model.Name != subtask.Name)
            {
                subtask.Name = model.Name;
            }
            if (model.Description != subtask.Description)
            {
                subtask.Description = model.Description;
            }
            if (model.CategoryId != subtask.CategoryId)
            {
                subtask.CategoryId = model.CategoryId;
            }


            await context.SaveChangesAsync();
        }

        /// <summary>
        /// This method receives subtaskId, finds the Subtask and returns the related Project's ProjectManager. 
        /// </summary>
        public async Task<ApplicationUser> GetProjectManagerAsync(int subtaskId)
        {
            var subtask = await context.Subtasks
                .FirstOrDefaultAsync(s => s.Id == subtaskId);

            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.Id == subtask.ProjectId);

            var projectManager = await context.Users
                .FirstOrDefaultAsync(u => u.Id == project.ProjectManagerId);

            return projectManager;
        }


    }
}
