using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly AppDbContext _context;

        public VisitRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Visit> GetAllVisits()
        {
            return _context.Visits.AsQueryable();
        }

        public async Task<List<Visit>> GetAllVisitsAsync()
        {
            return await _context.Visits.ToListAsync();
        }

        public async Task<Visit> GetVisitByIdAsync(long id)
        {
            return await _context.Visits.FindAsync(id);
        }

        public async Task<Visit> AddVisitAsync(Visit visit)
        {
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
            return visit;
        }

        public async Task<Visit> UpdateVisitAsync(Visit visit)
        {
            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();
            return visit;
        }

        public async Task<bool> DeleteVisitAsync(long id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return false;
            }

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Visit> GetVisitsBySiteId(long siteId)
        {
            return _context.Visits.Where(v => v.SiteID == siteId);
        }

    }
}
