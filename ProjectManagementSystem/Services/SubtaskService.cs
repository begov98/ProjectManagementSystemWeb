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
        private readonly UserManager<ApplicationUser>? userManager;

        public SubtaskService(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager) 
        {
            context = _context;
            userManager = _userManager;

        }

        public SubtaskService(ApplicationDbContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Adds new Subtask entity in the database. The data is passed by the form in Subtask/Add.cshtml
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Add the Subtask entity in DB</returns>
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

            if (entity == null)
            {
                throw new ArgumentException("Subtask cannot be created. Please try again later! If the problem persist, contact your service provider!");
            }

            await context.Subtasks.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all ApplicationUsers from the database in role "Specialist".
        /// </summary>
        /// <returns>All ApplicationUsers in role "Specialist" as IEnumerable</returns>
        public async Task<IEnumerable<ApplicationUser>> GetEmployeesAsync()
        {
                return await userManager!.GetUsersInRoleAsync("Specialist");

        }

        /// <summary>
        /// This method receives specialistId and subtaskId, find's the User and Subtask and assign the selected user
        /// to this Subtask.
        /// </summary>
        /// <param name="specialistId"></param>
        /// <param name="subtaskId"></param>
        /// <returns>Saves this relation in "ApplicationUsersSubtasks" table</returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// Gets all Projects from the database. 
        /// </summary>
        /// <returns>Returns all Projects to List</returns>
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await context.Projects.ToListAsync();
        }

        /// <summary>
        /// Gets all Statuses from the database.
        /// </summary>
        /// <returns>Returns all Statuses as IEnumerable</returns>
        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            return await context.Statuses.ToListAsync();
        }

        /// <summary>
        /// Gets all Categories from the database.
        /// </summary>
        /// <returns>Returns all Categories as IEnumerable</returns>
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        /// <summary>
        /// Gets subtask by given parameter subtaskId
        /// </summary>
        /// <param name="subtaskId"></param>
        /// <returns>Subtask data as SubtaskViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
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
                throw new ArgumentException("Task not found... :(");
            }

            return new SubtaskViewModel()
            {
                Id = subtask.Id,
                Name = subtask.Name,
                Description = subtask.Description,
                StatusId = subtask.StatusId,
                Status = subtask?.Status?.StatusTitle!,
                ProjectId = subtask!.ProjectId,
                Project = subtask?.Project?.Name!,
                CategoryId = subtask!.CategoryId,
                Category = subtask?.Category?.Name!,
                SpecialistsIds = subtask?.ApplicationUsersSubtasks?.Select(u => u.ApplicationUserId)!
            };
        }

        /// <summary>
        /// Removes assigned ApplicationUser from the Subtask
        /// </summary>
        /// <param name="specialistId"></param>
        /// <param name="subtaskId"></param>
        /// <returns>Remove the found ApplicationUser from the Subtask</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task RemoveSpecialistsAsync(string specialistId, int subtaskId)
        {
            var subtask = await context.Subtasks
                .Where(st => st.Id == subtaskId)
                .Include(st => st.ApplicationUsersSubtasks)
                .FirstOrDefaultAsync();

            if (subtask == null)
            {
                throw new ArgumentException("Task not found!");
            }

            var specialist = subtask.ApplicationUsersSubtasks.FirstOrDefault(sp => sp.ApplicationUserId == specialistId);

            if (specialist != null)
            {
                subtask.ApplicationUsersSubtasks.Remove(specialist);

                await context.SaveChangesAsync();
            }


        }

        /// <summary>
        /// Change the Subtask's status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="subtaskId"></param>
        /// <returns>Change the Status of the Subtask</returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// Finds subtask by id and deletes it.
        /// </summary>
        /// <param name="subtaskId"></param>
        /// <returns>Delete subtask entity with ID=subtaskId</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task DeleteSubtaskAsync(int subtaskId)
        {
            var subtask = await context.Subtasks.FirstOrDefaultAsync(s => s.Id == subtaskId);

            if (subtask == null)
            {
                throw new ArgumentException("Invalid subtask ID!");
            }

            context.Remove(subtask);
            await context.SaveChangesAsync();

        }

        /// <summary>
        /// Get the subtask information from DB and returns it as EditSubtaskViewModel
        /// </summary>
        /// <param name="subtaskId"></param>
        /// <returns>EditSubtaskViewModel fulfilled with Subtask entity's data</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<EditSubtaskViewModel> GetSubtaskEditInfoAsync(int subtaskId)
        {
            var subtask = await context.Subtasks
                    .FirstOrDefaultAsync(p => p.Id == subtaskId);

            var categories = await GetCategoriesAsync();

            if (subtask == null)
            {
                throw new ArgumentException("Task not found... :(");

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
        /// Edit the information in entity based on the changes in EditSubtaskViewModel form.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="subtaskId"></param>
        /// <returns>Edit and save Subtask entity's data.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task EditSubtaskAsync(EditSubtaskViewModel model, int subtaskId)
        {

            var subtask = await context.Subtasks.FirstOrDefaultAsync(s => s.Id == subtaskId);

            if (subtask == null)
            {
                throw new ArgumentException("Invalid subtask ID!");
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
        /// Finds the project manager of the subtask's project.
        /// </summary>
        /// <param name="subtaskId"></param>
        /// <returns>The project manager as ApplicationUser</returns>
        public async Task<ApplicationUser> GetProjectManagerAsync(int subtaskId)
        {
            var subtask = await context.Subtasks
                .FirstOrDefaultAsync(s => s.Id == subtaskId);

            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.Id == subtask!.ProjectId);

            var projectManager = await context.Users
                .FirstOrDefaultAsync(u => u.Id == project!.ProjectManagerId);

            if (projectManager == null)
            {
                throw new ArgumentException("ProjectManager not found...");
            }

            return projectManager;
        }


    }
}
