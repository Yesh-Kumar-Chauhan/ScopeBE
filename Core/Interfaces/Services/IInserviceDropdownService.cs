using Core.Entities;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IInserviceDropdownService
    {
        Task<GenericResponse<List<TopicType>>> GetTopicTypesAsync();
        Task<GenericResponse<List<WorkshopType>>> GetWorkshopTypesAsync();
    }
}
