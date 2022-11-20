using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;


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

        public async Task<IEnumerable<ProjectViewModel>> GetAllAsync()
        {
            var entities = await context.Projects
                .ToListAsync();



            return entities.Select(p => new ProjectViewModel()
            {
                Name = p.Name,
                Description = p.Description,
                ProjectManagerId = p.ProjectManagerId,
                Picture = p.Picture
            });

        }

        public async Task<IEnumerable<ApplicationUser>> GetProjectManagersAsync()
        {
            return await userManager.GetUsersInRoleAsync("ProjectManager");

            //var listOfPMs = await userManager.GetUsersInRoleAsync("Project manager");

            //var pMNames = new List<string>();

            //foreach (var PMName in listOfPMs)
            //{
            //    pMNames.Add(PMName.Name);
            //}

            //return pMNames;
        }
    }
}
