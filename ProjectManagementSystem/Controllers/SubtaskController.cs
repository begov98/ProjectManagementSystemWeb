using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    public class SubtaskController : Controller
    {
        private readonly ISubtaskService subtaskService;
        private readonly ICommentService commentService;
        public SubtaskController(ISubtaskService _subtaskService, ICommentService _commentService)
        {
            subtaskService = _subtaskService;
            commentService = _commentService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Add()
        {
            var model = new AddSubtaskViewModel()
            {
                Statuses = await subtaskService.GetStatusesAsync(),
                Projects = await subtaskService.GetProjectsAsync(),
                Categories = await subtaskService.GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Add(AddSubtaskViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await subtaskService.AddSubtaskAsync(model);

                return RedirectToAction("Details", "Projects", new { ProjectId = model.ProjectId });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong!");

                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details (int subtaskId)
        {
            var model = await subtaskService.GetSubtaskAsync(subtaskId);
            var comments = await commentService.GetCommentsByIdAsync(subtaskId);
            var pm = await subtaskService.GetProjectManagerAsync(subtaskId);
            int count = comments.Count();
            ViewBag.NumberOfComments = count;
            ViewBag.Comments = comments;
            ViewBag.ProjectManager = pm;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(CommentViewModel commentModel)
        {
            await commentService.AddCommentAsync(commentModel);
            var comments = await commentService.GetCommentsAsync();
            int count = comments.Count();
            ViewBag.NumberOfComments = count;
            ViewBag.Comments = comments;
            return RedirectToAction("Details", new { subtaskId = commentModel.SubtaskId });
        }

        [HttpGet]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> AddSpecialist(int subtaskId)
        {
            ViewBag.subtaskId = subtaskId;
            var subtask = await subtaskService.GetSubtaskAsync(subtaskId);
            if (subtask == null)
            {
                throw new ArgumentException("Not found subtask with the given ID");
            }
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

                if (employee.ApplicationUsersSubtasks.Any(st => st.SubtaskId == subtaskId))
                {
                    specialistViewModel.Selected = true;
                }
                else
                {
                    specialistViewModel.Selected = false;
                }

                model.Add(specialistViewModel);
            }

            return View(model);
            


        }

        [HttpPost]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> AddSpecialist(List<AddSpecialistViewModel> model,int subtaskId)
        {
            
            foreach (var specialist in model)
            {
                await subtaskService.RemoveSpecialistsAsync(specialist.SpecialistId, subtaskId);
            }

            foreach (var checkSpecialist in model)
            {
                if (checkSpecialist.Selected)
                {
                    await subtaskService.AddSpecialistsToSubtask(checkSpecialist.SpecialistId, subtaskId);
                }
            }


            return RedirectToAction("Details", new { subtaskId = subtaskId});

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(int subtaskId)
        {
            ViewBag.subtaskId = subtaskId;
            var subtask = await subtaskService.GetSubtaskAsync(subtaskId);
            if (subtask == null)
            {
                throw new ArgumentException("Not found subtask with the given ID");
            }
            ViewBag.SubtaskName = subtask.Name;
            ViewBag.ProjectName = subtask.Project;

            var model = new List<ChangeStatusViewModel>();

            var statuses = await subtaskService.GetStatusesAsync();

            foreach (var status in statuses)
            {
                var statusViewModel = new ChangeStatusViewModel
                {
                    StatusId = status.Id,
                    StatusName = status.StatusTitle
                };

                if (subtask.StatusId == status.Id)
                {
                    statusViewModel.Selected = true;
                }
                else
                {
                    statusViewModel.Selected = false;
                }

                model.Add(statusViewModel);
            }

            return View(model);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(List<ChangeStatusViewModel> model, int subtaskId)
        {

            foreach (var checkStatus in model)
            {
                if (checkStatus.Selected)
                {
                    await subtaskService.ChangeStatusAsync(checkStatus.StatusId, subtaskId);
                }
            }


            return RedirectToAction("Details", new { subtaskId = subtaskId });

        }

        [HttpPost]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Delete(int subtaskId, int projectId)
        {
            await subtaskService.DeleteSubtaskAsync(subtaskId);
            return RedirectToAction("Details", "Projects", new { projectId = projectId });
        }

        [HttpGet]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Edit(int subtaskId)
        {
            var model = await subtaskService.GetSubtaskEditInfoAsync(subtaskId);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, ProjectManager")]
        public async Task<IActionResult> Edit(EditSubtaskViewModel model)
        {
            await subtaskService.EditSubtaskAsync(model, model.Id);
            return RedirectToAction("Details", "Subtask", new { subtaskId = model.Id });
        }


    }
}
