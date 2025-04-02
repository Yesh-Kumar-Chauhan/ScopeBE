using AutoMapper;
using Core.DTOs.Personel;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Personal
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DirectorService> _logger;

        public DirectorService(IDirectorRepository directorRepository, IMapper mapper, ILogger<DirectorService> logger)
        {
            _directorRepository = directorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<List<DirectorDto>>> GetDirectorsByPersonIdAsync(long personId)
        {
            try
            {
                var directors = await _directorRepository.GetDirectorsByPersonIdAsync(personId);
                //var directorDto = _mapper.Map<List<DirectorDto>>(directors);
                return new GenericResponse<List<DirectorDto>>(true, "Directors retrieved successfully.", directors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving directors.");
                return new GenericResponse<List<DirectorDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<DirectorDto>> GetDirectorByIdAsync(long directorId)
        {
            try
            {
                var director = await _directorRepository.GetByIdAsync(directorId);
                if (director == null)
                {
                    return new GenericResponse<DirectorDto>(false, "Director not found.", null);
                }

                var directorDto = _mapper.Map<DirectorDto>(director);
                return new GenericResponse<DirectorDto>(true, "Director retrieved successfully.", directorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the director.");
                return new GenericResponse<DirectorDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<DirectorDto>> SaveDirectorAsync(DirectorDto directorDto)
        {
            try
            {
                var director = _mapper.Map<Director>(directorDto);

                var newDirector = await _directorRepository.AddAsync(director);
                directorDto = _mapper.Map<DirectorDto>(newDirector);

                return new GenericResponse<DirectorDto>(true, "Director saved successfully.", directorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the director.");
                return new GenericResponse<DirectorDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<DirectorDto>> UpdateDirectorAsync(DirectorDto directorDto)
        {
            try
            {
                var director = _mapper.Map<Director>(directorDto);

                if (directorDto.DirectorID > 0)
                {
                    var updatedDirector = await _directorRepository.UpdateAsync(director);
                    if (updatedDirector == null)
                    {
                        return new GenericResponse<DirectorDto>(false, "Director not found.", null);
                    }
                    directorDto = _mapper.Map<DirectorDto>(updatedDirector);
                }

                return new GenericResponse<DirectorDto>(true, "Director updated successfully.", directorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the director.");
                return new GenericResponse<DirectorDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<string>> DeleteDirectorAsync(long directorId)
        {
            try
            {
                var success = await _directorRepository.DeleteAsync(directorId);
                if (!success)
                {
                    return new GenericResponse<string>(false, "Director not found.", null);
                }

                return new GenericResponse<string>(true, "Director deleted successfully.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the director.");
                return new GenericResponse<string>(false, ex.Message, null);
            }
        }
    }
}
