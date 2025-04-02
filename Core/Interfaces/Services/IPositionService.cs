using Core.Entities;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IPositionService
    {
        Task<GenericResponse<List<Positions>>> GetPositionsAsync();
        Task<GenericResponse<Dictionary<string, List<Positions>>>> GetPositionsByTypesAsync(List<string> types);
    }
}
