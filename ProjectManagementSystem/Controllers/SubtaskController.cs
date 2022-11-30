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
                //TODO: Add categories...
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

        [HttpGet]
        public async Task<IActionResult> Details (int subtaskId)
        {
            var model = await subtaskService.GetSubtaskAsync(subtaskId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddSpecialist(int subtaskId)
        {
            ViewBag.subtaskId = subtaskId;
            var subtask = await subtaskService.GetSubtaskAsync(subtaskId);
            //TODO:Add check
            ViewBag.SubtaskName = subtask.Name;
            ViewBag.ProjectName = subtask.Project;

            var model = new List<AddSpecialistViewModel>();

            var specialists = await subtaskService.GetEmployeesAsync();
            
            foreach(var employee in specialists)
            {
                var specialistViewModel = new AddSpecialistViewModel
                {
                    SpecialistId = employee.Id,
                    SpecialistName = employee.Name,
                    SpecialistSurname = employee.Surname
                };
              
               //TODO: Add requirements for true/false
                model.Add(specialistViewModel);
            }

            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialist(List<AddSpecialistViewModel> model,int subtaskId)
        {
            foreach (var specialist in model)
            {
              await subtaskService.AddSpecialistsToSubtask(specialist.SpecialistId, subtaskId);
            }

            return View(model); //TODO: To redirect to the right page... :)

        }



    }
}
