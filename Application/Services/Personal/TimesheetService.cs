using Application.Helpers;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.Core.DTOs;
using Core.DTOs.Personel;
using Core.Entities;
using Core.Entities.Brainyclock;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IPersonelRepository _personelRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly GoogleMapsGeocodingServiceHelper _geocodingServiceHelper;
        private readonly IMapper _mapper;
        private readonly ILogger<TimesheetService> _logger;
        private readonly MySqlDbContext _brainyclockContext;

        public TimesheetService(ITimesheetRepository timesheetRepository,
            IMapper mapper,
            ILogger<TimesheetService> logger,
            IScheduleRepository scheduleRepository,
            IDistrictRepository districtRepository,
            ISchoolRepository schoolRepository,
            GoogleMapsGeocodingServiceHelper geocodingServiceHelper,
            ISiteRepository siteRepository,
            IPersonelRepository personelRepository,
            IAttendanceRepository attendanceRepository,
            MySqlDbContext brainyclockContext)
        {
            _scheduleRepository = scheduleRepository;
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _logger = logger;
            _districtRepository = districtRepository;
            _schoolRepository = schoolRepository;
            _geocodingServiceHelper = geocodingServiceHelper;
            _siteRepository = siteRepository;
            _personelRepository = personelRepository;
            _attendanceRepository = attendanceRepository;
            _brainyclockContext = brainyclockContext;
        }

        public async Task<GenericResponse<IEnumerable<TimesheetDto>>> GetTimesheetsWithFilterAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _timesheetRepository.GetAllTimesheets();

                // Apply filtering if needed
                //if (!string.IsNullOrWhiteSpace(search))
                //{
                //    query = query.Where(t => t.Description.Contains(search));
                //}

                var totalItems = await query.CountAsync();
                var timesheets = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                var timesheetDtos = _mapper.Map<IEnumerable<TimesheetDto>>(timesheets);

                return new GenericResponse<IEnumerable<TimesheetDto>>(true, "Timesheets retrieved successfully.", timesheetDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving timesheets.");
                return new GenericResponse<IEnumerable<TimesheetDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<TimesheetDto>>> GetAllTimesheetsAsync()
        {
            try
            {
                var timesheets = await _timesheetRepository.GetAllTimesheetsAsync();
                var timesheetDtos = _mapper.Map<List<TimesheetDto>>(timesheets);
                return new GenericResponse<List<TimesheetDto>>(true, "All timesheets retrieved successfully.", timesheetDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all timesheets.");
                return new GenericResponse<List<TimesheetDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<TimesheetDto>> GetTimesheetByIdAsync(long id)
        {
            try
            {
                var timesheet = await _timesheetRepository.GetTimesheetByIdAsync(id);
                if (timesheet == null)
                {
                    _logger.LogWarning("Timesheet with ID {TimesheetId} not found.", id);
                    return new GenericResponse<TimesheetDto>(false, "Timesheet not found.", null);
                }

                var timesheetDto = _mapper.Map<TimesheetDto>(timesheet);
                return new GenericResponse<TimesheetDto>(true, "Timesheet retrieved successfully.", timesheetDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving timesheet with ID {TimesheetId}.", id);
                return new GenericResponse<TimesheetDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<TimesheetDto>> CreateTimesheetAsync(TimesheetDto timesheetDto)
        {
            try
            {
                var timesheet = _mapper.Map<Timesheet>(timesheetDto);
                if (timesheet.ExternalEventId != null)
                {
                    if (timesheetDto.DistrictID != null)
                    {
                        var existingDistrict = await _districtRepository.GetDistrictByDistNumberAsync((long)timesheetDto.DistrictID);

                        if (existingDistrict != null)
                        {
                            timesheet.DistrictID = existingDistrict.DistrictID;
                        }
                        else
                        {
                            return new GenericResponse<TimesheetDto>(false, "District number is missing in scope", timesheetDto);
                        }
                    }
                    else
                    {
                        return new GenericResponse<TimesheetDto>(false, "District number is missing", timesheetDto);
                    }
                }
                var isTimesheetAvailable = await _timesheetRepository.GetTimesheetBySiteAndDateAsync(timesheet.TimeSheetDate, timesheet.SiteID, timesheet.PersonID);

                if (isTimesheetAvailable != null)
                {
                    return new GenericResponse<TimesheetDto>(false, "Timesheet already marked.", null);
                }
                var createdTimesheet = await _timesheetRepository.AddTimesheetAsync(timesheet);
                var createdTimesheetDto = _mapper.Map<TimesheetDto>(createdTimesheet);
                return new GenericResponse<TimesheetDto>(true, "Timesheet created successfully.", createdTimesheetDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new timesheet.");
                return new GenericResponse<TimesheetDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<TimesheetDto>> UpdateTimesheetAsync(long id, TimesheetDto timesheetDto)
        {
            try
            {
                var timesheet = _mapper.Map<Timesheet>(timesheetDto);
                timesheet.TimesheetID = id;

                var updatedTimesheet = await _timesheetRepository.UpdateTimesheetAsync(timesheet);
                if (updatedTimesheet == null)
                {
                    _logger.LogWarning("Timesheet with ID {TimesheetId} not found or update failed.", id);
                    return new GenericResponse<TimesheetDto>(false, "Timesheet not found or update failed.", null);
                }

                var updatedTimesheetDto = _mapper.Map<TimesheetDto>(updatedTimesheet);
                return new GenericResponse<TimesheetDto>(true, "Timesheet updated successfully.", updatedTimesheetDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating timesheet with ID {TimesheetId}.", id);
                return new GenericResponse<TimesheetDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<TimesheetDto>>> BulkUpdateTimesheetsAsync(List<TimesheetDto> timesheets)
        {
            try
            {
                // Separate timesheets into two lists: for update and add
                var updateList = timesheets.Where(t => t.TimesheetID > 0).ToList();
                var addList = timesheets.Where(t => t.TimesheetID == 0 || t.TimesheetID == null).ToList();

                // Map the DTOs to entities
                var timesheetsToUpdate = _mapper.Map<List<Timesheet>>(updateList);
                var timesheetsToAdd = _mapper.Map<List<Timesheet>>(addList);

                var districts = await _districtRepository.GetAllDistrictsAsync();

                var brainyclockSchedules = await _brainyclockContext.Employee
                        .Select(x => new Employee
                        {
                            Id = x.Id,
                            EmployeeId = x.EmployeeId
                        })
                    .Where(x => x.EmployeeId != null).ToListAsync();

                // Update existing timesheets
                var updatedTimesheets = new List<Timesheet>();
                if (timesheetsToUpdate.Any())
                {
                    updatedTimesheets = await _timesheetRepository.BulkUpdateTimesheetsAsync(timesheetsToUpdate);

                    //update to brainyclock app
                    var scheduleIds = updateList.Select(x => x.scheduleId).ToList();
                    var brainyclockAttendances = await _brainyclockContext.Attendance
                        .Where(x => x.EventId != null && scheduleIds.Contains(x.EventId))
                        .ToListAsync(); // Run query first

                    var allMatchedAttendances = new List<BrainyclockAttendance>();
                    updateList.ForEach(t =>
                    {
                        var matchedAttendances = brainyclockAttendances
                            .Where(x => x.EventId == t.scheduleId)
                            .ToList();

                        matchedAttendances.ForEach(x =>
                        {
                            x.ClockIn = string.IsNullOrEmpty(t.Paycode) ?
                             (string.IsNullOrEmpty(t.TimeIn.ToString()) ? null : t.TimeIn) :
                             (string.IsNullOrEmpty(t.AdditionalStart.ToString()) ? null : t.AdditionalStart);
                            x.ClockOut = string.IsNullOrEmpty(t.Paycode) ?
                            (string.IsNullOrEmpty(t.TimeOut.ToString()) ? null : t.TimeOut) :
                            (string.IsNullOrEmpty(t.AdditionalStop.ToString()) ? null : t.AdditionalStop);
                        });
                        allMatchedAttendances.AddRange(matchedAttendances);
                    });
                    _brainyclockContext.Attendance.UpdateRange(allMatchedAttendances);
                    await _brainyclockContext.SaveChangesAsync();
                }

                // Add new timesheets
                var addedTimesheets = new List<Timesheet>();
                if (timesheetsToAdd.Any())
                {
                    addedTimesheets = await _timesheetRepository.BulkAddTimesheetsAsync(timesheetsToAdd);

                    //update to brainyclock app
                    var personalIds = addList
                                        .Where(x => x.PersonID.HasValue)
                                        .Select(x => (int)x.PersonID.Value)
                                        .ToList();

                    List<BrainyclockAttendance> attendanceList = new List<BrainyclockAttendance>();
                    addList.ForEach(t =>
                    {
                        var district = districts.FirstOrDefault(x => x.DistrictID == t.DistrictID.GetValueOrDefault());
                        var DistNumber = district != null ? (int)district.DIST_NUM : 0;

                        var employee = brainyclockSchedules.FirstOrDefault(x => int.Parse(x.EmployeeId) == (int)t.PersonID.Value);

                        attendanceList.Add(
                        new BrainyclockAttendance
                        {
                            CompanyId = 414,
                            ShiftId = null,
                            EmployeeId = employee.Id,
                            ClockIn = string.IsNullOrEmpty(t.Paycode)? 
                            (string.IsNullOrEmpty(t.TimeIn.ToString()) ? null : t.TimeIn) :
                            (string.IsNullOrEmpty(t.AdditionalStart.ToString()) ? null : t.AdditionalStart),
                            ClockOut = string.IsNullOrEmpty(t.Paycode) ?
                            (string.IsNullOrEmpty(t.TimeOut.ToString()) ? null : t.TimeOut) :
                            (string.IsNullOrEmpty(t.AdditionalStop.ToString()) ? null : t.AdditionalStop),
                            LunchIn = null,
                            LunchOut = null,
                            Overtime = 0,
                            CreatedAt = DateTime.Today,
                            DepartmentId = null,
                            MarkAttendanceBy = employee.Id,
                            EventId = t.scheduleId.GetValueOrDefault(),
                            SiteId = t.SiteID.HasValue ? (int)t.SiteID.Value : 0,
                            DistNumber = t.distNumber.GetValueOrDefault(),
                        });
                    });
                    await _brainyclockContext.Attendance.AddRangeAsync(attendanceList);
                    await _brainyclockContext.SaveChangesAsync();

                }

                // Combine results
                var allTimesheets = updatedTimesheets.Concat(addedTimesheets).ToList();

                if (!allTimesheets.Any())
                {
                    _logger.LogWarning("No timesheets were updated or added.");
                    return new GenericResponse<List<TimesheetDto>>(false, "No timesheets were updated or added.", null);
                }

                // Map back to DTOs
                var allTimesheetDtos = _mapper.Map<List<TimesheetDto>>(allTimesheets);

                return new GenericResponse<List<TimesheetDto>>(true, "Timesheets processed successfully.", allTimesheetDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating timesheets in bulk.");
                return new GenericResponse<List<TimesheetDto>>(false, ex.Message, null);
            }
        }


        public async Task<GenericResponse<bool>> DeleteTimesheetAsync(long id)
        {
            try
            {
                var deleted = await _timesheetRepository.DeleteTimesheetAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Timesheet with ID {TimesheetId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "Timesheet not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "Timesheet deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting timesheet with ID {TimesheetId}.", id);
                return new GenericResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<GenericResponse<IEnumerable<dynamic>>> GetTimesheetsByPersonIdAsync(long personId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var timesheets = await _timesheetRepository.GetTimesheetsByPersonIdAsync(personId, startDate, endDate);
                //var timesheetDtos = _mapper.Map<List<TimesheetDto>>(timesheets);
                if (timesheets == null || !timesheets.Any())
                {
                    return new GenericResponse<IEnumerable<dynamic>>(true, "No timesheets found.", null);
                }

                var personIds = timesheets
                    .Select(t => (long?)t.PersonID) // Assuming PersonID is part of the dynamic object
                    .Distinct()
                    .ToList();

                // Fetch schedules for each distinct PersonID
                var schedulesByPersonId = await _scheduleRepository.GetSchedulesByPersonIdsAsync(personIds);

                // Fetch attendance records for all distinct PersonIDs
                var attendanceByPersonId = await _attendanceRepository.GetAttendanceByPersonIdsAsync(personIds);


                // Add schedules to each timesheet
                var enrichedTimesheets = timesheets.Select(timesheet =>
                {
                    var personId = (long)timesheet.PersonID;
                    var schedules = schedulesByPersonId.Where(s => s.PersonID == personId).ToList();
                    var attendance = attendanceByPersonId.Where(a => a.STAFF_ID == personId && a.DATE >= startDate && a.DATE <= endDate).ToList();

                    // Add schedules to the timesheet dynamically
                    var timesheetWithSchedules = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var property in (IDictionary<string, object>)timesheet)
                    {
                        timesheetWithSchedules[property.Key] = property.Value;
                    }

                    timesheetWithSchedules["Schedules"] = schedules;
                    timesheetWithSchedules["Attendance"] = attendance;
                    return timesheetWithSchedules;
                });



                return new GenericResponse<IEnumerable<dynamic>>(true, "Timesheets retrieved successfully.", enrichedTimesheets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving timesheets.");
                return new GenericResponse<IEnumerable<dynamic>>(false, "An error occurred while retrieving timesheets.", null);
            }
        }

        //public async Task<GenericResponse<IEnumerable<dynamic>>> GetTimesheetsBySiteAndDistrictAsync(
        //     int siteId, int distNumber, DateTime? startDate, DateTime? endDate)
        //{
        //    try
        //    {
        //        // Fetch timesheets using the repository
        //        var timesheets = await _timesheetRepository.GetTimesheetsBySiteAndDistrictAsync(siteId, distNumber, startDate, endDate);

        //        return new GenericResponse<IEnumerable<dynamic>>(true, "Timesheets retrieved successfully.", timesheets);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while retrieving timesheets.");
        //        return new GenericResponse<IEnumerable<dynamic>>(false, "An error occurred while retrieving timesheets.", null);
        //    }
        //}


        //public async Task<GenericResponse<IEnumerable<dynamic>>> GetTimesheetsBySiteAndDistrictAsync(
        //    int siteId, int distNumber, DateTime? startDate, DateTime? endDate)
        //{
        //    try
        //    {
        //        // Fetch timesheets using the repository
        //        var timesheets = await _timesheetRepository.GetTimesheetsBySiteAndDistrictAsync(siteId, distNumber, startDate, endDate);

        //        if (timesheets == null || !timesheets.Any())
        //        {
        //            return new GenericResponse<IEnumerable<dynamic>>(true, "No timesheets found.", null);
        //        }

        //        // Extract distinct PersonIDs from timesheets
        //        var personIds = timesheets
        //            .Select(t => (long?)t.PersonID) // Assuming PersonID is part of the dynamic object
        //            .Distinct()
        //            .ToList();

        //        // Fetch schedules for each distinct PersonID
        //        var schedulesByPersonId = await _scheduleRepository.GetSchedulesByPersonIdsAsync(personIds);

        //        // Fetch attendance records for all distinct PersonIDs
        //        var attendanceByPersonId = await _attendanceRepository.GetAttendanceByPersonIdsAsync(personIds);


        //        // Add schedules to each timesheet
        //        var enrichedTimesheets = timesheets.Select(timesheet =>
        //        {
        //            var personId = (long)timesheet.PersonID;
        //            var schedules = schedulesByPersonId.Where(s => s.PersonID == personId).ToList();
        //            var attendance = attendanceByPersonId.Where(a => a.STAFF_ID == personId && a.DATE >= startDate && a.DATE <= endDate).ToList();

        //            // Add schedules to the timesheet dynamically
        //            var timesheetWithSchedules = new ExpandoObject() as IDictionary<string, object>;
        //            foreach (var property in (IDictionary<string, object>)timesheet)
        //            {
        //                timesheetWithSchedules[property.Key] = property.Value;
        //            }

        //            timesheetWithSchedules["Schedules"] = schedules;
        //            timesheetWithSchedules["Attendance"] = attendance;
        //            return timesheetWithSchedules;
        //        });

        //        return new GenericResponse<IEnumerable<dynamic>>(true, "Timesheets retrieved successfully.", enrichedTimesheets);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while retrieving timesheets.");
        //        return new GenericResponse<IEnumerable<dynamic>>(false, "An error occurred while retrieving timesheets.", null);
        //    }
        //}

        //    public async Task<GenericResponse<IEnumerable<dynamic>>> GetTimesheetsBySiteAndDistrictAsync(
        //int siteId, int distNumber, DateTime? startDate, DateTime? endDate)
        //    {
        //        try
        //        {
        //            // Fetch timesheets using the repository
        //            var timesheets = await _timesheetRepository.GetTimesheetsBySiteAndDistrictAsync(siteId, distNumber, startDate, endDate);

        //            if (timesheets == null || !timesheets.Any())
        //            {
        //                return new GenericResponse<IEnumerable<dynamic>>(true, "No timesheets found.", null);
        //            }

        //            // Extract distinct PersonIDs from timesheets
        //            var personIds = timesheets
        //                .Select(t => (long?)t.PersonID) // Assuming PersonID is part of the dynamic object
        //                .Distinct()
        //                .ToList();

        //            // Fetch schedules for each distinct PersonID
        //            var schedulesByPersonId = await _scheduleRepository.GetSchedulesByPersonIdsAsync(personIds);

        //            // Fetch attendance records for all distinct PersonIDs
        //            var attendanceByPersonId = await _attendanceRepository.GetAttendanceByPersonIdsAsync(personIds);

        //            // Group timesheets by PersonID
        //            var groupedByPerson = timesheets.GroupBy(t => (long)t.PersonID);

        //            // Structure data as required
        //            var personList = await groupedByPerson.Select(personGroup =>
        //            {
        //                var personId = personGroup.Key;
        //                var personSchedules = schedulesByPersonId.Where(s => s.PersonID == personId).ToList();
        //                var personAttendance = attendanceByPersonId.Where(a => a.STAFF_ID == personId && a.DATE >= startDate && a.DATE <= endDate).ToList();

        //                // Enrich each schedule with its associated timesheets
        //                var schedulesWithTimesheets = personSchedules.Select(schedule =>
        //                {
        //                    var scheduleTimesheets = personGroup.Where(t => t.PersonID.ToString() == schedule.PersonID.ToString() && t.SiteID.ToString() == schedule.SiteID.ToString()).ToList(); // Assuming ScheduleID links timesheets to schedules

        //                    // Create a dynamic object and manually add properties from the schedule
        //                    var enrichedSchedule = new ExpandoObject() as IDictionary<string, object>;

        //                    // Use reflection to copy properties from the schedule object
        //                    foreach (var property in schedule.GetType().GetProperties())
        //                    {
        //                        //if(schedule.Personel || schedule.Site)
        //                        //{

        //                        //}
        //                        enrichedSchedule[property.Name] = property.GetValue(schedule);
        //                    }

        //                    // Add Timesheets to the enriched schedule
        //                    enrichedSchedule["Timesheets"] = scheduleTimesheets;

        //                    return enrichedSchedule;
        //                }).ToListAsync();

        //                // Structure person object
        //                var personData = new ExpandoObject() as IDictionary<string, object>;
        //                personData["PersonID"] = personId;
        //                personData["FIRSTNAME"] = personGroup.FirstOrDefault()?.FIRSTNAME; // Assuming PersonName is part of timesheets
        //                personData["LASTNAME"] = personGroup.FirstOrDefault()?.LASTNAME; // Assuming PersonName is part of timesheets
        //                personData["Schedules"] = schedulesWithTimesheets;
        //                personData["Attendance"] = personAttendance;

        //                return personData;
        //            });

        //            return new GenericResponse<IEnumerable<dynamic>>(true, "Data retrieved successfully.", personList);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "An error occurred while retrieving data.");
        //            return new GenericResponse<IEnumerable<dynamic>>(false, "An error occurred while retrieving data.", null);
        //        }
        //    }

        public async Task<GenericResponse<IEnumerable<dynamic>>> GetTimesheetsBySiteAndDistrictAsync(
    int siteId, int distNumber, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // Fetch timesheets using the repository
                var timesheets = await _timesheetRepository.GetTimesheetsBySiteAndDistrictAsync(siteId, distNumber, startDate, endDate);

                if (timesheets == null || !timesheets.Any())
                {
                    return new GenericResponse<IEnumerable<dynamic>>(true, "No timesheets found.", null);
                }

                // Extract distinct PersonIDs from timesheets
                var personIds = timesheets
                    .Select(t => (long?)t.PersonID)
                    .Distinct()
                    .ToList();

                // Fetch schedules for each distinct PersonID
                var schedulesByPersonId = await _scheduleRepository.GetSchedulesByPersonIdsAsync(personIds);

                // Fetch attendance records for all distinct PersonIDs
                var absentData = await _attendanceRepository.GetAttendanceByPersonIdsAsync(personIds);
                var attendanceSiteIds = await _siteRepository.GetAllSitesBySiteNumber(absentData.Select(x => x.SITENUM).ToList()); ;

                List<AttendanceAbsentDto> attendanceByPersonId = new List<AttendanceAbsentDto>();


                absentData.ForEach(x =>
                {
                    attendanceByPersonId.Add(new AttendanceAbsentDto
                    {
                        AttendanceID = x.AttendanceID,
                        STAFF_ID = x.STAFF_ID,
                        DATE = x.DATE,
                        REASON = x.REASON,
                        PAID = x.PAID,
                        CHARGED = x.CHARGED,
                        FRACTION = x.FRACTION,
                        SITENUM = x.SITENUM,
                        SITENAM = x.SITENAM,
                        ReasonID = x.ReasonID,
                        Paycode = x.Paycode
                    });
                });

                // Group timesheets by PersonID
                var groupedByPerson = timesheets.GroupBy(t => (long)t.PersonID);

                // Structure data as required using asynchronous operations
                var personList = await Task.WhenAll(groupedByPerson.Select(async personGroup =>
                {
                    var personId = personGroup.Key;
                    var firstPerson = personGroup.FirstOrDefault();

                    var personSchedules = schedulesByPersonId?.Where(s => s.PersonID == personId).ToList();
                    var personAttendance = attendanceByPersonId?
                        .Where(a => a.STAFF_ID == personId && a.DATE >= startDate && a.DATE <= endDate)
                        .ToList();

                    // Skip iteration if schedules or attendance is not present
                    if ((personSchedules == null || !personSchedules.Any()) &&
                        (personAttendance == null || !personAttendance.Any()))
                    {
                        return null; // Skip this personGroup
                    }

                    var schedulesWithTimesheets = personSchedules.Select(schedule =>
                    {
                        if (schedule == null) return null;

                        var scheduleTimesheets = personGroup
                            .Where(t => t?.PersonID?.ToString() == schedule?.PersonID.ToString()
                                     && t?.SiteID?.ToString() == schedule?.SiteID.ToString())
                            .ToList();

                        var enrichedSchedule = new ExpandoObject() as IDictionary<string, object>;
                        foreach (var property in schedule.GetType().GetProperties())
                        {
                            enrichedSchedule[property.Name] = property.GetValue(schedule) ?? "N/A";
                        }
                        enrichedSchedule["Timesheets"] = scheduleTimesheets;

                        return enrichedSchedule;
                    }).ToList();

                    var personData = new ExpandoObject() as IDictionary<string, object>;
                    personData["PersonID"] = personId;
                    personData["FIRSTNAME"] = firstPerson?.FIRSTNAME ?? "";
                    personData["LASTNAME"] = firstPerson?.LASTNAME ?? "";
                    personData["Schedules"] = schedulesWithTimesheets;
                    personData["Attendance"] = personAttendance;

                    return personData;
                }));

                return new GenericResponse<IEnumerable<dynamic>>(true, "Data retrieved successfully.", personList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data.");
                return new GenericResponse<IEnumerable<dynamic>>(false, "An error occurred while retrieving data.", null);
            }
        }



        public async Task<GenericResponse<List<ScheduleDto>>> GetScheduleTimesheetsByPersonId(long personId, DateTime date)
        {
            try
            {
                // Retrieve all schedules for the person
                var schedules = await _scheduleRepository.GetScheduleByPersonIdAsync(personId);

                // Get the day of the week name for the selected date (e.g., "Tuesday") and capitalize the first letter
                var selectedDayName = char.ToUpper(date.DayOfWeek.ToString()[0]) + date.DayOfWeek.ToString().Substring(1);

                // Construct dynamic properties based on day name (e.g., "mondayTimeIn", "mondayTimeOut")
                var timeInProperty = $"{selectedDayName}TimeIn";
                var timeOutProperty = $"{selectedDayName}TimeOut";

                // Filter out schedules with DeletedDate, and where TimeIn or TimeOut is null for the selected day
                var validSchedules = schedules
                    .Where(s => !s.DeletedDate.HasValue &&
                                s.GetType().GetProperty(timeInProperty)?.GetValue(s) != null &&
                                s.GetType().GetProperty(timeOutProperty)?.GetValue(s) != null)
                    .ToList();

                // Remove valid records invalidated by DeletedDate and DeletedSiteType matches
                var invalidSchedules = schedules
                    .Where(s => s.DeletedDate.HasValue && s.DeletedDate == date && !string.IsNullOrEmpty(s.DeletedSiteType))
                    .ToList();

                // Remove invalid schedules from validSchedules based on Id
                //validSchedules = validSchedules.Where(valid =>
                //    !invalidSchedules.Any(invalid => invalid.Id == valid.Id)).ToList();

                // Apply additional filtering based on the provided `date`
                // Remove invalid records and those with matching SiteType where Date and UpdatedDate are null or empty
                validSchedules = validSchedules.Where(valid =>
                {
                    // Check if there's a matching invalid record with the same SiteType
                    var hasMatchingInvalid = invalidSchedules.Any(invalid =>
                        invalid.DeletedSiteType == valid.SiteType &&
                        (string.IsNullOrEmpty(valid.Date?.ToString()) && string.IsNullOrEmpty(valid.UpdatedDate?.ToString())));

                    // Exclude valid records if they match the above criteria
                    return !hasMatchingInvalid;
                }).ToList();

                // Apply additional filtering based on the provided `date`
                validSchedules = validSchedules.Where(valid =>
                    valid.Date == date ||
                    valid.UpdatedDate == date ||
                    (valid.StartDate <= date && valid.EndDate >= date && valid.Date == null && valid.UpdatedDate == null)
                ).ToList();

                // Apply additional filtering based on the provided `date`
                //validSchedules = validSchedules.Where(valid =>
                //    (
                //        valid.Date == date ||
                //        valid.UpdatedDate == date ||
                //        (valid.StartDate <= date && valid.EndDate >= date && valid.Date == null && valid.UpdatedDate == null)
                //    ) &&
                //    !invalidSchedules.Any(invalid =>
                //        (invalid.DeletedDate == valid.Date ||
                //         invalid.DeletedDate == valid.UpdatedDate ||
                //         (invalid.DeletedDate >= valid.StartDate && invalid.DeletedDate <= valid.EndDate)) &&
                //         invalid.DeletedSiteType == valid.SiteType
                //    )
                //).ToList();

                // Map the remaining valid schedules to DTOs
                var schedulesDtos = _mapper.Map<List<ScheduleDto>>(validSchedules);

                return new GenericResponse<List<ScheduleDto>>(true, "Schedule retrieved successfully.", schedulesDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving timesheets.");
                return new GenericResponse<List<ScheduleDto>>(false, "An error occurred while retrieving schedule.", null);
            }
        }


        public async Task<GenericResponse<bool>> UpdateSiteByTimesheetDateAsync()
        {
            try
            {
                var existingSites = await _siteRepository.GetAllSitesAsync();

                var sites = new List<Site>();
                foreach (var site in existingSites)
                {
                    var addressParts = new List<string> { site.ADDR1, site.ADDR2, site.ADDR3 }
                                       .Where(part => !string.IsNullOrWhiteSpace(part))
                                       .ToList();

                    var fullAddress = string.Join(", ", addressParts);
                    var (latitude, longitude) = await _geocodingServiceHelper.GetLatLongFromAddressAsync(fullAddress);

                    site.Latitude = latitude;
                    site.Longitude = longitude;

                    sites.Add(site);
                }

                await _siteRepository.SaveChangesAsync();

                return new GenericResponse<bool>(true, "Timesheets and schools updated successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating schools by timesheet date.");
                return new GenericResponse<bool>(false, "An error occurred while updating schools by timesheet date.", false);
            }
        }


        public async Task<GenericResponse<List<SiteTimesheetDto>>> GetSiteByTimesheetDate(DateTime date)
        {
            try
            {
                // Fetch timesheets for the specified date
                var timesheets = await _timesheetRepository.GetTimesheetByDateAsync(date);
                if (!timesheets.Any())
                    return new GenericResponse<List<SiteTimesheetDto>>(true, "No timesheets found for the specified date.", new List<SiteTimesheetDto>());

                // Extract SiteIDs and PersonnelIDs from timesheets
                var siteIds = timesheets.Select(t => t.SiteID).Distinct().ToList();
                var personnelIds = timesheets.Select(t => t.PersonID).Distinct().ToList();

                // Fetch all required sites and personnel in one go
                var sites = await _siteRepository.GetAllSitesByIdAsync(siteIds);
                var personnelList = await _personelRepository.GetPersonelsByIdsAsync(personnelIds);

                // Convert sites and personnel to dictionaries for efficient lookup
                var siteDictionary = sites.ToDictionary(s => s.SiteID);
                var personnelDictionary = personnelList.ToDictionary(p => p.PersonalID); // Use PersonalID as the key

                // Initialize the result list
                var siteTimesheetDtos = new List<SiteTimesheetDto>();

                foreach (var timesheet in timesheets)
                {
                    // Lookup Site and Personnel for the current timesheet
                    var site = siteDictionary.GetValueOrDefault((long)timesheet.SiteID);
                    var personnel = personnelDictionary.GetValueOrDefault((long)timesheet.PersonID); // Match PersonID to PersonalID dictionary

                    if (site != null && personnel != null)
                    {
                        // Map site and personnel details to DTOs
                        var siteDto = _mapper.Map<SiteDto>(site);
                        var personnelDto = _mapper.Map<PersonelDto>(personnel);

                        // Create a SiteTimesheetDto with embedded timesheet and personnel information
                        var siteTimesheetDto = new SiteTimesheetDto
                        {
                            Site = siteDto,
                            Timesheet = _mapper.Map<TimesheetDto>(timesheet),
                            Personel = personnelDto
                        };

                        siteTimesheetDtos.Add(siteTimesheetDto);
                    }
                }

                return new GenericResponse<List<SiteTimesheetDto>>(true, "Sites and personnel details fetched successfully.", siteTimesheetDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching site and personnel information by timesheet date.");
                return new GenericResponse<List<SiteTimesheetDto>>(false, "An error occurred while fetching site and personnel information.", null);
            }
        }

        public async Task<GenericResponse<TimesheetDto>> UpdateExternalTimesheet(long id, TimesheetDto timesheetDto)
        {
            try
            {
                var timesheet = _mapper.Map<Timesheet>(timesheetDto);
                timesheet.ExternalEventId = id;

                var existingTimesheet = await _timesheetRepository.GetTimesheetByExternalIdAsync(id);
                if (existingTimesheet == null)
                {
                    _logger.LogWarning("Timesheet with ExternalEventID {TimesheetId} not found or update failed.", id);
                    return new GenericResponse<TimesheetDto>(false, "Timesheet not found or update failed.", null);
                }
                timesheet.TimesheetID = existingTimesheet.TimesheetID;

                if (timesheet.ExternalEventId != null)
                {
                    var existingDistrict = await _districtRepository.GetDistrictByDistNumberAsync((long)timesheetDto.DistrictID);
                    timesheet.DistrictID = existingDistrict.DistrictID;
                }

                var updatedTimesheet = await _timesheetRepository.UpdateTimesheetAsync(timesheet);
                if (updatedTimesheet == null)
                {
                    _logger.LogWarning("Timesheet with ID {TimesheetId} not found or update failed.", id);
                    return new GenericResponse<TimesheetDto>(false, "Timesheet not found or update failed.", null);
                }

                var updatedTimesheetDto = _mapper.Map<TimesheetDto>(updatedTimesheet);
                return new GenericResponse<TimesheetDto>(true, "Timesheet updated successfully.", updatedTimesheetDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating timesheet with ID {TimesheetId}.", id);
                return new GenericResponse<TimesheetDto>(false, ex.Message, null);
            }
        }
    }
}
