using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AppDbContext _context;

        public StatusRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Status> GetAllStatuses()
        {
            return _context.Status.AsQueryable();
        }

        public async Task<List<Status>> GetAllStatusesAsync()
        {
            return await _context.Status.ToListAsync();
        }

        public async Task<Status?> GetStatusByIdAsync(long id)
        {
            return await _context.Status.FindAsync(id);
        }

        public async Task<Status> AddStatusAsync(Status status)
        {
            _context.Status.Add(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<Status?> UpdateStatusAsync(Status status)
        {
            var existingStatus = await _context.Status.FindAsync(status.StatusID);
            if (existingStatus == null)
                return null;

            _context.Entry(existingStatus).CurrentValues.SetValues(status);
            await _context.SaveChangesAsync();
            return existingStatus;
        }

        public async Task<bool> DeleteStatusAsync(long id)
        {
            var status = await _context.Status.FindAsync(id);
            if (status == null)
                return false;

            _context.Status.Remove(status);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
