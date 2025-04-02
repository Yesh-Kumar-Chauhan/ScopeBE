using Core.DTOs.Personel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IDirectorRepository
    {
        Task<List<DirectorDto>> GetDirectorsByPersonIdAsync(long personId);
        Task<Director> GetByIdAsync(long directorId);
        Task<Director> AddAsync(Director director);
        Task<Director> UpdateAsync(Director director);
        Task<bool> DeleteAsync(long directorId);
    }
}
