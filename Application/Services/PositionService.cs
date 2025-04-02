using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PositionService : IPositionService
    {

        private readonly IPositionRepository _positionRepository;
        private readonly ILogger<PositionService> _logger;

        public PositionService(IPositionRepository positionRepository, ILogger<PositionService> logger)
        {
            _positionRepository = positionRepository;
            _logger = logger;
        }

        public async Task<GenericResponse<List<Positions>>> GetPositionsAsync()
        {
            try
            {
                var positions = await _positionRepository.GetPositionsByTypeAsync(null);
                return new GenericResponse<List<Positions>>(true, "Positions retrieved successfully.", positions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving positions by type.");
                return new GenericResponse<List<Positions>>(false, "An error occurred while retrieving positions.", null);
            }
        }

        public async Task<GenericResponse<Dictionary<string, List<Positions>>>> GetPositionsByTypesAsync(List<string> types)
        {
            var positionResults = new Dictionary<string, List<Positions>>();

            try
            {
                foreach (var type in types)
                {
                    var positions = await _positionRepository.GetPositionsByTypeAsync(type);

                    switch (type)
                    {
                        case "1":
                            positionResults["beforePositions"] = positions;
                            break;
                        case "2":
                            positionResults["duringPositions"] = positions;
                            break;
                        case "3":
                            positionResults["afterPositions"] = positions;
                            break;
                        default:
                            // Optionally handle unexpected types
                            _logger.LogWarning($"Unexpected type: {type}");
                            break;
                    }
                }

                return new GenericResponse<Dictionary<string, List<Positions>>>(true, "Positions retrieved successfully.", positionResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving positions by type.");
                return new GenericResponse<Dictionary<string, List<Positions>>>(false, "An error occurred while retrieving positions.", null);
            }
        }

    }
}
