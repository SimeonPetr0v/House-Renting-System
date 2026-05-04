using House_Renting_System.Models.House.Helpers;
using House_Renting_System.Services.Contracts;
using House_Renting_System.Services.Models;
using HouseRentingSystem.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House_Renting_System.Services.Implementations
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext context;

        public HouseService(HouseRentingDbContext context)
        {
            this.context = context;
        }

        public async Task<AllHousesQueryModel> GetAllHousesAsync(AllHousesQueryModel query, string? currentUserId)
        {
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
                housesQuery = housesQuery.Where(h => h.Category.Name == query.Category);
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
                    CurentUserIsOwner = h.AgentId == currentUserId
                })
                .ToListAsync();

            query.Categories = await context.Categories
                .AsNoTracking()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();

            return query;
        }

        public async Task<IEnumerable<HouseViewModel>> GetHousesByUserId(string userId)
        {
            return await context.Houses
                .AsNoTracking()
                .Where(h => h.AgentId == userId && h.IsDeleted == false)
                .Select(h => new HouseViewModel
                {
                    Id = h.Id,
                    Name = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                    CurentUserIsOwner = true
                })
                .ToListAsync();
        }

        public async Task<HouseDetailViewModel?> GetHouseDetailsAsync(int id)
        {
            return await context.Houses
                .Include(h => h.Agent)
                .AsNoTracking()
                .Where(h => h.Id == id && h.IsDeleted == false)
                .Select(h => new HouseDetailViewModel
                {
                    Id = h.Id,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    Description = h.Description,
                    CreatedBy = h.Agent.UserName,
                    Price = h.PricePerMonth,
                    Name = h.Title
                })
                .FirstOrDefaultAsync();
        }

        public async Task<HouseFormViewModel> GetCreateHouseFormModelAsync()
        {
            return new HouseFormViewModel
            {
                Categories = await GetCategoriesAsync()
            };
        }

        public async Task CreateHouseAsync(HouseFormViewModel model, string userId)
        {
            var newHouse = new HouseRentingSystem.Data.Data.Entities.House
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
        }

        public async Task<HouseFormViewModel?> GetHouseForEditAsync(int id)
        {
            return await context.Houses
                .AsNoTracking()
                .Where(h => h.Id == id && h.IsDeleted == false)
                .Select(h => new HouseFormViewModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    Description = h.Description,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    SelectedCategoryId = h.CategoryId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EditHouseAsync(HouseFormViewModel model)
        {
            var house = await context.Houses.FindAsync(model.Id);

            if (house == null || house.IsDeleted)
            {
                return false;
            }

            house.Title = model.Title;
            house.Address = model.Address;
            house.Description = model.Description;
            house.ImageUrl = model.ImageUrl;
            house.PricePerMonth = model.PricePerMonth;
            house.CategoryId = model.SelectedCategoryId;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteHouseAsync(int id)
        {
            var house = await context.Houses.FindAsync(id);

            if (house == null || house.IsDeleted)
            {
                return false;
            }

            house.IsDeleted = true;

            await context.SaveChangesAsync();

            return true;
        }

        private async Task<List<CategoryViewModel>> GetCategoriesAsync()
        {
            return await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
