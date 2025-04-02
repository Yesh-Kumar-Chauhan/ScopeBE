using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly AppDbContext _context;

        public SchoolRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<School> GetAllSchools()
        {
            return _context.Schools.AsQueryable();
        }

        public async Task<IEnumerable<School>> GetAllSchoolsAsync()
        {
            return await _context.Schools.ToListAsync();
        }

        public async Task<School> GetSchoolByIdAsync(long id)
        {
            return await _context.Schools.FindAsync(id);
        }

        public async Task<School> AddSchoolAsync(School school)
        {
            await _context.Schools.AddAsync(school);
            await _context.SaveChangesAsync();
            return school;
        }

        public async Task<School> UpdateSchoolAsync(School school)
        {
            _context.Schools.Update(school);
            await _context.SaveChangesAsync();
            return school;
        }

        public async Task<bool> DeleteSchoolAsync(long id)
        {
            var school = await GetSchoolByIdAsync(id);
            if (school == null)
            {
                return false;
            }

            _context.Schools.Remove(school);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<School>> GetAllSchoolsByDistrictNumbers(List<long> distNums)
        {
            var distNumStrings = distNums.Select(d => d.ToString()).ToList();
            return await _context.Schools
                .Where(s => distNumStrings.Contains(s.DIST_NUM))
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
