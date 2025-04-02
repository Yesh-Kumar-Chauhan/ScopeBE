using AutoMapper;
using Core.DTOs.Inservice;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using DocumentFormat.OpenXml.InkML;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InserviceService : IInserviceService
{
    private readonly IPersonelRepository _personelRepository;
    private readonly IInserviceRepository _inserviceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<InserviceService> _logger;
    private readonly AppDbContext _context;

    public InserviceService(
        AppDbContext context,
        IPersonelRepository personelRepository,
        IInserviceRepository inserviceRepository,
        IMapper mapper,
        ILogger<InserviceService> logger)
    {
        _context = context;
        _personelRepository = personelRepository;
        _inserviceRepository = inserviceRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GenericResponse<IEnumerable<InserviceDto>>> GetInservicesWithFilterAsync(string? search, int page, int pageSize)
    {
        try
        {
            var query = _inserviceRepository.GetAllInservices();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i => i.TRAINING.Contains(search) || i.SPONSOR.Contains(search));
            }

            var totalItems = await query.CountAsync();

            var inservices = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var inserviceDtos = _mapper.Map<IEnumerable<InserviceDto>>(inservices);
            return new GenericResponse<IEnumerable<InserviceDto>>(true, "Inservices retrieved successfully.", inserviceDtos, totalItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving inservices with filter.");
            return new GenericResponse<IEnumerable<InserviceDto>>(false, ex.Message, null);
        }
    }

    public async Task<GenericResponse<List<InserviceDto>>> GetAllInservicesAsync()
    {
        try
        {
            var inservices = await _inserviceRepository.GetAllInservicesAsync();
            var inserviceDtos = _mapper.Map<List<InserviceDto>>(inservices);
            return new GenericResponse<List<InserviceDto>>(true, "All inservices retrieved successfully.", inserviceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all inservices.");
            return new GenericResponse<List<InserviceDto>>(false, ex.Message, null);
        }
    }

    public async Task<GenericResponse<InserviceDto>> GetInserviceByIdAsync(long id)
    {
        try
        {
            var inservice = await _inserviceRepository.GetInserviceByIdAsync(id);
            if (inservice == null)
            {
                _logger.LogWarning("Inservice with ID {InserviceId} not found.", id);
                return new GenericResponse<InserviceDto>(false, "Inservice not found.", null);
            }
            
            var inserviceDto = _mapper.Map<InserviceDto>(inservice);

            // Check if the StaffId is not null and retrieve the related Personnel data
            if (inservice.STAFF_ID.HasValue)
            {
                var personnel = await _context.Personel.FindAsync(inservice.STAFF_ID.Value);
                if (personnel != null)
                {
                    // Assign Personnel fields to the DTO
                    inserviceDto.CPR = personnel.CPR;
                    inserviceDto.SHarassmentExp = personnel.SHarassmentExp;
                    inserviceDto.SHarassmentExp2 = personnel.SHarassmentExp2;
                    inserviceDto.FirstAid = personnel.FIRSTAID;
                    inserviceDto.MatDate = personnel.MATDATE;
                    inserviceDto.MatApp = personnel.MATAPP;
                    inserviceDto.ACES = personnel.ACES;
                    inserviceDto.Elaw = personnel.ELaw;
                    inserviceDto.Foundations = personnel.Foundations;
                    inserviceDto.Foundations15H = personnel.Foundations15H;
                }
            }
            return new GenericResponse<InserviceDto>(true, "Inservice retrieved successfully.", inserviceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving inservice with ID {InserviceId}.", id);
            return new GenericResponse<InserviceDto>(false, ex.Message, null);
        }
    }

    public async Task<GenericResponse<InserviceDto>> CreateInserviceAsync(InserviceDto inserviceDto)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var inservice = _mapper.Map<Inservice>(inserviceDto);
                var createdInservice = await _inserviceRepository.AddInserviceAsync(inservice);

                if (inserviceDto.StaffId.HasValue)
                {
                    var personnel = await _personelRepository.GetPersonelByIdAsync(inserviceDto.StaffId.Value);
                    if (personnel != null)
                    {
                        UpdatePersonnelFields(personnel, inserviceDto);
                        await _personelRepository.UpdatePersonelAsync(personnel);
                    }
                }

                await transaction.CommitAsync();

                var createdInserviceDto = _mapper.Map<InserviceDto>(createdInservice);
                return new GenericResponse<InserviceDto>(true, "Inservice created successfully.", createdInserviceDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while creating a new inservice.");
                return new GenericResponse<InserviceDto>(false, ex.Message, null);
            }
        }
    }

    public async Task<GenericResponse<InserviceDto>> UpdateInserviceAsync(long id, InserviceDto inserviceDto)
    {

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var inservice = _mapper.Map<Inservice>(inserviceDto);
                inservice.InserviceID = id;

                var updatedInservice = await _inserviceRepository.UpdateInserviceAsync(inservice);
                if (updatedInservice == null)
                {
                    _logger.LogWarning("Inservice with ID {InserviceId} not found or update failed.", id);
                    return new GenericResponse<InserviceDto>(false, "Inservice not found or update failed.", null);
                }

                if (inserviceDto.StaffId.HasValue)
                {
                    var personnel = await _personelRepository.GetPersonelByIdAsync(inserviceDto.StaffId.Value);
                    if (personnel != null)
                    {
                        UpdatePersonnelFields(personnel, inserviceDto);
                        await _personelRepository.UpdatePersonelAsync(personnel);
                    }
                }

                await transaction.CommitAsync();

                var updatedInserviceDto = _mapper.Map<InserviceDto>(updatedInservice);
                return new GenericResponse<InserviceDto>(true, "Inservice updated successfully.", updatedInserviceDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating inservice with ID {InserviceId}.", id);
                return new GenericResponse<InserviceDto>(false, ex.Message, null);
            }
        }
    }

    private void UpdatePersonnelFields(Personel personnel, InserviceDto inserviceDto)
    {
        personnel.CPR = inserviceDto.CPR ?? personnel.CPR;
        personnel.SHarassmentExp = inserviceDto.SHarassmentExp ?? personnel.SHarassmentExp;
        personnel.SHarassmentExp2 = inserviceDto.SHarassmentExp2 ?? personnel.SHarassmentExp2;
        personnel.FIRSTAID = inserviceDto.FirstAid ?? personnel.FIRSTAID;
        personnel.MATDATE = inserviceDto.MatDate ?? personnel.MATDATE;
        personnel.MATAPP = inserviceDto.MatApp ?? personnel.MATAPP;
        personnel.ACES = inserviceDto.ACES ?? personnel.ACES;
        personnel.ELaw = inserviceDto.Elaw ?? personnel.ELaw;
        personnel.Foundations = inserviceDto.Foundations ?? personnel.Foundations;
        personnel.Foundations15H = inserviceDto.Foundations15H ?? personnel.Foundations15H;
    }
    public async Task<GenericResponse<bool>> DeleteInserviceAsync(long id)
    {
        try
        {
            var deleted = await _inserviceRepository.DeleteInserviceAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Inservice with ID {InserviceId} not found or deletion failed.", id);
                return new GenericResponse<bool>(false, "Inservice not found or deletion failed.", false);
            }

            return new GenericResponse<bool>(true, "Inservice deleted successfully.", true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting inservice with ID {InserviceId}.", id);
            return new GenericResponse<bool>(false, ex.Message, false);
        }
    }

    public async Task<GenericResponse<List<InserviceDto>>> GetInserviceSelectAsync(long personId, int operation)
    {
        try
        {
            var inservices = await _inserviceRepository.GetInserviceSelectAsync(personId, operation);
            return new GenericResponse<List<InserviceDto>>(true, "Inservices retrieved successfully.", inservices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving inservices for person ID {PersonId}.", personId);
            return new GenericResponse<List<InserviceDto>>(false, ex.Message, null);
        }
    }


    public async Task<GenericResponse<bool>> BulkCreateInservicesAndUpdatePersonnelAsync(InserviceBulkDto formDto)
    {
        try
        {
            // Create lists to hold all the new inservice records and personnel to update
            var inservices = new List<Inservice>();
            var personnelToUpdate = new List<Personel>();

            foreach (var topicId in formDto.TopicIds)
            {
                foreach (var personelId in formDto.PersonnelIds)
                {
                    var inservice = new Inservice
                    {
                        WorkShopTypeID = formDto.WorkShopTypeID,
                        TopicID = topicId,
                        TRAINING = formDto.Training,
                        SPONSOR = formDto.Sponsor,
                        DATE = formDto.Date,
                        STAFF_ID = personelId,
                        HOURS = formDto.Hours,
                        Paid = formDto.Paid,
                        PaidDate = formDto.PaidDate,
                    };

                    inservices.Add(inservice);

                    var personnel = await _personelRepository.GetPersonelByIdAsync(personelId);
                    if (personnel != null)
                    {
                        // Update personnel fields
                        if (formDto.CPR.HasValue) personnel.CPR = formDto.CPR;
                        if (formDto.SHarassmentExp.HasValue) personnel.SHarassmentExp = formDto.SHarassmentExp;
                        if (formDto.SHarassmentExp2.HasValue) personnel.SHarassmentExp2 = formDto.SHarassmentExp2;
                        if (formDto.FirstAid.HasValue) personnel.FIRSTAID = formDto.FirstAid;
                        if (formDto.MatDate.HasValue) personnel.MATDATE = formDto.MatDate;
                        if (formDto.MatApp.HasValue) personnel.MATAPP = formDto.MatApp;
                        if (formDto.ACES.HasValue) personnel.ACES = formDto.ACES;
                        if (formDto.Elaw.HasValue) personnel.ELaw = formDto.Elaw;
                        if (formDto.Foundations.HasValue) personnel.Foundations = formDto.Foundations;
                        if (formDto.Foundations15H.HasValue) personnel.Foundations15H = formDto.Foundations15H;

                        // Add personnel to the list for bulk update
                        if (!personnelToUpdate.Any(p => p.PersonalID == personnel.PersonalID))
                        {
                            personnelToUpdate.Add(personnel);
                        }
                    }
                }
            }

            // Perform bulk insert for inservices
            await _inserviceRepository.BulkInsertInservicesAsync(inservices);

            // Perform bulk update for personnel
            if (personnelToUpdate.Any())
            {
                await _personelRepository.BulkUpdatePersonnelAsync(personnelToUpdate);
            }

            return new GenericResponse<bool>(true, "Inservices created and personnel updated successfully.", true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating inservices and updating personnel.");
            return new GenericResponse<bool>(false, "An error occurred while processing the request.", false);
        }
    }
}
