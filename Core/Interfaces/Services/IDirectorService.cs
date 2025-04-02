using Core.DTOs.Personel;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IDirectorService
    {
        Task<GenericResponse<List<DirectorDto>>> GetDirectorsByPersonIdAsync(long personId);
        Task<GenericResponse<DirectorDto>> GetDirectorByIdAsync(long directorId);
        Task<GenericResponse<DirectorDto>> SaveDirectorAsync(DirectorDto directorDto);
        Task<GenericResponse<DirectorDto>> UpdateDirectorAsync(DirectorDto directorDto);
        Task<GenericResponse<string>> DeleteDirectorAsync(long directorId);
    }
}
