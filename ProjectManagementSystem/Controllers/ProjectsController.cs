using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using System.Security.Claims;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService _projectService)
        {
            projectService = _projectService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await projectService.GetAllAsync();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Add()
        {
            var model = new AddProjectViewModel()
            {
                ProjectManagers = await projectService.GetProjectManagersAsync()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Add(AddProjectViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await projectService.AddProjectAsync(model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong!");

                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Details (int projectId)
        {
            var model = await projectService.GetProjectAsync(projectId);
            var subtasks = await projectService.GetTasksAsync();
            ViewBag.Subtasks = subtasks;
            return View(model);

        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManageProjects()
        {
            var model = await projectService.GetAllAsync();

            var subtasks = await projectService.GetTasksAsync();

            ViewBag.Subtasks = subtasks;
            

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int projectId)
        {
            await projectService.DeleteProjectAsync(projectId);
            return RedirectToAction(nameof(ManageProjects));
        }

        [HttpGet]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Edit(int projectId)
        {
            var model = await projectService.GetProjectEditInfoAsync(projectId);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Edit (EditProjectViewModel model)
        {
            await projectService.EditProjectAsync(model, model.Id);
            return RedirectToAction(nameof(ManageProjects));
        }

    }
}
