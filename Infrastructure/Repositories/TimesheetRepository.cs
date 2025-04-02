using Core.Entities;
using Core.Interfaces.Repositories;
using Dapper;
using EFCore.BulkExtensions;
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
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly AppDbContext _context;

        public TimesheetRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Timesheet> GetAllTimesheets()
        {
            return _context.Timesheet.AsQueryable();
        }

        public async Task<List<Timesheet>> GetAllTimesheetsAsync()
        {
            return await _context.Timesheet.ToListAsync();
        }

        public async Task<Timesheet?> GetTimesheetByIdAsync(long id)
        {
            return await _context.Timesheet.FindAsync(id);
        }

        public async Task<Timesheet?> GetTimesheetBySiteAndDateAsync(DateTime? date, long? siteId, long? personId)
        {
            return await _context.Timesheet.FirstOrDefaultAsync(x => x.TimeSheetDate.Value.Date == date.Value.Date && x.SiteID == siteId && x.PersonID == personId);
        }

        public async Task<Timesheet> AddTimesheetAsync(Timesheet timesheet)
        {
            _context.Timesheet.Add(timesheet);
            await _context.SaveChangesAsync();
            return timesheet;
        }

        public async Task<Timesheet?> UpdateTimesheetAsync(Timesheet timesheet)
        {
            var existingTimesheet = await _context.Timesheet.FindAsync(timesheet.TimesheetID);
            if (existingTimesheet == null)
                return null;

            _context.Entry(existingTimesheet).CurrentValues.SetValues(timesheet);
            await _context.SaveChangesAsync();
            return existingTimesheet;
        }

        public async Task<bool> DeleteTimesheetAsync(long id)
        {
            var timesheet = await _context.Timesheet.FindAsync(id);
            if (timesheet == null)
                return false;

            _context.Timesheet.Remove(timesheet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<dynamic>> GetTimesheetsByPersonIdAsync(long personId, DateTime? startDate, DateTime? endDate)
        {
            

            //var personIdParam = new SqlParameter("@PersonID", SqlDbType.BigInt)
            //{
            //    Value = personId == 0 ? (object)DBNull.Value : personId
            //};

            //var startDateParam = new SqlParameter("@StartDate", SqlDbType.Date)
            //{
            //    Value = startDate.HasValue ? (object)startDate.Value : DBNull.Value
            //};

            //var endDateParam = new SqlParameter("@EndDate", SqlDbType.Date)
            //{
            //    Value = endDate.HasValue ? (object)endDate.Value : DBNull.Value
            //};

            var timesheets = await _context.Database
                .GetDbConnection()
                .QueryAsync<dynamic>("EXEC sp_Timesheet_Select @PersonID, @StartDate, @EndDate", new { PersonID = personId, StartDate = startDate, EndDate = endDate });
                
            return timesheets;
        }

        public async Task<List<Timesheet>> GetTimesheetByDateAsync(DateTime date)
        {
            return await _context.Timesheet
                .Where(x => x.TimeSheetDate.HasValue && x.TimeSheetDate.Value.Date == date.Date)
                .ToListAsync();
        }

        public async Task<Timesheet?> GetTimesheetByExternalIdAsync(long id)
        {
            return await _context.Timesheet
                                 .FirstOrDefaultAsync(x => x.ExternalEventId == id);
        }

        public async Task<List<Timesheet>> BulkUpdateTimesheetsAsync(List<Timesheet> timesheets)
        {
            await _context.BulkUpdateAsync(timesheets);
            return timesheets; // Return the updated timesheets

        }

        public async Task<List<Timesheet>> BulkAddTimesheetsAsync(List<Timesheet> timesheets)
        {
            await _context.BulkInsertAsync(timesheets);
            return timesheets;
        }

        public async Task<IEnumerable<dynamic>> GetTimesheetsBySiteAndDistrictAsync(
            int siteId, int distNumber, DateTime? startDate, DateTime? endDate)
        {
            // Create SQL parameters
            var siteIdParam = new SqlParameter("@SiteID", SqlDbType.Int)
            {
                Value = siteId
            };

            var distNumberParam = new SqlParameter("@DistNumber", SqlDbType.Int)
            {
                Value = distNumber
            };

            var startDateParam = new SqlParameter("@StartDate", SqlDbType.Date)
            {
                Value = startDate.HasValue ? (object)startDate.Value : DBNull.Value
            };

            var endDateParam = new SqlParameter("@EndDate", SqlDbType.Date)
            {
                Value = endDate.HasValue ? (object)endDate.Value : DBNull.Value
            };

            // Execute stored procedure and fetch results
            var result = await _context.Database
                .GetDbConnection()
                .QueryAsync<dynamic>(
                    "EXEC sp_Timesheet_Select_ByPersonIds @SiteID, @DistNumber, @StartDate, @EndDate",
                    new { SiteID = siteId, DistNumber = distNumber, StartDate = startDate, EndDate = endDate });

            return result;
        }

    }
}
