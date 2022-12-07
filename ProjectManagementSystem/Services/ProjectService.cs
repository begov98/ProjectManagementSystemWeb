﻿using Microsoft.AspNetCore.Identity;
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
                .Include(p => p.ProjectManager)
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
                Status = t?.Status?.StatusTitle,
                ProjectId = t.ProjectId,
                SpecialistsIds = t?.ApplicationUsersSubtasks?.Select(u => u.ApplicationUserId)
            });

            //var subtasks = await context.Subtasks.ToListAsync();
            //return subtasks;
        }

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

        public async Task<EditProjectViewModel> GetProjectEditInfoAsync(int projectId)
        {
            var project = await context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project not found... :("); //TODO: To implement some error message/page!

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
