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

        public ProjectService(ApplicationDbContext _context)
        {
            context = _context;
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
    }
}
