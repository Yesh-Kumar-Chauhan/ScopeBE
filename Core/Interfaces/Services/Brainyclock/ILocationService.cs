using Core.Entities;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services.Brainyclock
{
    public interface ILocationService
    {
        Task<GenericResponse<LocationModal>> AddLocationAsync(LocationModal locationInput);
        Task<GenericResponse<List<Site>>> SyncSiteAndLocationAsync();
    }
}
