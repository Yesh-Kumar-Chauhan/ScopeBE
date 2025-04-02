using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SchedularTimesheetRepository : ISchedularTimesheetRepository
{
    private readonly AppDbContext _context;

    public SchedularTimesheetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SchedularTimesheet>> GetSchedularTimesheetsAsync()
    {
        // Include related entities like Personel, Schedule, and Site
        return await _context.SchedularTimesheets
                             .Include(st => st.Personel)
                             .Include(st => st.Schedule)
                             .Include(st => st.Site)
                             .ToListAsync();
    }

    public async Task<SchedularTimesheet> GetSchedularTimesheetByIdAsync(long id)
    {
        // Include related entities by Id
        return await _context.SchedularTimesheets
                             .Include(st => st.Personel)
                             .Include(st => st.Schedule)
                             .Include(st => st.Site)
                             .FirstOrDefaultAsync(st => st.Id == id);
    }

    public async Task<SchedularTimesheet> AddSchedularTimesheetAsync(SchedularTimesheet timesheet)
    {
        await _context.SchedularTimesheets.AddAsync(timesheet);
        await _context.SaveChangesAsync();
        return timesheet;
    }

    public async Task<SchedularTimesheet> UpdateSchedularTimesheetAsync(SchedularTimesheet timesheet)
    {
        _context.SchedularTimesheets.Update(timesheet);
        await _context.SaveChangesAsync();
        return timesheet;
    }

    public async Task<bool> DeleteSchedularTimesheetAsync(long id)
    {
        var timesheet = await _context.SchedularTimesheets.FindAsync(id);
        if (timesheet == null)
            return false;

        _context.SchedularTimesheets.Remove(timesheet);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<SchedularTimesheet> GetSchedularTimesheetsByScheduleIdAsync(long scheduleId)
    {
        return await _context.SchedularTimesheets
            .Include(st => st.Personel)
            .Include(st => st.Schedule)
            .Include(st => st.Site)
            .Where(st => st.ScheduleId == scheduleId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<SchedularTimesheet>> GetSchedularTimesheetsByPersonalIdAsync(long personId)
    {
        return await _context.SchedularTimesheets
              .Include(st => st.Personel)
              .Include(st => st.Schedule)
              .Include(st => st.Site)
              .Where(st => st.PersonID == personId)
              .ToListAsync();
    }
}
