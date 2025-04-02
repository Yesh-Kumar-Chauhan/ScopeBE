using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IClosingRepository
    {
        IQueryable<Closing> GetAllClosings();
        Task<List<Closing>> GetAllClosingsAsync();
        Task<Closing?> GetClosingByIdAsync(long id);
        Task<Closing> AddClosingAsync(Closing closing);
        Task<Closing?> UpdateClosingAsync(Closing closing);
        Task<bool> DeleteClosingAsync(long id);
        Task<List<ClosingDto>> GetClosingsByDistrictIdAsync(long? districtId);
    }
}
