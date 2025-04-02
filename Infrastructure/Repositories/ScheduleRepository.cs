using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext _context;

        public ScheduleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesAsync()
        {
            return await _context.Schedules.Include(p => p.Personel).ToListAsync();
        }

        public async Task<Schedule> GetScheduleByIdAsync(long id)
        {
            return await _context.Schedules.FindAsync(id);
        }

        public async Task<Schedule> AddScheduleAsync(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task DeleteScheduleByEmployeeAsync(long personnelId)
        {
            _context.Schedules.RemoveRange(_context.Schedules.Where(x => x.PersonID == personnelId));
            await _context.SaveChangesAsync();
        }


        public async Task<Schedule> UpdateScheduleAsync(Schedule schedule)
        {
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<bool> DeleteScheduleAsync(long id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteScheduleBySessionIdAsync(Guid sessionId)
        {
            var schedule = await _context.Schedules.Where(x => x.SessionId == sessionId).ToListAsync();
            if (schedule != null)
            {
                _context.Schedules.RemoveRange(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Schedule>> GetScheduleByPersonIdAsync(long personId)
        {
            return await _context.Schedules
                .Include(p => p.Personel)
                .Include(p => p.Site)
                .Where(p => p.PersonID == personId)
                .ToListAsync(); // Return a list of schedules
        }

        public async Task<List<Schedule>> GetSchedulesByPersonIdsAsync(List<long?> personIds)
        {
            return await _context.Schedules
                .Include(p => p.Personel) // Include related Personel data if required
                .Include(p => p.Site)     // Include related Site data if required
                .Where(p => personIds.Contains(p.PersonID)) // Filter by PersonIDs
                .ToListAsync();
        }

        public async Task<List<Schedule>> GetScheduleByPersonIdAndDateRangeAsync(long personId, DateTime startDate, DateTime endDate)
        {
            return await _context.Schedules
                .Where(s => s.PersonID == personId && s.Date >= startDate && s.Date <= endDate)
                .ToListAsync();
        }

        public async Task<List<Schedule>> AddSchedulesAsync(List<Schedule> schedules)
        {
            _context.Schedules.AddRange(schedules);
            await _context.SaveChangesAsync();
            return schedules;

        }
    }
}
