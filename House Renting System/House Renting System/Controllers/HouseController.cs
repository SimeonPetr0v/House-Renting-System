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
        private readonly HouseRentingDbContext context;

        public HouseController(HouseRentingDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AllHouses([FromQuery] AllHousesQueryModel query)
        {
            var currentUsersId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (query.CurrentPage < 1)
            {
                query.CurrentPage = 1;
            }

            var housesQuery = context.Houses
                .AsNoTracking()
                .Where(h => h.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                housesQuery = housesQuery
                    .Where(h => h.Category.Name == query.Category);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string search = query.SearchTerm.ToLower();

                housesQuery = housesQuery.Where(h =>
                    h.Title.ToLower().Contains(search) ||
                    h.Address.ToLower().Contains(search) ||
                    h.Description.ToLower().Contains(search));
            }

            housesQuery = query.Sorting switch
            {
                HouseSorting.Price => housesQuery.OrderBy(h => h.PricePerMonth),

                HouseSorting.NotRentedFirst => housesQuery
                    .OrderBy(h => h.RenterId != null)
                    .ThenByDescending(h => h.Id),

                _ => housesQuery.OrderByDescending(h => h.Id)
            };

            query.TotalHousesCount = await housesQuery.CountAsync();

            query.Houses = await housesQuery
                .Skip((query.CurrentPage - 1) * AllHousesQueryModel.HousesPerPage)
                .Take(AllHousesQueryModel.HousesPerPage)
                .Select(h => new HouseViewModel
                {
                    Id = h.Id,
                    Name = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                    CurentUserIsOwner = h.AgentId == currentUsersId
                })
                .ToListAsync();

            query.Categories = await context.Categories
                .AsNoTracking()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();

            ViewBag.Title = "All houses";

            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var searched = await context.Houses
                .Include(h => h.Agent)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);

            if (searched == null)
            {
                return NotFound();
            }

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
            List<CategoryViewModel> listOfCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            var houseCategories = new HouseFormViewModel()
            {
                Categories = listOfCategories
            };

            return View(houseCategories);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHouse(HouseFormViewModel model)
        {
            var houseCategories = await GetCategories();

            if (!ModelState.IsValid)
            {
                model.Categories = houseCategories;
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //bool addressExists = await context.Houses
                //.AnyAsync(h => h.Address.ToLower() == model.Address.ToLower());

            //if (addressExists)
            //{
                //model.Categories = houseCategories;
                //ModelState.AddModelError("Address", "This address is already registered");
                //return View(model);
            //}

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

            var houses = await context.Houses
                .AsNoTracking()
                .Where(h => h.AgentId == userId && h.IsDeleted == false)
                .Select(h => new HouseViewModel
                {
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    Name = h.Title,
                    Id = h.Id,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                    CurentUserIsOwner = true
                })
                .ToListAsync();

            var model = new AllHousesQueryModel
            {
                Houses = houses,
                TotalHousesCount = houses.Count
            };

            ViewBag.Title = "My houses";

            return View(nameof(AllHouses), model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var house = await context.Houses.FindAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            var houseCategories = await GetCategories();

            var model = new HouseFormViewModel()
            {
                Id = house.Id,
                Address = house.Address,
                ImageUrl = house.ImageUrl,
                Description = house.Description,
                Title = house.Title,
                PricePerMonth = house.PricePerMonth,
                SelectedCategoryId = house.CategoryId,
                Categories = houseCategories,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(HouseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();
                return View(model);
            }

            var house = await context.Houses.FindAsync(model.Id);

            if (house == null)
            {
                return NotFound();
            }

            house.PricePerMonth = model.PricePerMonth;
            house.Address = model.Address;
            house.ImageUrl = model.ImageUrl;
            house.Description = model.Description;
            house.Title = model.Title;
            house.CategoryId = model.SelectedCategoryId;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(MyHouses));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var house = await context.Houses.FindAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            house.IsDeleted = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(MyHouses));
        }

        private async Task<List<CategoryViewModel>> GetCategories()
        {
            return await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
        }
    }
}