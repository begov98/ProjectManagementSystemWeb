using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    public class SubtaskController : Controller
    {
        private readonly ISubtaskService subtaskService;
        public SubtaskController(ISubtaskService _subtaskService)
        {
            subtaskService = _subtaskService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddSubtaskViewModel()
            {
                Statuses = await subtaskService.GetStatusesAsync(),
                Projects = await subtaskService.GetProjectsAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSubtaskViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await subtaskService.AddSubtaskAsync(model);

                return RedirectToAction("All", "Projects");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong!");

                return View(model);
            }

        }

    }
}
