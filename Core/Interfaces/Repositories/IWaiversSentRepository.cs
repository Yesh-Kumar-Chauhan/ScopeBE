using Core.DTOs.Personel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IWaiversSentRepository
    {
        Task<List<WaiversSent>> GetWaiversByStaffIdAsync(long staffId);
        Task<WaiversSent> GetByIdAsync(long waiverId);
        Task<WaiversSent> AddAsync(WaiversSent waiver);
        Task<WaiversSent> UpdateAsync(WaiversSent waiver);
        Task<bool> DeleteAsync(long waiverId);
        Task<List<WaiversReceivedDto>> GetWaiversReceivedByStaffId(long staffId);
        Task<bool> AddOrUpdateWaiversReceivedAsync(List<WaiversReceived> waivers);
    }
}
