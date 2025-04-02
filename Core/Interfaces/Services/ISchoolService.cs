using Core.DTOs;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ISchoolService
    {
        Task<GenericResponse<IEnumerable<SchoolDto>>> GetSchoolsWithFilterAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<SchoolDto>>> GetAllSchoolsAsync();
        Task<GenericResponse<SchoolDto>> GetSchoolByIdAsync(long id);
        Task<GenericResponse<SchoolDto>> CreateSchoolAsync(SchoolDto schoolDto);
        Task<GenericResponse<SchoolDto>> UpdateSchoolAsync(long id, SchoolDto schoolDto);
        Task<GenericResponse<bool>> DeleteSchoolAsync(long id);
    }
}
