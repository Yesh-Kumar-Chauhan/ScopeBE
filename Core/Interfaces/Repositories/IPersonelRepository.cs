using Core.DTOs.Personel;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IPersonelRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        IQueryable<Personel> GetAllPersonel(); // For search and pagination
        Task<IEnumerable<Personel>> GetAllPersonelAsync(); // Optional: For simple retrieval without filtering
        Task<Personel?> GetPersonelByIdAsync(long id); // Fetch a personel by ID
        Task<Personel?> GetPersonelByEmailAsync(string email); // Fetch a personel by Email
        Task<Personel> AddPersonelAsync(Personel personel); // Add a new personel
        Task<Personel?> UpdatePersonelAsync(Personel personel); // Update an existing personel
        Task<bool> DeletePersonelAsync(long id); // Delete a personel by ID


        //DropDown 
        Task<List<ExtendedPersonelDto>> GetPersonelByKeywordAndOperationAsync(string keyword, int operation);
        Task BulkUpdatePersonnelAsync(List<Personel> personnel);
        Task<List<Personel>> GetPersonelsByIdsAsync(List<long?> personnelIds);
    }
}
