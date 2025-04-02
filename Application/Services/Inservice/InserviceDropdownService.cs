using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Inservice
{
    public class InserviceDropdownService : IInserviceDropdownService
    {
        private readonly ITopicTypeRepository _topicTypeRepository;
        private readonly IWorkshopTypeRepository _workshopTypeRepository;
        private readonly ILogger<InserviceDropdownService> _logger;

        public InserviceDropdownService(
            ITopicTypeRepository topicTypeRepository,
            IWorkshopTypeRepository workshopTypeRepository,
            ILogger<InserviceDropdownService> logger)
        {
            _topicTypeRepository = topicTypeRepository;
            _workshopTypeRepository = workshopTypeRepository;
            _logger = logger;
        }

        public async Task<GenericResponse<List<TopicType>>> GetTopicTypesAsync()
        {
            try
            {
                var topicTypes = await _topicTypeRepository.GetAllTopicTypesAsync();
                return new GenericResponse<List<TopicType>>(true, "Topic types retrieved successfully.", topicTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving topic types.");
                return new GenericResponse<List<TopicType>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<WorkshopType>>> GetWorkshopTypesAsync()
        {
            try
            {
                var workshopTypes = await _workshopTypeRepository.GetAllWorkshopTypesAsync();
                return new GenericResponse<List<WorkshopType>>(true, "Workshop types retrieved successfully.", workshopTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving workshop types.");
                return new GenericResponse<List<WorkshopType>>(false, ex.Message, null);
            }
        }
    }
}
