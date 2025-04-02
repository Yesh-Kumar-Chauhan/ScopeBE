using Core.DTOs.Site;
using Core.DTOs.Sites;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ISiteRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        IQueryable<Site> GetAllSites(); // For search and pagination
        Task<List<Site>> GetAllSitesAsync(); // Optional: For simple retrieval without filtering
        Task<Site?> GetSiteByIdAsync(long id);
        Task<Site> AddSiteAsync(Site site);
        Task<Site?> UpdateSiteAsync(Site site);
        Task<bool> DeleteSiteAsync(long id);

        //dependent sites
        Task<List<SiteByType>> GetSitesByTypeAsync(string? search, int type);
        Task<List<ExtendedSiteDto>> GetSitesByDistrictIdAsync(long districtId, long? districtNum, int operation);
        Task<List<Site>> GetSitesByOperationAsync(string keyword, int operation);
        Task<List<Site>> GetAllSitesByIdAsync(List<long?> siteIds);
        Task<List<Site>> GetAllSitesBySiteNumber(List<long?> siteNumber);
        Task SaveChangesAsync();
    }
}
