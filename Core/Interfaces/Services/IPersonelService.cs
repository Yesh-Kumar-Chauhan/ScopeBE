using Core.DTOs;
using Core.DTOs.Personel;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IPersonelService
    {
        //crud
        Task<GenericResponse<IEnumerable<PersonelDto>>> GetFilteredPersonelAsync(string? search, int page, int pageSize);
        Task<GenericResponse<IEnumerable<PersonelDto>>> GetAllPersonelAsync();
        Task<GenericResponse<PersonelDto>> GetPersonelByIdAsync(long id);
        Task<GenericResponse<PersonelDto>> GetPersonelByEmailAsync(string email);
        Task<GenericResponse<PersonelDto>> CreatePersonelAsync(PersonelDto personelDto);
        Task<GenericResponse<PersonelDto>> UpdatePersonelAsync(long id, PersonelDto personelDto);
        Task<GenericResponse<bool>> DeletePersonelAsync(long id);

        //dropdown
        Task<GenericResponse<List<ExtendedPersonelDto>>> GetPersonelByKeywordAndOperationAsync(string keyword, int operation, int? page, int? pageSize);
        //Task<GenericResponse<List<ScheduleDto>>> GetPersonelScheduleExcel();
        Task<GenericResponse<Dictionary<string, byte[]>>> GetPersonelScheduleExcel();
    }
}
