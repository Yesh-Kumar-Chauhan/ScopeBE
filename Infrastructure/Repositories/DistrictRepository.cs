using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly AppDbContext _context;

        public DistrictRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<District> GetAllDistricts()
        {
            return _context.Districts.AsQueryable();
        }

        public async Task<List<District>> GetAllDistrictsAsync()
        {
            return await _context.Districts.ToListAsync();
        }

        public async Task<District> GetDistrictByIdAsync(long id)
        {
            return await _context.Districts.FindAsync(id);
        }

        public async Task<District> AddDistrictAsync(District district)
        {
            _context.Districts.Add(district);
            await _context.SaveChangesAsync();
            return district;
        }

        public async Task<District> UpdateDistrictAsync(District district)
        {
            _context.Entry(district).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return district;
        }

        public async Task<bool> DeleteDistrictAsync(long id)
        {
            var district = await _context.Districts.FindAsync(id);
            if (district == null) return false;

            _context.Districts.Remove(district);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<District>> GetDistrictsAsync(string keyword, int operation)
        {
            var districts = new List<District>();

            var keywordParam = new SqlParameter("@KeyWord", SqlDbType.NVarChar)
            {
                Value = string.IsNullOrEmpty(keyword) ? (object)DBNull.Value : keyword
            };
            var operationParam = new SqlParameter("@Operation", SqlDbType.Int)
            {
                Value = operation
            };

            districts = await _context.Districts
                .FromSqlRaw("EXEC sp_Districts_Select @KeyWord, @Operation", keywordParam, operationParam)
                .ToListAsync();

            return districts;
        }

        public async Task<List<District>> GetAllDistrictsByIdAsync(List<long?> districtIds)
        {
            return await _context.Districts
                   .Where(d => districtIds.Contains(d.DistrictID))
                  .ToListAsync();
        }

        public async Task<District> GetDistrictByDistNumberAsync(long id)
        {
            return await _context.Districts.FirstOrDefaultAsync(x => x.DIST_NUM == id);
        }
    }
}
