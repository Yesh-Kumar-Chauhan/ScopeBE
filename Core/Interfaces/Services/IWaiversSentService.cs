using Core.DTOs.Personel;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IWaiversSentService
    {
        Task<GenericResponse<List<WaiversSentDto>>> GetWaiversByStaffIdAsync(long staffId);
        Task<GenericResponse<WaiversSentDto>> GetWaiverByIdAsync(long waiverId);
        Task<GenericResponse<WaiversSentDto>> SaveWaiverAsync(WaiversSentDto waiverDto);
        Task<GenericResponse<WaiversSentDto>> UpdateWaiverAsync(WaiversSentDto waiverDto);
        Task<GenericResponse<string>> DeleteWaiverAsync(long waiverId);
        Task<GenericResponse<List<WaiversReceivedDto>>> GetWaiversReceivedByStaffId(long staffId);
        Task<GenericResponse<bool>> BulkUpdateWaiversReceivedAsync(List<WaiversReceivedDto> waiversDto);
    }
}
