using AutoMapper;
using Core.DTOs.Workshop;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using NetTopologySuite.Operation.Buffer.Validate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WorkshopService : IWorkshopService
    {
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IPersonelRepository _personnelRepository;
        private readonly ITopicTypeRepository _topicTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WorkshopService> _logger;
        private readonly AppDbContext _context;

        public WorkshopService(
            AppDbContext context,
            ITopicTypeRepository topicTypeRepository,
            IWorkshopRepository workshopRepository,
            IPersonelRepository personnelRepository,
            IMapper mapper,
            ILogger<WorkshopService> logger)
        {
            _context = context;
            _topicTypeRepository = topicTypeRepository;
            _workshopRepository = workshopRepository;
            _personnelRepository = personnelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //public async Task<GenericResponse<IEnumerable<WorkshopDto>>> GetWorkshopsWithPaginationAsync(string? search, int page, int pageSize)
        //{
        //    try
        //    {
        //        var query = _workshopRepository.GetAllWorkshops();

        //        if (!string.IsNullOrWhiteSpace(search))
        //        {
        //            query = query.Where(w => w.WorkshopName.Contains(search));
        //        }

        //        var totalItems = await query.CountAsync();
        //        var workshops = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        //        var workshopDtos = _mapper.Map<IEnumerable<WorkshopDto>>(workshops);

        //        return new GenericResponse<IEnumerable<WorkshopDto>>(true, "Workshops retrieved successfully.", workshopDtos, totalItems);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while retrieving workshops with pagination.");
        //        return new GenericResponse<IEnumerable<WorkshopDto>>(false, ex.Message, null);
        //    }
        //}

        //public async Task<GenericResponse<List<WorkshopDto>>> GetAllWorkshopsAsync(string? search)
        //{
        //    try
        //    {
        //        var query = _workshopRepository.GetAllWorkshops();

        //        if (!string.IsNullOrWhiteSpace(search))
        //        {
        //            query = query.Where(w => w.WorkshopName.Contains(search));
        //        }

        //        var workshops = await query.ToListAsync();
        //        var workshopDtos = _mapper.Map<List<WorkshopDto>>(workshops);

        //        return new GenericResponse<List<WorkshopDto>>(true, "All workshops retrieved successfully.", workshopDtos);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while retrieving all workshops.");
        //        return new GenericResponse<List<WorkshopDto>>(false, ex.Message, null);
        //    }
        //}

        public async Task<GenericResponse<List<WorkshopDto>>> GetAllWorkshopsAsync(string? search)
        {
            try
            {
                // Get the raw workshop data from repository
                return await _workshopRepository.GetAllWorkshopsAsync(search);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all workshops.");
                return new GenericResponse<List<WorkshopDto>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<List<WorkshopDto>>> GetWorkshopsWithPaginationAsync(string? search, int page, int pageSize)
        {
            try
            {
                // Get the raw workshop data from repository
                var workshopResponse = await _workshopRepository.GetAllWorkshopsAsync(search);

                // Get the total number of workshops (before pagination)
                var totalItems = workshopResponse.Data.Count;

                // Apply pagination: Skip the first (page-1) * pageSize items and take pageSize items
                var pagedWorkshops = workshopResponse.Data.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Return the paginated workshops and total item count
                return new GenericResponse<List<WorkshopDto>>(true, "Workshops retrieved successfully.", pagedWorkshops, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving workshops with pagination.");
                return new GenericResponse<List<WorkshopDto>>(false, ex.Message, null);
            }
        }



        public async Task<GenericResponse<WorkshopDto>> GetWorkshopByIdAsync(long id)
        {
            try
            {
                var workshop = await _workshopRepository.GetWorkshopByIdAsync(id);
                if (workshop == null)
                {
                    _logger.LogWarning("Workshop with ID {WorkshopId} not found.", id);
                    return new GenericResponse<WorkshopDto>(false, "Workshop not found.", null);
                }

                var workshopDto = _mapper.Map<WorkshopDto>(workshop);
                return new GenericResponse<WorkshopDto>(true, "Workshop retrieved successfully.", workshopDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving workshop with ID {WorkshopId}.", id);
                return new GenericResponse<WorkshopDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<WorkshopFormDto>> CreateWorkshopAsync(WorkshopFormDto workshopDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var workshops = new List<Workshop>();
                    var workshopTopics = new List<WorkshopTopic>();
                    var workshopMembers = new List<WorkshopMember>();

                    var workshop = new Workshop
                    {
                        Hours = workshopDto.Hours,
                        TypeID = workshopDto.TypeID,
                        Date = workshopDto.Date,
                        Sponsor = workshopDto.Sponsor,
                        Paid = workshopDto.Paid,
                        PaidDate = workshopDto.PaidDate,
                        WorkshopName = workshopDto.WorkshopName
                    };
                    workshops.Add(workshop);

                    foreach (var topicId in workshopDto.TopicIds)
                    {
                        var topic = await _topicTypeRepository.GetTopicByIdAsync(topicId);
                        if (topic == null)
                        {
                            throw new Exception($"Topic with ID {topicId} not found.");
                        }

                        var workshopTopic = new WorkshopTopic
                        {
                            Workshop = workshop, // workshop reference
                            TopicID = topicId,
                        };

                        workshopTopics.Add(workshopTopic);
                    }

                    foreach (var personId in workshopDto.PersonIds)
                    {
                        var personnel = await _personnelRepository.GetPersonelByIdAsync(personId);
                        if (personnel == null)
                        {
                            throw new Exception($"Personnel with ID {personId} not found.");
                        }

                        var workshopMember = new WorkshopMember
                        {
                            Workshop = workshop, // workshop reference
                            PersonID = personId,
                        };

                        workshopMembers.Add(workshopMember);

                    }

                    // Perform bulk insert for workshops, workshop topics, and workshop members
                    await _workshopRepository.BulkInsertWorkshopsAsync(workshops);
                    await _workshopRepository.BulkInsertWorkshopTopicsAsync(workshopTopics);
                    await _workshopRepository.BulkInsertWorkshopMembersAsync(workshopMembers);

                    await transaction.CommitAsync();
                    return new GenericResponse<WorkshopFormDto>(true, "Workshops created successfully.", workshopDto);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "An error occurred while creating workshops.");
                    return new GenericResponse<WorkshopFormDto>(false, "An error occurred while processing the request.", null);
                }
            }
        }

        public async Task<GenericResponse<WorkshopDto>> UpdateWorkshopAsync(long id, WorkshopFormDto workshopDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingWorkshop = await _workshopRepository.GetWorkshopByIdAsync(id);
                    if (existingWorkshop == null)
                    {
                        _logger.LogWarning("Workshop with ID {WorkshopId} not found.", id);
                        return new GenericResponse<WorkshopDto>(false, "Workshop not found.", null);
                    }

                    // Update workshop details
                    existingWorkshop.TypeID = workshopDto.TypeID;
                    existingWorkshop.Date = workshopDto.Date;
                    existingWorkshop.Sponsor = workshopDto.Sponsor;
                    existingWorkshop.Paid = workshopDto.Paid;
                    existingWorkshop.PaidDate = workshopDto.PaidDate;
                    existingWorkshop.WorkshopName = workshopDto.WorkshopName;
                    existingWorkshop.Hours = workshopDto.Hours;

                    // Identify and remove WorkshopTopics not in updated list using the repository
                    var topicIdsToRemove = existingWorkshop.WorkshopTopics
                        .Where(wt => !workshopDto.TopicIds.Contains((long)wt.TopicID))
                        .Select(wt => wt.WorkshopTopicID)
                        .ToList();
                    await _workshopRepository.RemoveWorkshopTopicsAsync(topicIdsToRemove);

                    // Identify and add new WorkshopTopics from the updated list
                    var topicIdsToAdd = workshopDto.TopicIds
                        .Where(topicId => !existingWorkshop.WorkshopTopics.Any(wt => wt.TopicID == topicId))
                        .ToList();
                    foreach (var topicId in topicIdsToAdd)
                    {
                        var topic = await _topicTypeRepository.GetTopicByIdAsync(topicId);
                        if (topic == null)
                        {
                            throw new Exception($"Topic with ID {topicId} not found.");
                        }

                        var workshopTopic = new WorkshopTopic
                        {
                            WorkshopID = id,
                            TopicID = topicId,
                        };

                        existingWorkshop.WorkshopTopics.Add(workshopTopic);
                    }

                    // Identify and remove WorkshopMembers not in updated list using the repository
                    var personIdsToRemove = existingWorkshop.WorkshopMembers
                        .Where(wm => !workshopDto.PersonIds.Contains((long)wm.PersonID))
                        .Select(wm => wm.WorkshopMemberID)
                        .ToList();
                    await _workshopRepository.RemoveWorkshopMembersAsync(personIdsToRemove);

                    // Identify and add new WorkshopMembers from the updated list
                    var personIdsToAdd = workshopDto.PersonIds
                        .Where(personId => !existingWorkshop.WorkshopMembers.Any(wm => wm.PersonID == personId))
                        .ToList();
                    foreach (var personId in personIdsToAdd)
                    {
                        var personnel = await _personnelRepository.GetPersonelByIdAsync(personId);
                        if (personnel == null)
                        {
                            throw new Exception($"Personnel with ID {personId} not found.");
                        }

                        var workshopMember = new WorkshopMember
                        {
                            WorkshopID = id,
                            PersonID = personId,
                        };

                        existingWorkshop.WorkshopMembers.Add(workshopMember);
                    }

                    // Save all changes to the database
                    await _workshopRepository.UpdateWorkshopAsync(existingWorkshop);

                    await transaction.CommitAsync();

                    var updatedWorkshopDto = _mapper.Map<WorkshopDto>(existingWorkshop);
                    return new GenericResponse<WorkshopDto>(true, "Workshop updated successfully.", updatedWorkshopDto);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "An error occurred while updating workshop with ID {WorkshopId}.", id);
                    return new GenericResponse<WorkshopDto>(false, "An error occurred while processing the request.", null);
                }
            }
        }


        public async Task<GenericResponse<bool>> DeleteWorkshopAsync(long id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingWorkshop = await _workshopRepository.GetWorkshopByIdAsync(id);
                    if (existingWorkshop == null)
                    {
                        _logger.LogWarning("Workshop with ID {WorkshopId} not found.", id);
                        return new GenericResponse<bool>(false, "Workshop not found.", false);
                    }

                    // Remove related WorkshopTopics
                    if (existingWorkshop.WorkshopTopics.Any())
                    {
                        await _workshopRepository.RemoveWorkshopTopicsAsync(existingWorkshop.WorkshopTopics.Select(wt => wt.WorkshopTopicID));
                    }

                    // Remove related WorkshopMembers
                    if (existingWorkshop.WorkshopMembers.Any())
                    {
                        await _workshopRepository.RemoveWorkshopMembersAsync(existingWorkshop.WorkshopMembers.Select(wm => wm.WorkshopMemberID));
                    }

                    // Remove the Workshop
                    var deleted = await _workshopRepository.DeleteWorkshopAsync(id);
                    if (!deleted)
                    {
                        _logger.LogWarning("Workshop with ID {WorkshopId} deletion failed.", id);
                        await transaction.RollbackAsync();
                        return new GenericResponse<bool>(false, "Workshop deletion failed.", false);
                    }

                    await transaction.CommitAsync();
                    return new GenericResponse<bool>(true, "Workshop deleted successfully.", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "An error occurred while deleting workshop with ID {WorkshopId}.", id);
                    return new GenericResponse<bool>(false, ex.Message, false);
                }
            }
        }


    }

}
