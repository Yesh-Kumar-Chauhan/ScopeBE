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
    public class WorkshopTypeRepository : IWorkshopTypeRepository
    {
        private readonly AppDbContext _context;

        public WorkshopTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkshopType>> GetAllWorkshopTypesAsync()
        {
            return await _context.WorkshopType.ToListAsync();
        }

        public async Task<WorkshopType> GetWorkshopTypeByIdAsync(long workshopTypeId)
        {
            return await _context.WorkshopType.FindAsync(workshopTypeId);
        }
    }
}
