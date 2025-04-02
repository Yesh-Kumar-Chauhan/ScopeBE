using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IWorkshopTypeRepository
    {
        Task<List<WorkshopType>> GetAllWorkshopTypesAsync();
        Task<WorkshopType> GetWorkshopTypeByIdAsync(long workshopTypeId);
    }
}
