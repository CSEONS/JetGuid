using GGuid.Domain;
using GGuid.Domain.Entities;
using GGuid.Models;
using GGuid.Service;
using GGuid.Views.Services.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GGuid.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ImageUploader _imageUploader;

        public HomeController(DataManager dataManager, UserManager<AppUser> userManager, ImageUploader imageUploader)
        {
            _dataManager = dataManager;
            _userManager = userManager;
            _imageUploader = imageUploader;
        }

        public IActionResult Index()
        {
            var events = _dataManager.Events.GetAll().Skip(0).Take(10);

            return View(events);
        }

        public IActionResult AddEvent()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(user);
        }

        public IActionResult EventList(EventFilter? filter, int start = 0, int count = 10)
        {
            if (filter is null)
                filter = new EventFilter()
                {
                    Format = "Both"
                };

            var models = _dataManager.Events.GetAll();

            if (string.IsNullOrEmpty(filter.SearchText) is false)
            {
                models = models.Where(e => e.Name.Contains(filter.SearchText));
            }

            return View(models);
        }

        public async Task<IActionResult> CreateEventForm(Guid id)
        {
            var item = await _dataManager.Events.GetByIdAsync(id);

            if (item is null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent(Event item, IFormFile previewImage)
        {
            if (previewImage is null || previewImage.Length < 1)
                item.PreviewImgPath = _imageUploader.DefaultEventLogo;
            else
                item.PreviewImgPath = await _imageUploader.UploadImage(previewImage);

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return BadRequest();

            user.CreateEventCount++;
            await _userManager.UpdateAsync(user);

            item.OwnerId = user.Id;
            item.Format = item.Format == "0" || item.Format == "1" ? (item.Format == "0" ? "Online" : "Offline") : item.Format; 

            await _dataManager.Events.AddAsync(item);

            return RedirectToAction(nameof(HomeController.Profile), nameof(HomeController).CutController());
        }

        public async Task<IActionResult> EventDetails(Guid id)
        {
            var item = await _dataManager.Events.GetByIdAsync(id);

            if (item is null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(item.OwnerId.ToString());

            ViewBag.OwnerName = user.UserName;
            ViewBag.OwnerImage = user.ProfileImagePath;

            return View(item);
        }

        public async Task<IActionResult> JoinEvent(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            var item = await _dataManager.Events.GetByIdAsync(id);
            if (item is null)
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());

            user.JoinEvents.Add(item);
            user.VisitCount++;
            await _userManager.UpdateAsync(user);

            return await EventDetails(id);
        }
    }
}