using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using DocumentFormat.OpenXml.Wordprocessing;
using Application.Services;
using RestSharp;
using Newtonsoft.Json.Linq;
using Infrastructure.Repositories;
using Azure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using Core.Entities.Brainyclock;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IDistrictRepository _districtRepository;
    private readonly ISiteRepository _siteRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ScheduleService> _logger;
    private readonly IConfiguration _configuration;
    private readonly RestClientService _restClientService;
    private readonly AppDbContext _context;
    private readonly MySqlDbContext _mySqlContext;


    public ScheduleService(IScheduleRepository scheduleRepository, IMapper mapper,
        ILogger<ScheduleService> logger, IConfiguration configuration,
        RestClientService restClientService,
        ISiteRepository siteRepository,
        IDistrictRepository districtRepository,
        AppDbContext context,
        MySqlDbContext mySqlContext
        )
    {
        _scheduleRepository = scheduleRepository;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
        _restClientService = restClientService;
        _siteRepository = siteRepository;
        _districtRepository = districtRepository;
        _context = context;
        _mySqlContext = mySqlContext;
    }

    public async Task<GenericResponse<IEnumerable<ScheduleDto>>> GetSchedulesAsync()
    {
        try
        {
            var schedules = await _scheduleRepository.GetSchedulesAsync();
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
            return new GenericResponse<IEnumerable<ScheduleDto>>(true, "Schedules retrieved successfully.", scheduleDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving schedules.");
            return new GenericResponse<IEnumerable<ScheduleDto>>(false, "An error occurred while retrieving schedules.", null);
        }
    }

    public async Task<GenericResponse<ScheduleDto>> GetScheduleByIdAsync(long id)
    {
        try
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                _logger.LogWarning("Schedule with ID {ScheduleId} not found.", id);
                return new GenericResponse<ScheduleDto>(false, "Schedule not found.", null);
            }

            var scheduleDto = _mapper.Map<ScheduleDto>(schedule);
            return new GenericResponse<ScheduleDto>(true, "Schedule retrieved successfully.", scheduleDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the schedule with ID {ScheduleId}.", id);
            return new GenericResponse<ScheduleDto>(false, "An error occurred while retrieving the schedule.", null);
        }
    }

    //public async Task<GenericResponse<ScheduleDto>> AddScheduleAsync(ScheduleDto scheduleDto)
    //{
    //    try
    //    {
    //        var schedule = _mapper.Map<Schedule>(scheduleDto);
    //        var existedSchedule = await _scheduleRepository.GetScheduleByPersonIdAsync((long)schedule.PersonID);
    //        if (existedSchedule.Count > 0)
    //        {
    //            var alreadyExists = existedSchedule.Find(x => x.SiteType.ToLower() == schedule.SiteType.ToLower() && x.Date != null);
    //            if (alreadyExists != null)
    //            {
    //                return new GenericResponse<ScheduleDto>(false, "Schedule is already exists for this sitetype.", null);
    //            }
    //        }

    //        var site = await _siteRepository.GetSiteByIdAsync((long)schedule.SiteID);
    //        if (site == null)
    //        {
    //            return new GenericResponse<ScheduleDto>(false, "Site not found.", null);
    //        }
    //        schedule.SiteName = site.SITE_NAM;
    //        // Extract the first three digits of SITE_NUM and assign to DistNumber
    //        if (site.SITE_NUM != null)
    //        {
    //            var siteNumString = site.SITE_NUM.ToString();
    //            schedule.DistNumber = int.Parse(siteNumString.Substring(0, Math.Min(3, siteNumString.Length)));
    //        }
    //        if (schedule.DistNumber != null)
    //        {

    //            var district = await _districtRepository.GetAllDistrictsAsync();
    //            schedule.DistName = district.Find(x => x.DIST_NUM == schedule.DistNumber).DIST_NAM;
    //        }

    //        var createdSchedule = await _scheduleRepository.AddScheduleAsync(schedule);
    //        var createdScheduleDto = _mapper.Map<ScheduleDto>(createdSchedule);
    //        // Prepare the RestSharp request for the external API call
    //        var request = new RestRequest("Schedules", Method.Post);
    //        request.AddJsonBody(createdScheduleDto);

    //        // Use RestClientService to make the API call
    //        var response = await _restClientService.PostAsync<ScheduleDto>("Schedules", createdScheduleDto);

    //        if (response.IsSuccessful)
    //        {
    //            return new GenericResponse<ScheduleDto>(true, "Schedule added successfully.", response.Data);
    //        }
    //        else
    //        {
    //            // Extract the error message from the response content
    //            string errorMessage = "Failed to add schedule on external API.";
    //            if (!string.IsNullOrEmpty(response.Content))
    //            {
    //                try
    //                {
    //                    // Parse the response content to extract the error message if it exists
    //                    var contentJson = JObject.Parse(response.Content);
    //                    errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
    //                }
    //                catch (Exception parseEx)
    //                {
    //                    _logger.LogError(parseEx, "Failed to parse error message from API response.");
    //                }
    //            }

    //            _logger.LogError("Failed to add schedule on external API: {0}", errorMessage);
    //            return new GenericResponse<ScheduleDto>(false, errorMessage, null);
    //        }
    //        //return new GenericResponse<ScheduleDto>(true, "Schedule added successfully.", createdScheduleDto);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while adding a schedule.");
    //        return new GenericResponse<ScheduleDto>(false, "An error occurred while adding the schedule.", null);
    //    }
    //}

    public async Task<GenericResponse<bool>> ClearSchedule(long personnelId)
    {
        using var transaction = await _scheduleRepository.BeginTransactionAsync();

        try
        {
            var response = await _restClientService.DeleteAsync($"ClearSchedules/{personnelId}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await _scheduleRepository.DeleteScheduleByEmployeeAsync(personnelId);
                await transaction.CommitAsync(); // Commit transaction if API call is successful
                return new GenericResponse<bool>(true, "Schedules deleted successfully.", true);
            }
            else
            {
                // Extract the error message from the response content
                string errorMessage = "Failed to delete schedules on external API.";
                if (!string.IsNullOrEmpty(response.Content))
                {
                    try
                    {
                        var contentJson = JObject.Parse(response.Content);
                        errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogError(parseEx, "Failed to parse error message from API response.");
                    }
                }

                _logger.LogError("Failed to delete schedules on external API: {0}", errorMessage);
                await transaction.RollbackAsync(); // Rollback transaction if API call fails
                return new GenericResponse<bool>(false, errorMessage, false);
            }

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(); // Rollback on exception
            _logger.LogError(ex, "An error occurred while deleting a schedule.");
            return new GenericResponse<bool>(false, "An error occurred while deleting the schedule.", false);
        }
    }


    public async Task<GenericResponse<ScheduleDto>> AddScheduleAsync(ScheduleDto scheduleDto)
    {
        using var transaction = await _scheduleRepository.BeginTransactionAsync();

        try
        {
            var schedule = _mapper.Map<Schedule>(scheduleDto);

            // Retrieve and validate site
            var site = await _siteRepository.GetSiteByIdAsync((long)schedule.SiteID);
            if (site == null)
            {
                return new GenericResponse<ScheduleDto>(false, "Site not found.", null);
            }

            //var existingSchedules = await _scheduleRepository.GetSchedulesAsync();
            //if (existingSchedules.Any(x => x.SiteID == schedule.SiteID && x.Position == "Site Director" && x.PersonID != schedule.PersonID) && schedule.Position == "Site Director")
            //{
            //    return new GenericResponse<ScheduleDto>(false, $"A Site Director is already assigned to SiteID {schedule.SiteID}.", null);
            //}
            schedule.SiteName = site.SITE_NAM;

            // Set DistNumber based on SITE_NUM
            if (site.SITE_NUM != null)
            {
                var siteNumString = site.SITE_NUM.ToString();
                schedule.DistNumber = int.Parse(siteNumString.Substring(0, Math.Min(3, siteNumString.Length)));
            }

            if (schedule.DistNumber != null)
            {
                var district = await _districtRepository.GetAllDistrictsAsync();
                schedule.DistName = district.Find(x => x.DIST_NUM == schedule.DistNumber)?.DIST_NAM;
            }

            if (schedule.Site != null)
            {
                schedule.Site = null; // Set the property to null to "remove" its value
            }
            if (schedule.Personel != null)
            {
                schedule.Personel = null; // Set the property to null to "remove" its value
            }


            // List to store created schedules
            var createdSchedules = new List<Schedule>();

            if (schedule.Date.HasValue || schedule.UpdatedDate.HasValue || schedule.StartDate.HasValue || schedule.EndDate.HasValue)
            {
                createdSchedules.Add(await _scheduleRepository.AddScheduleAsync(schedule));
            }

            // Create a duplicate of the schedule with DeletedDate and DeletedSiteType as null if DeletedDate is not null
            if (schedule.DeletedDate.HasValue && (schedule.Date.HasValue || schedule.UpdatedDate.HasValue))
            {
                var duplicateSchedule = new Schedule
                {
                    Date = null,
                    DeletedDate = null, 
                    DeletedSiteType = null,
                    UpdatedDate = schedule.UpdatedDate,
                    DistNumber = schedule.DistNumber,
                    DistName = schedule.DistName,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate,
                    PersonID = schedule.PersonID,
                    SiteID = schedule.SiteID,
                    Position = schedule.Position,
                    SiteType = schedule.SiteType,
                    SiteName = schedule.SiteName,

                    MondayTimeIn = schedule.MondayTimeIn,
                    MondayTimeOut = schedule.MondayTimeOut,
                    TuesdayTimeIn = schedule.TuesdayTimeIn,
                    TuesdayTimeOut = schedule.TuesdayTimeOut,
                    WednesdayTimeIn = schedule.WednesdayTimeIn,
                    WednesdayTimeOut = schedule.WednesdayTimeOut,
                    ThursdayTimeIn = schedule.ThursdayTimeIn,
                    ThursdayTimeOut = schedule.ThursdayTimeOut,
                    FridayTimeIn = schedule.FridayTimeIn,
                    FridayTimeOut = schedule.FridayTimeOut,

                    Notes = schedule.Notes,
                    Paycode = schedule.Paycode,
                };


                createdSchedules.Add(await _scheduleRepository.AddScheduleAsync(duplicateSchedule));
            }
            else if (schedule.DeletedDate.HasValue && !schedule.Date.HasValue && !schedule.UpdatedDate.HasValue)
            {

                var duplicateSchedule = new Schedule
                {
                    Date = null,
                    DeletedDate = schedule.DeletedDate,
                    DeletedSiteType = schedule.DeletedSiteType,
                    UpdatedDate = schedule.UpdatedDate,
                    DistNumber = schedule.DistNumber,
                    DistName = schedule.DistName,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate,
                    PersonID = schedule.PersonID,
                    SiteID = schedule.SiteID,
                    Position = schedule.Position,
                    SiteType = schedule.SiteType,
                    SiteName = schedule.SiteName,

                    MondayTimeIn = schedule.MondayTimeIn,
                    MondayTimeOut = schedule.MondayTimeOut,
                    TuesdayTimeIn = schedule.TuesdayTimeIn,
                    TuesdayTimeOut = schedule.TuesdayTimeOut,
                    WednesdayTimeIn = schedule.WednesdayTimeIn,
                    WednesdayTimeOut = schedule.WednesdayTimeOut,
                    ThursdayTimeIn = schedule.ThursdayTimeIn,
                    ThursdayTimeOut = schedule.ThursdayTimeOut,
                    FridayTimeIn = schedule.FridayTimeIn,
                    FridayTimeOut = schedule.FridayTimeOut,

                    Notes = schedule.Notes,
                    Paycode = schedule.Paycode,
                };


                createdSchedules.Add(await _scheduleRepository.AddScheduleAsync(duplicateSchedule));
            }

            var createdScheduleDto = _mapper.Map<List<ScheduleDto>>(createdSchedules);

            //await transaction.CommitAsync(); // Commit transaction if API call is successful
            //return new GenericResponse<ScheduleDto>(true, "Schedule added successfully.", createdScheduleDto.First());
            // Prepare and send the API request
            //var response = await _restClientService.PostAsync<List<ScheduleDto>>("Schedules", createdScheduleDto);

            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    await transaction.CommitAsync(); // Commit transaction if API call is successful
            //    return new GenericResponse<ScheduleDto>(true, "Schedule added successfully.", createdScheduleDto.First());
            //}
            //else
            //{
            //    // Extract error message from the response content
            //    string errorMessage = "Failed to add schedule on external API.";
            //    if (!string.IsNullOrEmpty(response.Content))
            //    {
            //        try
            //        {
            //            var contentJson = JObject.Parse(response.Content);
            //            errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
            //        }
            //        catch (Exception parseEx)
            //        {
            //            _logger.LogError(parseEx, "Failed to parse error message from API response.");
            //        }
            //    }

            //    _logger.LogError("Failed to add schedule on external API: {0}", errorMessage);
            //    await transaction.RollbackAsync(); // Rollback transaction if API call fails
            //    return new GenericResponse<ScheduleDto>(false, errorMessage, null);
            //}

            foreach (var createdSchedule in createdScheduleDto)
            {
                var scheduleRequest = new List<ScheduleDto>();
                scheduleRequest.Add(createdSchedule);
                var response = await _restClientService.PostAsync<ScheduleDto>("Schedules", scheduleRequest);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    // Extract error message from the response content
                    string errorMessage = "Failed to add schedule on external API.";
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        try
                        {
                            var contentJson = JObject.Parse(response.Content);
                            errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
                        }
                        catch (Exception parseEx)
                        {
                            _logger.LogError(parseEx, "Failed to parse error message from API response.");
                        }
                    }

                    _logger.LogError("Failed to add schedule on external API: {0}", errorMessage);
                    await transaction.RollbackAsync(); // Rollback transaction if any API call fails
                    return new GenericResponse<ScheduleDto>(false, errorMessage, null);
                }
            }

            await transaction.CommitAsync(); // Commit transaction if all API calls are successful
            return new GenericResponse<ScheduleDto>(true, "Schedule added successfully.", createdScheduleDto.First());

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(); // Rollback on exception
            _logger.LogError(ex, "An error occurred while adding a schedule.");
            return new GenericResponse<ScheduleDto>(false, "An error occurred while adding the schedule.", null);
        }
    }

    public async Task<GenericResponse<ScheduleDto>> UpdateScheduleAsync(ScheduleDto scheduleDto)
    {
        using var transaction = await _scheduleRepository.BeginTransactionAsync();

        try
        {
            var schedule = _mapper.Map<Schedule>(scheduleDto);
            schedule.Date = null;
            schedule.DeletedSiteType = null;
            schedule.DeletedDate = null;
            //if (schedule.Date.HasValue && schedule.UpdatedDate.HasValue && schedule.StartDate.HasValue && schedule.EndDate.HasValue)
            //{
            //    //skip
            //    schedule.Date = null;
            //    schedule.DeletedSiteType = null;
            //    schedule.DeletedDate = null;
            //}
            //else if (schedule.Date.HasValue || schedule.UpdatedDate.HasValue)
            //{
            //    schedule.DeletedSiteType = null;
            //    schedule.DeletedDate = null;
            //    //schedule.Date = null;
            //}

            var site = await _siteRepository.GetSiteByIdAsync((long)schedule.SiteID);
            if (site == null)
            {
                return new GenericResponse<ScheduleDto>(false, "Site not found.", null);
            }
            //var existingSchedules = await _scheduleRepository.GetSchedulesAsync();
            //if (existingSchedules.Any(x => x.SiteID == schedule.SiteID && x.Position == "Site Director" && x.PersonID != schedule.PersonID) && schedule.Position == "Site Director")
            //{
            //    return new GenericResponse<ScheduleDto>(false, $"A Site Director is already assigned to SiteID {schedule.SiteID}.", null);
            //}

            schedule.SiteName = site.SITE_NAM;

            // Set DistNumber based on SITE_NUM
            if (site.SITE_NUM != null)
            {
                var siteNumString = site.SITE_NUM.ToString();
                schedule.DistNumber = int.Parse(siteNumString.Substring(0, Math.Min(3, siteNumString.Length)));
            }

            if (schedule.DistNumber != null)
            {
                var district = await _districtRepository.GetAllDistrictsAsync();
                schedule.DistName = district.Find(x => x.DIST_NUM == schedule.DistNumber)?.DIST_NAM;
            }

            //schedule.Date = null;
            // Update the schedule in the repository
            if (schedule.Site != null)
            {
                schedule.Site = null; // Set the property to null to "remove" its value
            }

            var updatedSchedule = await _scheduleRepository.UpdateScheduleAsync(schedule);
            var updatedScheduleDto = _mapper.Map<ScheduleDto>(updatedSchedule);

            //await transaction.CommitAsync(); // Commit transaction if API call is successful
            //return new GenericResponse<ScheduleDto>(true, "Schedule updated successfully.", updatedScheduleDto);
            // Use RestClientService to make the API call

            if (updatedScheduleDto.Site != null)
            {
                updatedScheduleDto.Site = null; // Set the property to null to "remove" its value
            }

            if (updatedScheduleDto.Personel != null)
            {
                updatedScheduleDto.Personel = null; // Set the property to null to "remove" its value
            }

            var response = await _restClientService.PutAsync<ScheduleDto>($"Schedules/{scheduleDto.Id}", updatedScheduleDto);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await transaction.CommitAsync(); // Commit transaction if API call is successful
                return new GenericResponse<ScheduleDto>(true, "Schedule updated successfully.", updatedScheduleDto);
            }
            else
            {
                // Extract the error message from the response content
                string errorMessage = "Failed to update schedule on external API.";
                if (!string.IsNullOrEmpty(response.Content))
                {
                    try
                    {
                        var contentJson = JObject.Parse(response.Content);
                        errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogError(parseEx, "Failed to parse error message from API response.");
                    }
                }

                _logger.LogError("Failed to update schedule on external API: {0}", errorMessage);
                await transaction.RollbackAsync(); // Rollback transaction if API call fails
                return new GenericResponse<ScheduleDto>(false, errorMessage, null);
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(); // Rollback on exception
            _logger.LogError(ex, "An error occurred while updating the schedule.");
            return new GenericResponse<ScheduleDto>(false, "An error occurred while updating the schedule.", null);
        }
    }


    public async Task<GenericResponse<List<ScheduleDto>>> AddAdditionalSchedule(ScheduleDto scheduleDto)
    {
        using var transaction = await _scheduleRepository.BeginTransactionAsync();

        try
        {
            var schedule = _mapper.Map<Schedule>(scheduleDto);

            // Retrieve and validate site
            var site = await _siteRepository.GetSiteByIdAsync((long)schedule.SiteID);
            if (site == null)
            {
                return new GenericResponse<List<ScheduleDto>>(false, "Site not found.", null);
            }

            schedule.SiteName = site.SITE_NAM;

            // Set DistNumber based on SITE_NUM
            if (site.SITE_NUM != null)
            {
                var siteNumString = site.SITE_NUM.ToString();
                schedule.DistNumber = int.Parse(siteNumString.Substring(0, Math.Min(3, siteNumString.Length)));
            }

            if (schedule.DistNumber != null)
            {
                var district = await _districtRepository.GetAllDistrictsAsync();
                schedule.DistName = district.Find(x => x.DIST_NUM == schedule.DistNumber)?.DIST_NAM;
            }

            if (schedule.Site != null) schedule.Site = null;
            if (schedule.Personel != null) schedule.Personel = null;

            // Generate dates based on schedule type
            var dates = new List<DateTime>();
            if (scheduleDto.SelectedScheduleType == "1day")
            {
                if (scheduleDto.Date.HasValue)
                {
                    dates.Add(scheduleDto.Date.Value);
                }
                else
                {
                    return new GenericResponse<List<ScheduleDto>>(false, "Date is required for '1day' schedule type.", null);
                }
            }
            else if (scheduleDto.SelectedScheduleType == "1week")
            {
                dates = Enumerable.Range(0, 7)
                    .Select(offset => DateTime.Today.AddDays(offset))
                    .Where(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    .ToList();
            }
            else if (scheduleDto.SelectedScheduleType == "1month")
            {
                dates = Enumerable.Range(0, 30)
                    .Select(offset => DateTime.Today.AddDays(offset))
                    .Where(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    .ToList();
            }

            Guid sessionId = new Guid(Guid.NewGuid().ToString());

            // Prepare schedules for batch insert
            var schedulesToInsert = dates
                .Where(date =>
                // Check if any time for the specific day is defined
                (date.DayOfWeek == DayOfWeek.Monday && (schedule.MondayTimeIn.HasValue || schedule.MondayTimeOut.HasValue)) ||
                (date.DayOfWeek == DayOfWeek.Tuesday && (schedule.TuesdayTimeIn.HasValue || schedule.TuesdayTimeOut.HasValue)) ||
                (date.DayOfWeek == DayOfWeek.Wednesday && (schedule.WednesdayTimeIn.HasValue || schedule.WednesdayTimeOut.HasValue)) ||
                (date.DayOfWeek == DayOfWeek.Thursday && (schedule.ThursdayTimeIn.HasValue || schedule.ThursdayTimeOut.HasValue)) ||
                (date.DayOfWeek == DayOfWeek.Friday && (schedule.FridayTimeIn.HasValue || schedule.FridayTimeOut.HasValue))
            ).Select(date => new Schedule
            {
                Date = date,
                DistNumber = schedule.DistNumber,
                DistName = schedule.DistName,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate,
                PersonID = schedule.PersonID,
                SiteID = schedule.SiteID,
                Position = schedule.Position,
                SiteType = schedule.SiteType,
                SiteName = schedule.SiteName,

                MondayTimeIn = schedule.MondayTimeIn,
                MondayTimeOut = schedule.MondayTimeOut,
                TuesdayTimeIn = schedule.TuesdayTimeIn,
                TuesdayTimeOut = schedule.TuesdayTimeOut,
                WednesdayTimeIn = schedule.WednesdayTimeIn,
                WednesdayTimeOut = schedule.WednesdayTimeOut,
                ThursdayTimeIn = schedule.ThursdayTimeIn,
                ThursdayTimeOut = schedule.ThursdayTimeOut,
                FridayTimeIn = schedule.FridayTimeIn,
                FridayTimeOut = schedule.FridayTimeOut,

                Notes = schedule.Notes,
                Paycode = schedule.Paycode,
                //SessionId = sessionId
            }).ToList();

            // Batch insert and retrieve inserted records
            var insertedSchedules = await _scheduleRepository.AddSchedulesAsync(schedulesToInsert);

            // Map inserted schedules to DTOs
            var createdScheduleDtos = _mapper.Map<List<ScheduleDto>>(insertedSchedules);

            // Set the Site property to null for all schedules
            createdScheduleDtos.ForEach(schedule => schedule.Site = null);

            //here i need to insert the EmployeeSchedule
            // Prepare MySQL insertions for employeeSchedules
            var employeeSchedulesToInsert = insertedSchedules.Select(scheduleItem => new EmployeeSchedules
            {
                EmployeeId = (int)scheduleItem.PersonID,
                Position = scheduleItem.Position,
                Notes = scheduleItem.Notes,
                Date = scheduleItem.Date,
                SiteId = (int)scheduleItem.SiteID,
                SiteType = scheduleItem.SiteType,
                EndDate = scheduleItem.EndDate,
                StartDate = scheduleItem.StartDate,
                EventId = (int)scheduleItem.Id, // Use Schedule's Id as EventId
                SiteName = scheduleItem.SiteName,
                DistNumber = scheduleItem.DistNumber?.ToString(),
                DistName = scheduleItem.DistName,
                MondayTimeIn = scheduleItem.MondayTimeIn,
                MondayTimeOut = scheduleItem.MondayTimeOut,
                TuesdayTimeIn = scheduleItem.TuesdayTimeIn,
                TuesdayTimeOut = scheduleItem.TuesdayTimeOut,
                WednesdayTimeIn = scheduleItem.WednesdayTimeIn,
                WednesdayTimeOut = scheduleItem.WednesdayTimeOut,
                ThursdayTimeIn = scheduleItem.ThursdayTimeIn,
                ThursdayTimeOut = scheduleItem.ThursdayTimeOut,
                FridayTimeIn = scheduleItem.FridayTimeIn,
                FridayTimeOut = scheduleItem.FridayTimeOut,
                Paycode = scheduleItem.Paycode,
                UpdatedDate = scheduleItem.UpdatedDate,
                DeletedDate = scheduleItem.DeletedDate,
                DeletedSiteType = scheduleItem.DeletedSiteType
            }).ToList();

            // Insert into MySQL employeeSchedules
            await _mySqlContext.EmployeeSchedules.AddRangeAsync(employeeSchedulesToInsert);
            await _mySqlContext.SaveChangesAsync();


            // Get the list of inserted IDs
            //var insertedIds = insertedSchedules.Select(s => s.Id).ToList();
            // Call external API with the inserted records

            //var response = await _restClientService.PostAsync<ScheduleDto>("Schedules", createdScheduleDtos);
            // var response = await _restClientService.PostAsync<ScheduleDto>("sync-records", new { sessionId = sessionId});

            //if (response.StatusCode != System.Net.HttpStatusCode.OK)
            //{
            //    //need to delete the schedule from here for sessionId
            //    var isDeleted = await _scheduleRepository.DeleteScheduleBySessionIdAsync(sessionId);

            //    string errorMessage = "Failed to send schedules to the external API.";
            //    if (!string.IsNullOrEmpty(response.Content))
            //    {
            //        try
            //        {
            //            var contentJson = JObject.Parse(response.Content);
            //            errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
            //        }
            //        catch (Exception parseEx)
            //        {
            //            _logger.LogError(parseEx, "Failed to parse error message from API response.");
            //        }
            //    }

            //    _logger.LogError("External API call failed: {0}", errorMessage);
            //    //await transaction.RollbackAsync(); // Rollback if external API call fails
            //    return new GenericResponse<List<ScheduleDto>>(false, errorMessage, null);
            //}

            await transaction.CommitAsync(); // Commit transaction if all operations succeed
            return new GenericResponse<List<ScheduleDto>>(true, "Schedules added successfully.", createdScheduleDtos);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(); // Rollback on exception
            _logger.LogError(ex, "An error occurred while adding schedules.");
            return new GenericResponse<List<ScheduleDto>>(false, "An error occurred while adding schedules.", null);
        }
    }


    //public async Task<GenericResponse<List<ScheduleDto>>> AddAdditionalSchedule(ScheduleDto scheduleDto)
    //{
    //    using var transaction = await _scheduleRepository.BeginTransactionAsync();

    //    try
    //    {
    //        var schedule = _mapper.Map<Schedule>(scheduleDto);

    //        // Retrieve and validate site
    //        var site = await _siteRepository.GetSiteByIdAsync((long)schedule.SiteID);
    //        if (site == null)
    //        {
    //            return new GenericResponse<List<ScheduleDto>>(false, "Site not found.", null);
    //        }

    //        schedule.SiteName = site.SITE_NAM;

    //        // Set DistNumber based on SITE_NUM
    //        if (site.SITE_NUM != null)
    //        {
    //            var siteNumString = site.SITE_NUM.ToString();
    //            schedule.DistNumber = int.Parse(siteNumString.Substring(0, Math.Min(3, siteNumString.Length)));
    //        }

    //        if (schedule.DistNumber != null)
    //        {
    //            var district = await _districtRepository.GetAllDistrictsAsync();
    //            schedule.DistName = district.Find(x => x.DIST_NUM == schedule.DistNumber)?.DIST_NAM;
    //        }

    //        if (schedule.Site != null)
    //        {
    //            schedule.Site = null; // Set the property to null to "remove" its value
    //        }
    //        if (schedule.Personel != null)
    //        {
    //            schedule.Personel = null; // Set the property to null to "remove" its value
    //        }


    //        // List to store created schedules
    //        var allSchedule = new List<Schedule>();

    //        var dates = new List<DateTime>();

    //        if (scheduleDto.SelectedScheduleType == "1day")
    //        {
    //            // Use the date passed from the frontend
    //            if (scheduleDto.Date.HasValue)
    //            {
    //                dates.Add(scheduleDto.Date.Value); // Add the provided date
    //            }
    //            else
    //            {
    //                return new GenericResponse<List<ScheduleDto>>(false, "Date is required for '1day' schedule type.", null);
    //            }
    //        }
    //        else if (scheduleDto.SelectedScheduleType == "1week")
    //        {
    //            dates = Enumerable.Range(0, 7) // 7 days including today
    //                    .Select(offset => DateTime.Today.AddDays(offset))
    //                    .Where(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday) // Skip weekends
    //                    .ToList();
    //        }
    //        else if (scheduleDto.SelectedScheduleType == "1month")
    //        {
    //            dates = Enumerable.Range(1, 30) // 30 days from tomorrow
    //                    .Select(offset => DateTime.Today.AddDays(offset))
    //                    .Where(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday) // Skip weekends
    //                    .ToList();
    //        }


    //        // Generate and save schedules for each date
    //        foreach (var date in dates)
    //        {
    //            var newSchedule = new Schedule
    //            {
    //                Date = date,
    //                DistNumber = schedule.DistNumber,
    //                DistName = schedule.DistName,
    //                StartDate = schedule.StartDate,
    //                EndDate = schedule.EndDate,
    //                PersonID = schedule.PersonID,
    //                SiteID = schedule.SiteID,
    //                Position = schedule.Position,
    //                SiteType = schedule.SiteType,
    //                SiteName = schedule.SiteName,

    //                MondayTimeIn = schedule.MondayTimeIn,
    //                MondayTimeOut = schedule.MondayTimeOut,
    //                TuesdayTimeIn = schedule.TuesdayTimeIn,
    //                TuesdayTimeOut = schedule.TuesdayTimeOut,
    //                WednesdayTimeIn = schedule.WednesdayTimeIn,
    //                WednesdayTimeOut = schedule.WednesdayTimeOut,
    //                ThursdayTimeIn = schedule.ThursdayTimeIn,
    //                ThursdayTimeOut = schedule.ThursdayTimeOut,
    //                FridayTimeIn = schedule.FridayTimeIn,
    //                FridayTimeOut = schedule.FridayTimeOut,

    //                Notes = schedule.Notes,
    //                Paycode = schedule.Paycode,
    //            };

    //            allSchedule.Add(newSchedule);
    //        }


    //        //if (schedule.Date.HasValue || schedule.UpdatedDate.HasValue || schedule.StartDate.HasValue || schedule.EndDate.HasValue)
    //        //{
    //        //    createdSchedules.Add(await _scheduleRepository.AddScheduleAsync(schedule));
    //        //}

    //        //// Create a duplicate of the schedule with DeletedDate and DeletedSiteType as null if DeletedDate is not null
    //        //if (schedule.DeletedDate.HasValue && (schedule.Date.HasValue || schedule.UpdatedDate.HasValue))
    //        //{
    //        //    var duplicateSchedule = new Schedule
    //        //    {
    //        //        Date = null,
    //        //        DeletedDate = null,
    //        //        DeletedSiteType = null,
    //        //        UpdatedDate = schedule.UpdatedDate,
    //        //        DistNumber = schedule.DistNumber,
    //        //        DistName = schedule.DistName,
    //        //        StartDate = schedule.StartDate,
    //        //        EndDate = schedule.EndDate,
    //        //        PersonID = schedule.PersonID,
    //        //        SiteID = schedule.SiteID,
    //        //        Position = schedule.Position,
    //        //        SiteType = schedule.SiteType,
    //        //        SiteName = schedule.SiteName,

    //        //        MondayTimeIn = schedule.MondayTimeIn,
    //        //        MondayTimeOut = schedule.MondayTimeOut,
    //        //        TuesdayTimeIn = schedule.TuesdayTimeIn,
    //        //        TuesdayTimeOut = schedule.TuesdayTimeOut,
    //        //        WednesdayTimeIn = schedule.WednesdayTimeIn,
    //        //        WednesdayTimeOut = schedule.WednesdayTimeOut,
    //        //        ThursdayTimeIn = schedule.ThursdayTimeIn,
    //        //        ThursdayTimeOut = schedule.ThursdayTimeOut,
    //        //        FridayTimeIn = schedule.FridayTimeIn,
    //        //        FridayTimeOut = schedule.FridayTimeOut,

    //        //        Notes = schedule.Notes,
    //        //        Paycode = schedule.Paycode,
    //        //    };


    //        //    createdSchedules.Add(await _scheduleRepository.AddScheduleAsync(duplicateSchedule));
    //        //}
    //        //else if (schedule.DeletedDate.HasValue && !schedule.Date.HasValue && !schedule.UpdatedDate.HasValue)
    //        //{

    //        //    var duplicateSchedule = new Schedule
    //        //    {
    //        //        Date = null,
    //        //        DeletedDate = schedule.DeletedDate,
    //        //        DeletedSiteType = schedule.DeletedSiteType,
    //        //        UpdatedDate = schedule.UpdatedDate,
    //        //        DistNumber = schedule.DistNumber,
    //        //        DistName = schedule.DistName,
    //        //        StartDate = schedule.StartDate,
    //        //        EndDate = schedule.EndDate,
    //        //        PersonID = schedule.PersonID,
    //        //        SiteID = schedule.SiteID,
    //        //        Position = schedule.Position,
    //        //        SiteType = schedule.SiteType,
    //        //        SiteName = schedule.SiteName,

    //        //        MondayTimeIn = schedule.MondayTimeIn,
    //        //        MondayTimeOut = schedule.MondayTimeOut,
    //        //        TuesdayTimeIn = schedule.TuesdayTimeIn,
    //        //        TuesdayTimeOut = schedule.TuesdayTimeOut,
    //        //        WednesdayTimeIn = schedule.WednesdayTimeIn,
    //        //        WednesdayTimeOut = schedule.WednesdayTimeOut,
    //        //        ThursdayTimeIn = schedule.ThursdayTimeIn,
    //        //        ThursdayTimeOut = schedule.ThursdayTimeOut,
    //        //        FridayTimeIn = schedule.FridayTimeIn,
    //        //        FridayTimeOut = schedule.FridayTimeOut,

    //        //        Notes = schedule.Notes,
    //        //        Paycode = schedule.Paycode,
    //        //    };


    //        //    createdSchedules.Add(await _scheduleRepository.AddScheduleAsync(duplicateSchedule));
    //        //}
    //        //------------------------------------------------------------------------------------
    //        //var createdScheduleDto = _mapper.Map<List<ScheduleDto>>(createdSchedules);

    //        //foreach (var createdSchedule in createdScheduleDto)
    //        //{
    //        //    var scheduleRequest = new List<ScheduleDto>();
    //        //    scheduleRequest.Add(createdSchedule);
    //        //    var response = await _restClientService.PostAsync<ScheduleDto>("Schedules", scheduleRequest);

    //        //    if (response.StatusCode != System.Net.HttpStatusCode.OK)
    //        //    {
    //        //        // Extract error message from the response content
    //        //        string errorMessage = "Failed to add schedule on external API.";
    //        //        if (!string.IsNullOrEmpty(response.Content))
    //        //        {
    //        //            try
    //        //            {
    //        //                var contentJson = JObject.Parse(response.Content);
    //        //                errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
    //        //            }
    //        //            catch (Exception parseEx)
    //        //            {
    //        //                _logger.LogError(parseEx, "Failed to parse error message from API response.");
    //        //            }
    //        //        }

    //        //        _logger.LogError("Failed to add schedule on external API: {0}", errorMessage);
    //        //        await transaction.RollbackAsync(); // Rollback transaction if any API call fails
    //        //        return new GenericResponse<List<ScheduleDto>>(false, errorMessage, null);
    //        //    }
    //        //}

    //        await transaction.CommitAsync(); // Commit transaction if all API calls are successful
    //        return new GenericResponse<List<ScheduleDto>>(true, "Schedule added successfully.", null);

    //    }
    //    catch (Exception ex)
    //    {
    //        await transaction.RollbackAsync(); // Rollback on exception
    //        _logger.LogError(ex, "An error occurred while adding a schedule.");
    //        return new GenericResponse<List<ScheduleDto>>(false, "An error occurred while adding the schedule.", null);
    //    }
    //}


    //public async Task<GenericResponse<ScheduleDto>> UpdateScheduleAsync(ScheduleDto scheduleDto)
    //{
    //    try
    //    {
    //        var schedule = _mapper.Map<Schedule>(scheduleDto);
    //        var updatedSchedule = await _scheduleRepository.UpdateScheduleAsync(schedule);
    //        var updatedScheduleDto = _mapper.Map<ScheduleDto>(updatedSchedule);
    //        // Use RestClientService to make the API call
    //        var response = await _restClientService.PutAsync<ScheduleDto>($"Schedules/{scheduleDto.Id}", updatedScheduleDto);

    //        if (response.IsSuccessful)
    //        {
    //            return new GenericResponse<ScheduleDto>(true, "Schedule updated successfully.", response.Data);
    //        }
    //        else
    //        {
    //            // Extract the error message from the response content
    //            string errorMessage = "Failed to update schedule on external API.";
    //            if (!string.IsNullOrEmpty(response.Content))
    //            {
    //                try
    //                {
    //                    // Parse the response content to extract the error message if it exists
    //                    var contentJson = JObject.Parse(response.Content);
    //                    errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
    //                }
    //                catch (Exception parseEx)
    //                {
    //                    _logger.LogError(parseEx, "Failed to parse error message from API response.");
    //                }
    //            }

    //            _logger.LogError("Failed to update schedule on external API: {0}", errorMessage);
    //            return new GenericResponse<ScheduleDto>(false, errorMessage, null);
    //        }
    //        //return new GenericResponse<ScheduleDto>(true, "Schedule updated successfully.", updatedScheduleDto);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while updating the schedule.");
    //        return new GenericResponse<ScheduleDto>(false, "An error occurred while updating the schedule.", null);
    //    }
    //}

    public async Task<GenericResponse<bool>> DeleteScheduleAsync(long id)
    {
        try
        {
            bool deleted = await _scheduleRepository.DeleteScheduleAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Schedule not found or deletion failed.", id);
                return new GenericResponse<bool>(false, "Schedule not found or deletion failed.", false);
            }

            return new GenericResponse<bool>(true, "Schedule deleted successfully.", true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the schedule with ID {ScheduleId}.", id);
            return new GenericResponse<bool>(false, "An error occurred while deleting the schedule.", false);
        }
    }

    public async Task<GenericResponse<List<ScheduleDto>>> GetScheduleByPersonIdAsync(long personId)
    {
        try
        {
            var schedules = await _scheduleRepository.GetScheduleByPersonIdAsync(personId);
            if (schedules == null)
            {
                _logger.LogWarning("No schedules found for person ID {PersonId}.", personId);
                return new GenericResponse<List<ScheduleDto>>(false, "No schedules found.", null);
            }

            if (schedules.Count() == 0)
            {
                _logger.LogWarning("No schedules found for person ID {PersonId}.", personId);
                return new GenericResponse<List<ScheduleDto>>(true, "No schedules found.", []);
            }

            var scheduleDtos = _mapper.Map<List<ScheduleDto>>(schedules);
            return new GenericResponse<List<ScheduleDto>>(true, "Schedules retrieved successfully.", scheduleDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the schedules for person ID {PersonId}.", personId);
            return new GenericResponse<List<ScheduleDto>>(false, "An error occurred while retrieving the schedules.", null);
        }
    }



    public async Task<GenericResponse<List<ScheduleDto>>> GetScheduleByPersonIdAndDateRangeAsync(long personId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var schedules = await _scheduleRepository.GetScheduleByPersonIdAndDateRangeAsync(personId, startDate, endDate);

            if (schedules == null || schedules.Count() == 0)
            {
                _logger.LogWarning("No schedules found for person ID {PersonId} between {StartDate} and {EndDate}.", personId, startDate, endDate);
                return new GenericResponse<List<ScheduleDto>>(true, "No schedules found.", []);
            }

            var scheduleDtos = _mapper.Map<List<ScheduleDto>>(schedules);
            return new GenericResponse<List<ScheduleDto>>(true, "Schedules retrieved successfully.", scheduleDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the schedules for person ID {PersonId} between {StartDate} and {EndDate}.", personId, startDate, endDate);
            return new GenericResponse<List<ScheduleDto>>(false, "An error occurred while retrieving the schedules.", null);
        }
    }

    public async Task<GenericResponse<Dictionary<string, object>>> ImportSchedulesAsync(IFormFile file)
    {
        var schedules = new List<Schedule>();
        var statuses = new List<EmployeeScheduleImportStatus>();
        var failedRecords = new List<Dictionary<string, string>>();

        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Read Excel file
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                {
                    return new GenericResponse<Dictionary<string, object>>(false, "No worksheet found in the file.", null);
                }

                var existingSchedules = await _context.Schedules.ToListAsync();
                            

                // Process rows
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    try
                    {
                        // Extract data
                        var personId = Convert.ToInt64(worksheet.Cells[row, 1].Value);
                        //var date = DateTime.TryParse(worksheet.Cells[row, 2].Value?.ToString(), out var parsedDate) ? parsedDate : (DateTime?)null;
                        var startDate = DateTime.TryParse(worksheet.Cells[row, 2].Value?.ToString(), out var parsedStartDate) ? parsedStartDate : (DateTime?)null;
                        var endDate = DateTime.TryParse(worksheet.Cells[row, 3].Value?.ToString(), out var parsedEndDate) ? parsedEndDate : (DateTime?)null;
                        var siteId = Convert.ToInt64(worksheet.Cells[row, 4].Value);
                        var position = worksheet.Cells[row, 5].Value?.ToString();
                        var siteName = worksheet.Cells[row, 6].Value?.ToString();
                        var distNumber = Convert.ToInt64(worksheet.Cells[row, 7].Value);
                        var distName = worksheet.Cells[row, 8].Value?.ToString();
                        var siteType = worksheet.Cells[row, 9].Value?.ToString();
                        var notes = worksheet.Cells[row, 10].Value?.ToString();
                        var mondayTimeIn = worksheet.Cells[row, 11].Value?.ToString();
                        var mondayTimeOut = worksheet.Cells[row, 12].Value?.ToString();
                        var tuesdayTimeIn = worksheet.Cells[row, 13].Value?.ToString();
                        var tuesdayTimeOut = worksheet.Cells[row, 14].Value?.ToString();
                        var wednesdayTimeIn = worksheet.Cells[row, 15].Value?.ToString();
                        var wednesdayTimeOut = worksheet.Cells[row, 16].Value?.ToString();
                        var thursdayTimeIn = worksheet.Cells[row, 17].Value?.ToString();
                        var thursdayTimeOut = worksheet.Cells[row, 18].Value?.ToString();
                        var fridayTimeIn = worksheet.Cells[row, 19].Value?.ToString();
                        var fridayTimeOut = worksheet.Cells[row, 20].Value?.ToString();

                        // Validate mandatory fields
                        if (personId == 0 || string.IsNullOrEmpty(siteType))
                        {
                            failedRecords.Add(new Dictionary<string, string>
                            {
                                 { "Row", row.ToString() },
                                 { "Error", "Missing required fields (PersonId or SiteType)." }
                            });
                            continue;
                        }

                        // Check for duplicates
                        var existingSchedule = existingSchedules.Find(s => s.PersonID == personId && s.SiteType == siteType);

                        if (existingSchedule != null)
                        {
                            statuses.Add(new EmployeeScheduleImportStatus
                            {
                                Id = Guid.NewGuid(),
                                PersonId = personId,
                                SiteType = siteType,
                                ImportedOn = DateTime.Now,
                                ImportStatus = "Duplicate",
                            });
                            continue;
                        }

                        // Create new schedule
                        var schedule = new Schedule
                        {
                            PersonID = personId,
                            //Date = date,
                            StartDate = startDate ?? DateTime.Now,
                            EndDate = endDate ?? DateTime.Now.AddYears(5),
                            SiteID = siteId,
                            Position = position,
                            SiteName = siteName,
                            DistNumber = distNumber,
                            DistName = distName,
                            SiteType = siteType,
                            Notes = notes,
                            //MondayTimeIn = TimeSpan.TryParse(mondayTimeIn, out var monIn) ? monIn : null,
                            //MondayTimeOut = TimeSpan.TryParse(mondayTimeOut, out var monOut) ? monOut : null,
                            //TuesdayTimeIn = TimeSpan.TryParse(tuesdayTimeIn, out var tueIn) ? tueIn : null,
                            //TuesdayTimeOut = TimeSpan.TryParse(tuesdayTimeOut, out var tueOut) ? tueOut : null,
                            //WednesdayTimeIn = TimeSpan.TryParse(wednesdayTimeIn, out var wedIn) ? wedIn : null,
                            //WednesdayTimeOut = TimeSpan.TryParse(wednesdayTimeOut, out var wedOut) ? wedOut : null,
                            //ThursdayTimeIn = TimeSpan.TryParse(thursdayTimeIn, out var thuIn) ? thuIn : null,
                            //ThursdayTimeOut = TimeSpan.TryParse(thursdayTimeOut, out var thuOut) ? thuOut : null,
                            //FridayTimeIn = TimeSpan.TryParse(fridayTimeIn, out var friIn) ? friIn : null,
                            //FridayTimeOut = TimeSpan.TryParse(fridayTimeOut, out var friOut) ? friOut : null,
                            MondayTimeIn = ParseTimeSpan(mondayTimeIn),
                            MondayTimeOut = ParseTimeSpan(mondayTimeOut),
                            TuesdayTimeIn = ParseTimeSpan(tuesdayTimeIn),
                            TuesdayTimeOut = ParseTimeSpan(tuesdayTimeOut),
                            WednesdayTimeIn = ParseTimeSpan(wednesdayTimeIn),
                            WednesdayTimeOut = ParseTimeSpan(wednesdayTimeOut),
                            ThursdayTimeIn = ParseTimeSpan(thursdayTimeIn),
                            ThursdayTimeOut = ParseTimeSpan(thursdayTimeOut),
                            FridayTimeIn = ParseTimeSpan(fridayTimeIn),
                            FridayTimeOut = ParseTimeSpan(fridayTimeOut),
                        };
                        schedules.Add(schedule);

                        // Add success status
                        statuses.Add(new EmployeeScheduleImportStatus
                        {
                            Id = Guid.NewGuid(),
                            PersonId = personId,
                            SiteType = siteType,
                            ImportedOn = DateTime.Now,
                            ImportStatus = "Success",
                        });
                    }
                    catch (Exception ex)
                    {
                        failedRecords.Add(new Dictionary<string, string>
                            {
                                { "Row", row.ToString() },
                                { "Error", ex.Message }
                             });
                    }
                }
            }


            // Bulk insert into the database
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (schedules.Any())
                {
                    await _context.BulkInsertAsync(schedules);
                }
                if (statuses.Any())
                {
                    await _context.BulkInsertAsync(statuses);
                }
                //await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred during the bulk insert transaction.");
                throw; // Rethrow the exception to be handled by the calling code
            }

            // Return response
            var responseData = new Dictionary<string, object>
            {
                { "SuccessCount", statuses.Count(s => s.ImportStatus == "Success") },
                { "DuplicateCount", statuses.Count(s => s.ImportStatus == "Duplicate") },
                { "FailedRecords", failedRecords }
            };

            return new GenericResponse<Dictionary<string, object>>(true, "Schedules imported successfully.", responseData);
        }
        catch (Exception ex)
        {
            return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
        }
    }

    private TimeSpan? ParseTimeSpan(string time)
    {
        if (TimeSpan.TryParse(time, out var parsedTime))
        {
            // Validate that the time is within the allowed range for SqlDbType.Time
            if (parsedTime >= TimeSpan.Zero && parsedTime < TimeSpan.FromDays(1))
            {
                return parsedTime;
            }
        }
        return null; // Return null for invalid or out-of-range values
    }

}
