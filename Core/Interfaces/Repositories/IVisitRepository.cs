using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IVisitRepository
    {
        IQueryable<Visit> GetAllVisits();
        Task<List<Visit>> GetAllVisitsAsync();
        Task<Visit> GetVisitByIdAsync(long id);
        Task<Visit> AddVisitAsync(Visit visit);
        Task<Visit> UpdateVisitAsync(Visit visit);
        Task<bool> DeleteVisitAsync(long id);
        // Add this method to the interface
        IQueryable<Visit> GetVisitsBySiteId(long siteId);

    }

}
