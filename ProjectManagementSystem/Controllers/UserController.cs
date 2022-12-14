using ProjectManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Data.Models;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Numerics;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UserController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            IWebHostEnvironment _webHostEnvironment)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            webHostEnvironment = _webHostEnvironment;

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");

            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Default profile picture
            string webRootPart = webHostEnvironment.ContentRootPath;
            string path = Path.Combine((webRootPart + @"wwwroot\Images\default_profilePicture.png"));
            System.Drawing.Image img = System.Drawing.Image.FromFile(path)!;
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bytes = ms.ToArray();
            }
            var profilePicture = bytes;
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                EmailConfirmed = true,
                Name = model.Name,
                Surname = model.Surname,
                ProfilePicture = profilePicture
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
               await userManager.AddToRoleAsync(user, "Specialist");
                return RedirectToAction("Login", "User");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);



        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");

            }

            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login");

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }

}

