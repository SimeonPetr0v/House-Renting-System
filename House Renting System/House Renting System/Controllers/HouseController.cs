using House_Renting_System.Services.Models;
using House_Renting_System.Models.House.Helpers;
using HouseRentingSystem.Data.Data;
using HouseRentingSystem.Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using House_Renting_System.Services.Contracts;

namespace House_Renting_System.Controllers
{
    public class HouseController : Controller
    {
        private readonly IHouseService houseService;

        public HouseController(IHouseService houseService)
        {
            this.houseService = houseService;
        }

        [HttpGet]
        public async Task<IActionResult> AllHouses([FromQuery] AllHousesQueryModel query)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await houseService.GetAllHousesAsync(query, currentUserId);

            ViewBag.Title = "All houses";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await houseService.GetHouseDetailsAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateHouse()
        {
            var model = await houseService.GetCreateHouseFormModelAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHouse(HouseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var formModel = await houseService.GetCreateHouseFormModelAsync();
                model.Categories = formModel.Categories;

                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            await houseService.CreateHouseAsync(model, userId);

            return RedirectToAction(nameof(AllHouses));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyHouses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var model = await houseService.GetHousesByUserId(userId);

            ViewBag.Title = "My houses";

            return View(nameof(AllHouses), model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await houseService.GetHouseForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            var formModel = await houseService.GetCreateHouseFormModelAsync();
            model.Categories = formModel.Categories;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HouseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var formModel = await houseService.GetCreateHouseFormModelAsync();
                model.Categories = formModel.Categories;

                return View(model);
            }

            var updated = await houseService.EditHouseAsync(model);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(MyHouses));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await houseService.DeleteHouseAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(MyHouses));
        }
    }
}