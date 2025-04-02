using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IDistrictRepository
    {
        IQueryable<District> GetAllDistricts();
        Task<List<District>> GetAllDistrictsAsync();
        Task<District> GetDistrictByIdAsync(long id);
        Task<District> GetDistrictByDistNumberAsync(long id);
        Task<District> AddDistrictAsync(District district);
        Task<District> UpdateDistrictAsync(District district);
        Task<bool> DeleteDistrictAsync(long id);

        Task<List<District>> GetDistrictsAsync(string keyword, int operation);
        Task<List<District>> GetAllDistrictsByIdAsync(List<long?> districtIds);
    }
}
