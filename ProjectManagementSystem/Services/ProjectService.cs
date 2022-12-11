using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;
using System.ComponentModel.Design;
using Project = ProjectManagementSystem.Data.Models.Project;

namespace ProjectManagementSystem.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectService(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        /// <summary>
        /// Adds new Project entity in the database. The data is passed by the form in Projects/Add.cshtml
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Add Project entity in the DB</returns>
        public async Task AddProjectAsync(AddProjectViewModel model)
        {



            var entity = new Project()
            {
                Name = model.Name,
                Description = model.Description,
                ProjectManagerId = model.ProjectManagerId,
                Picture = model.Picture
            };

            await context.Projects.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>All projects from the DB in <IEnumerable> collection. </returns>
        public async Task<IEnumerable<ProjectViewModel>> GetAllAsync()
        {
            var entities = await context.Projects
                .Include(p => p.ProjectManager)
                .ToListAsync();

            return entities.Select(p => new ProjectViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ProjectManager = p?.ProjectManager?.Name!,
                ProjectManagerId = p!.ProjectManagerId,
                Picture = p.Picture
            });

        }

        /// <summary>
        /// Gets all ApplicationUsers in role "ProjectManager"
        /// </summary>
        /// <returns>All ApplicationUsers in role "ProjectManager" in <IEnumerable> collection.</returns>
        public async Task<IEnumerable<ApplicationUser>> GetProjectManagersAsync()
        {
            return await userManager.GetUsersInRoleAsync("ProjectManager");

        }

        /// <summary>
        /// Finds project by received projectId.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Finded project's data as ProjectViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ProjectViewModel> GetProjectAsync(int projectId)
        {


            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project not found... :(");

            }

            return new ProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                ProjectManagerId = project.ProjectManagerId,
                ProjectManager = project?.ProjectManager?.Name!,
                Picture = project!.Picture
            };
        }

        /// <summary>
        /// Gets all tasks information as IEnumerable in order to display it in Project's page.
        /// </summary>
        /// <returns>IEnumerable collection of all task's data as SubtaskViewModel.</returns>
        public async Task<IEnumerable<SubtaskViewModel>>GetTasksAsync()
        {
            var entities = await context.Subtasks
                .Include(t => t.Status)
                .Include(t => t.ApplicationUsersSubtasks)
                .ToListAsync();

            return entities.Select(t => new SubtaskViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                StatusId = t.StatusId,
                Status = t?.Status?.StatusTitle!,
                ProjectId = t!.ProjectId,
                SpecialistsIds = t?.ApplicationUsersSubtasks?.Select(u => u.ApplicationUserId)!
            });
        }

        /// <summary>
        /// Finds project by id and deletes it.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Delete project entity with ID=projectId</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task DeleteProjectAsync(int projectId)
        {
            var project = await context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            context.Remove(project);
            await context.SaveChangesAsync();

        }

        /// <summary>
        /// Get the project information from DB and returns it as EditProjectViewModel
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>EditProjectViewModel fulfilled with Project entity's data</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<EditProjectViewModel> GetProjectEditInfoAsync(int projectId)
        {
            var project = await context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project not found... :(");

            }
            var pmanagers = await GetProjectManagersAsync();

            return new EditProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Picture = project.Picture,
                ProjectManagers = pmanagers
            };
        }

        /// <summary>
        /// Edit the information in entity based on the changes in EditProjectViewModel form.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="projectId"></param>
        /// <returns>Edit and save Project entity's data.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task EditProjectAsync(EditProjectViewModel model, int projectId)
        {

            var project = await context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            if (model.Name != project.Name)
            {
                project.Name = model.Name;
            }
            if (model.Description != project.Description)
            {
                project.Description = model.Description;
            }
            if (model.ProjectManagerId != project.ProjectManagerId)
            {
                project.ProjectManagerId = model.ProjectManagerId;
            }
            if (model.Picture != model.Picture)
            {
                project.Picture = model.Picture;
            }

            await context.SaveChangesAsync();
        }
    }
    
}
