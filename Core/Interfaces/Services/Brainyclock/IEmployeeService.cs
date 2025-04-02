using Core.Entities.Brainyclock;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services.Brainyclock
{
    public interface IEmployeeService
    {
        Task<GenericResponse<List<Employee>>> PostEmployeesToExternalApi();
    }
}
