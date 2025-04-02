using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ISchoolRepository
    {
        Task SaveChangesAsync();
        IQueryable<School> GetAllSchools(); // For filtering and pagination
        Task<IEnumerable<School>> GetAllSchoolsAsync();
        Task<School> GetSchoolByIdAsync(long id);
        Task<School> AddSchoolAsync(School school);
        Task<School> UpdateSchoolAsync(School school);
        Task<bool> DeleteSchoolAsync(long id);
        Task<List<School>> GetAllSchoolsByDistrictNumbers(List<long> distNums);
    }
}
