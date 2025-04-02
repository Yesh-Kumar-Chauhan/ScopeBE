using Core.DTOs.Inservice;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IInserviceRepository
    {
        IQueryable<Inservice> GetAllInservices();
        Task<List<Inservice>> GetAllInservicesAsync();
        Task<Inservice> GetInserviceByIdAsync(long id);
        Task<Inservice> AddInserviceAsync(Inservice inservice);
        Task<Inservice> UpdateInserviceAsync(Inservice inservice);
        Task<bool> DeleteInserviceAsync(long id);
        Task<List<InserviceDto>> GetInserviceSelectAsync(long? personId, int operation);
        Task BulkInsertInservicesAsync(List<Inservice> inservices);
    }
}
