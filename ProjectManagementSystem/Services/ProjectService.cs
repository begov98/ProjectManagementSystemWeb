﻿using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ProjectManager = p?.ProjectManager?.Name,
                ProjectManagerId = p.ProjectManagerId,
                Picture = p.Picture
            });

        }

        public async Task<IEnumerable<ApplicationUser>> GetProjectManagersAsync()
        {
            return await userManager.GetUsersInRoleAsync("ProjectManager");

        }

        public async Task<ProjectViewModel> GetProjectAsync(int projectId)
        {


            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project not found... :("); //TODO: To implement some error message/page!

            }

            return new ProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                ProjectManager = project?.ProjectManager?.Name,
                Picture = project.Picture
            };


        }
    }
}
