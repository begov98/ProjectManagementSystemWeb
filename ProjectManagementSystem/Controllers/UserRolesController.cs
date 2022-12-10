using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class UserRolesController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserRolesController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Name = user.Name;
                thisViewModel.Surname = user.Surname;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }

        [Authorize(Roles = "Manager")]
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await userManager.GetRolesAsync(user));
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ChangeRoleViewModel>();
            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new ChangeRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Manage(List<ChangeRoleViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }

    }
}
