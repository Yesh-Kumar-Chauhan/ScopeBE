using Core.DTOs.Core.DTOs;
using Core.DTOs.Site;
using Core.DTOs.Sites;
using Core.Entities;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ISiteService
    {
        //crud
        Task<GenericResponse<IEnumerable<SiteDto>>> GetSitesWithFilterAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<SiteDto>>> GetAllSitesAsync();
        Task<GenericResponse<SiteDto>> GetSiteByIdAsync(long id);
        Task<GenericResponse<SiteDto>> CreateSiteAsync(SiteDto siteDto);
        Task<GenericResponse<SiteDto>> UpdateSiteAsync(long id, SiteDto siteDto);
        Task<GenericResponse<bool>> DeleteSiteAsync(long id);

        //sites
        Task<GenericResponse<Dictionary<string, List<SiteByType>>>> GetSitesByTypesAsync(List<int> types);
        Task<GenericResponse<List<ExtendedSiteDto>>> GetSitesByDistrictIdAsync(long districtId, long? districtNum = null, int operation = 0);
        Task<GenericResponse<List<SiteDto>>> GetSitesByOperationAsync(string keyword, int operation = 0);
        Task<GenericResponse<List<SiteNamesDto>>> GetSitesByIdsAsync(List<long?> ids);
    }
}
