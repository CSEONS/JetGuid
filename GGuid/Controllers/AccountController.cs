using GGuid.Domain.Entities;
using GGuid.Models;
using GGuid.Service;
using GGuid.Views.Services.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GGuid.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        private readonly ImageUploader _imageUploader;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ImageUploader imageUploader)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageUploader = imageUploader;
        }

        [AllowAnonymous]
        public IActionResult Register(string returnUrl = "")
        {
            return View(new RegisterViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid is false)
                return View(model);

            if (model.Password != model.RepitedPassword)
                return View(model);

            AppUser user = new()
            {
                Id = Guid.NewGuid(),
                Email = model.Email
            };

            user.UserName = $"User{new Random().Next(9999)}";

            var createUserResult = await _userManager.CreateAsync(user, model.Password);
            if (createUserResult.Succeeded is false)
                return View(model);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid is false)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction(nameof(AccountController.Register), nameof(AccountController).CutController());

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.Persistent, false);
            if (ModelState.IsValid is false)
                return View(model);

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

        public async Task<IActionResult> LogOut ()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

        public async Task<IActionResult> ChangeName(string changeName)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction(nameof(HomeController.Profile), nameof(HomeController).CutController());

            user.UserName = changeName;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(HomeController.Profile), nameof(HomeController).CutController());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfileImage(IFormFile photo)
        {
            var user = await _userManager.GetUserAsync(User);

            user.ProfileImagePath = await _imageUploader.UploadImage(photo);

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(HomeController.Profile), nameof(HomeController).CutController());
        }
    }
}
