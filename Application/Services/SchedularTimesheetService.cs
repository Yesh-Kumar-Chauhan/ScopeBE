using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SchedularTimesheetService : ISchedularTimesheetService
{
    private readonly ISchedularTimesheetRepository _schedularTimesheetRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SchedularTimesheetService> _logger;

    public SchedularTimesheetService(ISchedularTimesheetRepository schedularTimesheetRepository, IMapper mapper, ILogger<SchedularTimesheetService> logger)
    {
        _schedularTimesheetRepository = schedularTimesheetRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GenericResponse<IEnumerable<SchedularTimesheetDto>>> GetSchedularTimesheetsAsync()
    {
        try
        {
            var timesheets = await _schedularTimesheetRepository.GetSchedularTimesheetsAsync();
            var timesheetDtos = _mapper.Map<IEnumerable<SchedularTimesheetDto>>(timesheets);
            return new GenericResponse<IEnumerable<SchedularTimesheetDto>>(true, "Timesheets retrieved successfully.", timesheetDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving timesheets.");
            return new GenericResponse<IEnumerable<SchedularTimesheetDto>>(false, "An error occurred while retrieving timesheets.", null);
        }
    }

    public async Task<GenericResponse<SchedularTimesheetDto>> GetSchedularTimesheetByIdAsync(long id)
    {
        try
        {
            var timesheet = await _schedularTimesheetRepository.GetSchedularTimesheetByIdAsync(id);
            if (timesheet == null)
            {
                _logger.LogWarning("Timesheet with ID {TimesheetId} not found.", id);
                return new GenericResponse<SchedularTimesheetDto>(false, "Timesheet not found.", null);
            }

            var timesheetDto = _mapper.Map<SchedularTimesheetDto>(timesheet);
            return new GenericResponse<SchedularTimesheetDto>(true, "Timesheet retrieved successfully.", timesheetDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the timesheet with ID {TimesheetId}.", id);
            return new GenericResponse<SchedularTimesheetDto>(false, "An error occurred while retrieving the timesheet.", null);
        }
    }

    public async Task<GenericResponse<SchedularTimesheetDto>> AddSchedularTimesheetAsync(SchedularTimesheetDto timesheetDto)
    {
        try
        {
            var timesheet = _mapper.Map<SchedularTimesheet>(timesheetDto);
            var createdTimesheet = await _schedularTimesheetRepository.AddSchedularTimesheetAsync(timesheet);
            var createdTimesheetDto = _mapper.Map<SchedularTimesheetDto>(createdTimesheet);
            return new GenericResponse<SchedularTimesheetDto>(true, "Timesheet added successfully.", createdTimesheetDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a timesheet.");
            return new GenericResponse<SchedularTimesheetDto>(false, "An error occurred while adding the timesheet.", null);
        }
    }

    public async Task<GenericResponse<SchedularTimesheetDto>> UpdateSchedularTimesheetAsync(SchedularTimesheetDto timesheetDto)
    {
        try
        {
            var timesheet = _mapper.Map<SchedularTimesheet>(timesheetDto);
            var updatedTimesheet = await _schedularTimesheetRepository.UpdateSchedularTimesheetAsync(timesheet);
            var updatedTimesheetDto = _mapper.Map<SchedularTimesheetDto>(updatedTimesheet);
            return new GenericResponse<SchedularTimesheetDto>(true, "Timesheet updated successfully.", updatedTimesheetDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the timesheet.");
            return new GenericResponse<SchedularTimesheetDto>(false, "An error occurred while updating the timesheet.", null);
        }
    }

    public async Task<GenericResponse<bool>> DeleteSchedularTimesheetAsync(long id)
    {
        try
        {
            bool deleted = await _schedularTimesheetRepository.DeleteSchedularTimesheetAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Timesheet not found or deletion failed.", id);
                return new GenericResponse<bool>(false, "Timesheet not found or deletion failed.", false);
            }

            return new GenericResponse<bool>(true, "Timesheet deleted successfully.", true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the timesheet with ID {TimesheetId}.", id);
            return new GenericResponse<bool>(false, "An error occurred while deleting the timesheet.", false);
        }
    }

    public async Task<GenericResponse<SchedularTimesheetDto>> GetSchedularTimesheetsByScheduleIdAsync(long scheduleId)
    {
        try
        {
            var timesheets = await _schedularTimesheetRepository.GetSchedularTimesheetsByScheduleIdAsync(scheduleId);
            if (timesheets == null)
            {
                return new GenericResponse<SchedularTimesheetDto>(true, "No timesheets found for the provided schedule ID.", null);
            }

            var timesheetDtos = _mapper.Map<SchedularTimesheetDto>(timesheets);
            return new GenericResponse<SchedularTimesheetDto>(true, "Timesheets retrieved successfully.", timesheetDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving timesheets for schedule ID {ScheduleID}.", scheduleId);
            return new GenericResponse<SchedularTimesheetDto>(false, "An error occurred while retrieving timesheets.", null);
        }
    }

    public async Task<GenericResponse<List<SchedularTimesheetDto>>> GetSchedularTimesheetsByPersonalIdAsync(long personId)
    {
        try
        {
            var timesheets = await _schedularTimesheetRepository.GetSchedularTimesheetsByPersonalIdAsync(personId);
            if (timesheets == null)
            {
                return new GenericResponse<List<SchedularTimesheetDto>> (true, "No timesheets found for the provided personId ID.", null);
            }

            var timesheetDtos = _mapper.Map<List<SchedularTimesheetDto>> (timesheets);
            return new GenericResponse<List<SchedularTimesheetDto>> (true, "Timesheets retrieved successfully.", timesheetDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving timesheets for personId ID {personId}.", personId);
            return new GenericResponse<List<SchedularTimesheetDto>> (false, "An error occurred while retrieving timesheets.", null);
        }
    }

}
