using House_Renting_System.Models.House;
using House_Renting_System.Models.House.Helpers;
using HouseRentingSystem.Data.Data;
using HouseRentingSystem.Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace House_Renting_System.Controllers
{
    public class HouseController : Controller
    {
        private List<HouseViewModel> houses = new List<HouseViewModel>()
        {
            new HouseViewModel()
            {
                Id = 1,
                Name = "Beach House",
                Address = "Miami, Florida",
                ImageUrl = @"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQvcje9el9YUxSqN4VSt3llpb6su9ghN-_ZbA&s"


            },
            new HouseViewModel()
            {
                Id = 2,
                Name = "Mountain House",
                Address = "Rila Mountain, Bulgaria",
                ImageUrl = @"https://bghike.com/en/images/huts_pic/rila_lakes_main.jpg"

            },

            new HouseViewModel()
            {
                Id = 3,
                Name = "Urban House",
                Address = "Luylin, Sofia",
                ImageUrl = @"https://cdnp.ues.bg/projects/watermark_thumbs_768/257/180582.jpg"

            }
        };
        public IActionResult AllHouses()
        {
            return View(houses);
        }
        public IActionResult Details(int id)
        {
            return View(houses.FirstOrDefault(h => h.Id == id));
        }
    }
}
public class HouseController : Controller
{
    private readonly HouseRentingDbContext context;

    public HouseController(HouseRentingDbContext context)
    {
        this.context = context;
    }
    [HttpGet]
    public async Task<IActionResult> AllHouses()
    {
        var currentUsersId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var housesViewModel = await context.Houses
        .AsNoTracking()
        .Select(h => new HouseViewModel
        {
            Id = h.Id,
            Name = h.Title,
            Address = h.Address,
            ImageUrl = h.ImageUrl,
            CurentUserIsOwner = h.AgentId == currentUsersId
        })
        .ToListAsync();
        ViewBag.Title = "All houses";
        return View(housesViewModel);
    }
    [HttpGet]
    public async Task<IActionResult> Details(int Id)
    {
        var searched = await context.Houses
            .Include(h => h.Agent)
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == Id);

        var model = new HouseDetailViewModel()
        {
            Id = searched.Id,
            Address = searched.Address,
            ImageUrl = searched.ImageUrl,
            Description = searched.Description,
            CreatedBy = searched.Agent.UserName,
            Price = searched.PricePerMonth,
            Name = searched.Title
        };

        return View(model);
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> CreateHouse()
    {
        List<CategoryViewModel> ListOfCategories = await context.Categories
        .AsNoTracking()
        .Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name,
        })
        .ToListAsync();
        var houseCategories = new HouseFormViewModel()
        {
            Categories = ListOfCategories
        };
        return View(houseCategories);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateHouse(HouseFormViewModel model)
    {

        var houseCategories = await context.Categories
            .AsNoTracking()
            .Select(c => new CategoryViewModel()
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToListAsync();

        if (!ModelState.IsValid)
        {

            model.Categories = houseCategories;
            return View(model);
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        bool addressExists = await context.Houses
            .AnyAsync(h => h.Address.ToLower() == model.Address.ToLower());

        if (addressExists)
        {
            model.Categories = houseCategories;
            ModelState.AddModelError("Address", "This address is already registered");
            return View(model);
        }

        var newHouse = new House
        {
            Title = model.Title,
            Address = model.Address,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            PricePerMonth = model.PricePerMonth,
            CategoryId = model.SelectedCategoryId,
            AgentId = userId
        };

        context.Houses.Add(newHouse);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(AllHouses));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> MyHouses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var houses = context.Houses
            .Where(h => h.AgentId == userId)
            .Select(h => new HouseViewModel
            {
                Address = h.Address,
                ImageUrl = h.ImageUrl,
                Name = h.Title,
                Id = h.Id,
                CurentUserIsOwner = true
            })
            .ToListAsync();
        ViewBag.Title = "My houses";
        return View(nameof(AllHouses), houses);
    }
}
