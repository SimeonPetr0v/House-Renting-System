using House_Renting_System.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House_Renting_System.Services.Contracts
{
    public interface IHouseService
    {
        Task<AllHousesQueryModel> GetAllHousesAsync(AllHousesQueryModel query, string? currentUserId);

        Task<IEnumerable<HouseViewModel>> GetHousesByUserId(string userId);

        Task<HouseDetailViewModel?> GetHouseDetailsAsync(int id);

        Task<HouseFormViewModel> GetCreateHouseFormModelAsync();

        Task CreateHouseAsync(HouseFormViewModel model, string userId);

        Task<HouseFormViewModel?> GetHouseForEditAsync(int id);

        Task<bool> EditHouseAsync(HouseFormViewModel model);

        Task<bool> DeleteHouseAsync(int id);
    }
}