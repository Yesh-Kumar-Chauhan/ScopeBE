using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Infrastructure.Data;
using DocumentFormat.OpenXml.InkML;
using Core.DTOs.Personel;
using Core.Entities.Brainyclock;
using Newtonsoft.Json.Linq;
using Core.DTOs;
using Azure;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;

namespace Application.Services.Personal
{
    public class PersonelService : IPersonelService
    {
        private readonly IPersonelRepository _personelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonelService> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly RestClientService _restClientService;

        public PersonelService(
            AppDbContext context,
            IPersonelRepository personelRepository, IMapper mapper,
            ILogger<PersonelService> logger,
            IConfiguration configuration,
            RestClientService restClientService)
        {
            _context = context;
            _personelRepository = personelRepository;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _restClientService = restClientService;
        }

        public async Task<GenericResponse<IEnumerable<PersonelDto>>> GetFilteredPersonelAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _personelRepository.GetAllPersonel();

                // Apply filtering based on the search query
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchTrimmed = search.Trim().ToLower();
                    query = query.Where(p =>
                        p.PersonalID.ToString().Contains(searchTrimmed) ||
                        p.FIRSTNAME.Trim().ToLower().Contains(searchTrimmed) ||
                        p.LASTNAME.Trim().ToLower().Contains(searchTrimmed) ||
                        (p.FIRSTNAME.Trim().ToLower() + " " + p.LASTNAME.Trim().ToLower()).Contains(searchTrimmed)); // For full name search
                }

                // Get the total count of items (before pagination)
                var totalItems = await query.CountAsync();

                // Apply pagination
                var personels = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var personelDtos = _mapper.Map<IEnumerable<PersonelDto>>(personels);
                return new GenericResponse<IEnumerable<PersonelDto>>(true, "Personnel retrieved successfully.", personelDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all personnel.");
                return new GenericResponse<IEnumerable<PersonelDto>>(false, "An error occurred while retrieving personnel.", null);
            }
        }

        public async Task<GenericResponse<IEnumerable<PersonelDto>>> GetAllPersonelAsync()
        {
            try
            {
                // Retrieve all personnel from the repository asynchronously
                var personels = await _personelRepository.GetAllPersonelAsync();

                // Map the personnel entities to DTOs
                var personelDtos = _mapper.Map<IEnumerable<PersonelDto>>(personels);

                int totalItems = personelDtos.Count();
                // Return a successful response with the list of DTOs
                return new GenericResponse<IEnumerable<PersonelDto>>(true, "All personnel retrieved successfully.", personelDtos, totalItems);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                _logger.LogError(ex, "An error occurred while retrieving all personnel.");
                return new GenericResponse<IEnumerable<PersonelDto>>(false, "An error occurred while retrieving personnel.", null);
            }
        }


        public async Task<GenericResponse<PersonelDto>> GetPersonelByIdAsync(long id)
        {
            try
            {
                var personel = await _personelRepository.GetPersonelByIdAsync(id);
                if (personel == null)
                {
                    _logger.LogWarning("Personel with ID {PersonelId} not found.", id);
                    return new GenericResponse<PersonelDto>(false, "Personel not found.", null);
                }

                var personelDto = _mapper.Map<PersonelDto>(personel);
                return new GenericResponse<PersonelDto>(true, "Personel retrieved successfully.", personelDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving personel with ID {PersonelId}.", id);
                return new GenericResponse<PersonelDto>(false, "An error occurred while retrieving the personel.", null);
            }
        }
        public async Task<GenericResponse<PersonelDto>> GetPersonelByEmailAsync(string email)
        {
            try
            {
                var personel = await _personelRepository.GetPersonelByEmailAsync(email);
                if (personel == null)
                {
                    _logger.LogWarning("Personel with Email {email} not found.", email);
                    return new GenericResponse<PersonelDto>(true, "Personel not found.", null);
                }

                var personelDto = _mapper.Map<PersonelDto>(personel);
                return new GenericResponse<PersonelDto>(true, "Personel retrieved successfully.", personelDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving personel with email {email}.", email);
                return new GenericResponse<PersonelDto>(false, "An error occurred while retrieving the personel.", null);
            }
        }

        public async Task<GenericResponse<PersonelDto>> CreatePersonelAsync(PersonelDto personelDto)
        {
            using var transaction = await _personelRepository.BeginTransactionAsync(); // Start a transaction

            try
            {
                // Map PersonelDto to Personel entity and create it in the database
                var personel = _mapper.Map<Personel>(personelDto);
                var createdPersonel = await _personelRepository.AddPersonelAsync(personel);

                // Prepare Employee object to send to the external API
                var employee = new Employee
                {
                    CompanyId = 414,
                    DepartmentId = null,
                    CreatedBy = null,
                    FName = $"{createdPersonel.FIRSTNAME} {createdPersonel.LASTNAME}",
                    FirstName = createdPersonel.FIRSTNAME,
                    LastName = createdPersonel.LASTNAME,
                    Email = createdPersonel.EMAIL,
                    Password = "Test@123",
                    ShiftId1 = null,
                    ShiftId2 = null,
                    ShiftId3 = null,
                    LocationId = null,
                    OverTime = null,
                    HourlyRate = null,
                    Type = 5,
                    EmployeeId = createdPersonel.PersonalID.ToString(),
                };

                // Make the API call to post the Employee data
                var response = await _restClientService.PostAsync<Employee>("Employee/sync-employee", employee);

                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Successfully synced employee with the external API.");

                    // Commit transaction if API call is successful
                    await transaction.CommitAsync();

                    // Map created Personel entity back to DTO for response
                    var createdPersonelDto = _mapper.Map<PersonelDto>(createdPersonel);
                    return new GenericResponse<PersonelDto>(true, "Personel created successfully and synced with external API.", createdPersonelDto);
                }
                else
                {
                    string errorMessage = "Failed to sync employee with external API.";
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

                    _logger.LogError("Error from external API: {ErrorMessage}", errorMessage);
                    await transaction.RollbackAsync(); // Rollback transaction if API call fails
                    return new GenericResponse<PersonelDto>(false, errorMessage, null);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback on exception
                _logger.LogError(ex, "An error occurred while creating a new personel.");
                return new GenericResponse<PersonelDto>(false, "An error occurred while creating the personel.", null);
            }
        }


        public async Task<GenericResponse<PersonelDto>> UpdatePersonelAsync(long id, PersonelDto personelDto)
        {
            using var transaction = await _personelRepository.BeginTransactionAsync();

            try
            {

                var personel = _mapper.Map<Personel>(personelDto);
                personel.PersonalID = id;

                var updatedPersonel = await _personelRepository.UpdatePersonelAsync(personel);
                if (updatedPersonel == null)
                {
                    _logger.LogWarning("Personel with ID {PersonelId} not found or update failed.", id);
                    return new GenericResponse<PersonelDto>(false, "Personel not found or update failed.", null);
                }

                var updatedPersonelDto = _mapper.Map<PersonelDto>(updatedPersonel);

                // Prepare Employee object to send to the external API
                var employee = new Employee
                {
                    CompanyId = 414,
                    DepartmentId = null,
                    CreatedBy = null,
                    FName = $"{updatedPersonelDto.FIRSTNAME} {updatedPersonelDto.LASTNAME}",
                    FirstName = updatedPersonelDto.FIRSTNAME,
                    LastName = updatedPersonelDto.LASTNAME,
                    Email = updatedPersonelDto.EMAIL,
                    Password = "Test@123",
                    ShiftId1 = null,
                    ShiftId2 = null,
                    ShiftId3 = null,
                    LocationId = null,
                    OverTime = null,
                    HourlyRate = null,
                    Type = 5,
                    EmployeeId = updatedPersonelDto.PersonalID.ToString(),
                };

                // Make the API call to post the Employee data
                var response = await _restClientService.PostAsync<Employee>("Employee/sync-employee", employee);

                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Successfully synced employee with the external API.");

                    // Commit transaction if API call is successful
                    await transaction.CommitAsync();

                    // Map created Personel entity back to DTO for response
                    var createdPersonelDto = _mapper.Map<PersonelDto>(updatedPersonel);
                    return new GenericResponse<PersonelDto>(true, "Personel updated successfully and synced with external API.", createdPersonelDto);
                }
                else
                {
                    string errorMessage = "Failed to sync employee with external API.";
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

                    _logger.LogError("Error from external API: {ErrorMessage}", errorMessage);
                    await transaction.RollbackAsync(); // Rollback transaction if API call fails
                    return new GenericResponse<PersonelDto>(false, errorMessage, null);
                }
                //return new GenericResponse<PersonelDto>(true, "Personel updated successfully.", updatedPersonelDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating personel with ID {PersonelId}.", id);
                return new GenericResponse<PersonelDto>(false, "An error occurred while updating the personel.", null);
            }
        }

        public async Task<GenericResponse<bool>> DeletePersonelAsync(long id)
        {
            try
            {
                var deleted = await _personelRepository.DeletePersonelAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Personel with ID {PersonelId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "Personel not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "Personel deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting personel with ID {PersonelId}.", id);
                return new GenericResponse<bool>(false, "An error occurred while deleting the personel.", false);
            }
        }

        public async Task<GenericResponse<List<ExtendedPersonelDto>>> GetPersonelByKeywordAndOperationAsync(string keyword, int operation, int? page = null, int? pageSize = null)
        {
            try
            {
                // Fetch personel list based on keyword and operation
                var personels = await _personelRepository.GetPersonelByKeywordAndOperationAsync(keyword, operation);

                // Apply pagination if page and pageSize are provided
                if (page.HasValue && pageSize.HasValue)
                {
                    personels = personels
                        .Skip((page.Value - 1) * pageSize.Value)
                        .Take(pageSize.Value)
                        .ToList();
                }

                return new GenericResponse<List<ExtendedPersonelDto>>(true, "Personel data retrieved successfully.", personels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving personel data.");
                return new GenericResponse<List<ExtendedPersonelDto>>(false, ex.Message, null);
            }
        }

        //public async Task<GenericResponse<byte[]>> GetPersonelScheduleExcel()
        ////public async Task<GenericResponse<List<ScheduleDto>>> GetPersonelScheduleExcel()
        //{
        //    try
        //    {
        //        // Fetch data from the database
        //        var personelList = await _context.Personel.ToListAsync();
        //        var sites = await _context.Sites.ToListAsync();
        //        var districts = await _context.Districts.ToListAsync();

        //        var schedules = new List<ScheduleDto>();

        //        foreach (var person in personelList)
        //        {
        //            var weeklyData = new List<(string Day, string? TimeIn_B, string? TimeOut_B, string? TimeIn_D, string? TimeOut_D, string? TimeIn_A, string? TimeOut_A)>
        //            {
        //                ("Monday", person.MON_1_B, person.MON_1_E, person.MON_2_B, person.MON_2_E, person.MON_3_B, person.MON_3_E),
        //                ("Tuesday", person.TUE_1_B, person.TUE_1_E, person.TUE_2_B, person.TUE_2_E, person.TUE_3_B, person.TUE_3_E),
        //                ("Wednesday", person.WED_1_B, person.WED_1_E, person.WED_2_B, person.WED_2_E, person.WED_3_B, person.WED_3_E),
        //                ("Thursday", person.THU_1_B, person.THU_1_E, person.THU_2_B, person.THU_2_E, person.THU_3_B, person.THU_3_E),
        //                ("Friday", person.FRI_1_B, person.FRI_1_E, person.FRI_2_B, person.FRI_2_E, person.FRI_3_B, person.FRI_3_E)
        //            };

        //            foreach (var (day, timeIn_B, timeOut_B, timeIn_D, timeOut_D, timeIn_A, timeOut_A) in weeklyData)
        //            {
        //                var siteMappings = new List<(string? TimeIn, string? TimeOut, string? SiteNum, string SiteType, string SitePosition)>
        //                {
        //                    (timeIn_B, timeOut_B, person.SITE_NUM_B?.ToString(), "Before", person.SITE_POS_B),
        //                    (timeIn_D, timeOut_D, person.SITE_NUM_D?.ToString(), "During", person.SITE_POS_D),
        //                    (timeIn_A, timeOut_A, person.SITE_NUM_A?.ToString(), "After", person.SITE_POS_A)
        //                };

        //                foreach (var (timeIn, timeOut, siteNum, siteType, sitePosition) in siteMappings.Where(s => !string.IsNullOrEmpty(s.TimeIn) && !string.IsNullOrEmpty(s.TimeOut) && !string.IsNullOrEmpty(s.SiteNum)))
        //                {
        //                    // Extract district and site
        //                    string districtNum = string.Empty;
        //                    if (!string.IsNullOrEmpty(siteNum) && siteNum.Length >= 3)
        //                    {
        //                        districtNum = siteNum.Substring(0, 3);
        //                    }
        //                    else
        //                    {
        //                        continue;
        //                    }
        //                    var district = districts.FirstOrDefault(d => d.DIST_NUM.ToString() == districtNum);
        //                    var site = sites.FirstOrDefault(s => s.SITE_NUM.ToString() == siteNum);

        //                    // Check if a schedule already exists for this PersonID, SiteType, and Date
        //                    var existingSchedule = schedules.FirstOrDefault(s => s.PersonID == person.PersonalID && s.SiteType == siteType);

        //                    if (existingSchedule != null)
        //                    {
        //                        // Update the existing schedule
        //                        switch (day)
        //                        {
        //                            case "Monday":
        //                                existingSchedule.MondayTimeIn = TimeSpan.TryParse(timeIn, out var mondayIn) ? mondayIn : existingSchedule.MondayTimeIn;
        //                                existingSchedule.MondayTimeOut = TimeSpan.TryParse(timeOut, out var mondayOut) ? mondayOut : existingSchedule.MondayTimeOut;
        //                                break;
        //                            case "Tuesday":
        //                                existingSchedule.TuesdayTimeIn = TimeSpan.TryParse(timeIn, out var tuesdayIn) ? tuesdayIn : existingSchedule.TuesdayTimeIn;
        //                                existingSchedule.TuesdayTimeOut = TimeSpan.TryParse(timeOut, out var tuesdayOut) ? tuesdayOut : existingSchedule.TuesdayTimeOut;
        //                                break;
        //                            case "Wednesday":
        //                                existingSchedule.WednesdayTimeIn = TimeSpan.TryParse(timeIn, out var wednesdayIn) ? wednesdayIn : existingSchedule.WednesdayTimeIn;
        //                                existingSchedule.WednesdayTimeOut = TimeSpan.TryParse(timeOut, out var wednesdayOut) ? wednesdayOut : existingSchedule.WednesdayTimeOut;
        //                                break;
        //                            case "Thursday":
        //                                existingSchedule.ThursdayTimeIn = TimeSpan.TryParse(timeIn, out var thursdayIn) ? thursdayIn : existingSchedule.ThursdayTimeIn;
        //                                existingSchedule.ThursdayTimeOut = TimeSpan.TryParse(timeOut, out var thursdayOut) ? thursdayOut : existingSchedule.ThursdayTimeOut;
        //                                break;
        //                            case "Friday":
        //                                existingSchedule.FridayTimeIn = TimeSpan.TryParse(timeIn, out var fridayIn) ? fridayIn : existingSchedule.FridayTimeIn;
        //                                existingSchedule.FridayTimeOut = TimeSpan.TryParse(timeOut, out var fridayOut) ? fridayOut : existingSchedule.FridayTimeOut;
        //                                break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Create a new schedule
        //                        var schedule = new ScheduleDto
        //                        {
        //                            PersonID = person.PersonalID,
        //                            Date = null,
        //                            StartDate = DateTime.Now,
        //                            EndDate = DateTime.Now.AddYears(5),
        //                            SiteID = site?.SiteID,
        //                            Position = sitePosition,
        //                            SiteName = site?.SITE_NAM,
        //                            DistNumber = district?.DIST_NUM,
        //                            DistName = district?.DIST_NAM,
        //                            SiteType = siteType,
        //                            Notes = "",
        //                            //Notes = $"Scheduled for {day} ({siteType})",
        //                            Paycode = null,
        //                            UpdatedDate = null,
        //                            DeletedDate = null,
        //                            DeletedSiteType = null,
        //                        };

        //                        // Assign timeIn and timeOut to the corresponding weekday
        //                        switch (day)
        //                        {
        //                            case "Monday":
        //                                schedule.MondayTimeIn = TimeSpan.TryParse(timeIn, out var mondayIn) ? mondayIn : null;
        //                                schedule.MondayTimeOut = TimeSpan.TryParse(timeOut, out var mondayOut) ? mondayOut : null;
        //                                break;
        //                            case "Tuesday":
        //                                schedule.TuesdayTimeIn = TimeSpan.TryParse(timeIn, out var tuesdayIn) ? tuesdayIn : null;
        //                                schedule.TuesdayTimeOut = TimeSpan.TryParse(timeOut, out var tuesdayOut) ? tuesdayOut : null;
        //                                break;
        //                            case "Wednesday":
        //                                schedule.WednesdayTimeIn = TimeSpan.TryParse(timeIn, out var wednesdayIn) ? wednesdayIn : null;
        //                                schedule.WednesdayTimeOut = TimeSpan.TryParse(timeOut, out var wednesdayOut) ? wednesdayOut : null;
        //                                break;
        //                            case "Thursday":
        //                                schedule.ThursdayTimeIn = TimeSpan.TryParse(timeIn, out var thursdayIn) ? thursdayIn : null;
        //                                schedule.ThursdayTimeOut = TimeSpan.TryParse(timeOut, out var thursdayOut) ? thursdayOut : null;
        //                                break;
        //                            case "Friday":
        //                                schedule.FridayTimeIn = TimeSpan.TryParse(timeIn, out var fridayIn) ? fridayIn : null;
        //                                schedule.FridayTimeOut = TimeSpan.TryParse(timeOut, out var fridayOut) ? fridayOut : null;
        //                                break;
        //                        }

        //                        schedules.Add(schedule);
        //                    }
        //                }

        //            }
        //        }

        //        // Set License Context
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //        // Generate Excel File
        //        using var package = new ExcelPackage();
        //        var worksheet = package.Workbook.Worksheets.Add("Schedules");

        //        // Add Headers
        //        var headers = new[]
        //        {
        //            "PersonID", "Date", "StartDate", "EndDate", "SiteID", "Position", "SiteName",
        //            "DistNumber", "DistName", "SiteType", "Notes", "MondayTimeIn", "MondayTimeOut",
        //            "TuesdayTimeIn", "TuesdayTimeOut", "WednesdayTimeIn", "WednesdayTimeOut",
        //            "ThursdayTimeIn", "ThursdayTimeOut", "FridayTimeIn", "FridayTimeOut"
        //        };

        //        for (int i = 0; i < headers.Length; i++)
        //        {
        //            worksheet.Cells[1, i + 1].Value = headers[i];
        //        }

        //        // Populate Data
        //        for (int i = 0; i < schedules.Count; i++)
        //        {
        //            var schedule = schedules[i];
        //            worksheet.Cells[i + 2, 1].Value = schedule.PersonID;
        //            worksheet.Cells[i + 2, 2].Value = schedule.Date?.ToShortDateString();
        //            worksheet.Cells[i + 2, 3].Value = schedule.StartDate?.ToShortDateString();
        //            worksheet.Cells[i + 2, 4].Value = schedule.EndDate?.ToShortDateString();
        //            worksheet.Cells[i + 2, 5].Value = schedule.SiteID;
        //            worksheet.Cells[i + 2, 6].Value = schedule.Position;
        //            worksheet.Cells[i + 2, 7].Value = schedule.SiteName;
        //            worksheet.Cells[i + 2, 8].Value = schedule.DistNumber;
        //            worksheet.Cells[i + 2, 9].Value = schedule.DistName;
        //            worksheet.Cells[i + 2, 10].Value = schedule.SiteType;
        //            worksheet.Cells[i + 2, 11].Value = schedule.Notes;
        //            worksheet.Cells[i + 2, 12].Value = schedule.MondayTimeIn?.ToString();
        //            worksheet.Cells[i + 2, 13].Value = schedule.MondayTimeOut?.ToString();
        //            worksheet.Cells[i + 2, 14].Value = schedule.TuesdayTimeIn?.ToString();
        //            worksheet.Cells[i + 2, 15].Value = schedule.TuesdayTimeOut?.ToString();
        //            worksheet.Cells[i + 2, 16].Value = schedule.WednesdayTimeIn?.ToString();
        //            worksheet.Cells[i + 2, 17].Value = schedule.WednesdayTimeOut?.ToString();
        //            worksheet.Cells[i + 2, 18].Value = schedule.ThursdayTimeIn?.ToString();
        //            worksheet.Cells[i + 2, 19].Value = schedule.ThursdayTimeOut?.ToString();
        //            worksheet.Cells[i + 2, 20].Value = schedule.FridayTimeIn?.ToString();
        //            worksheet.Cells[i + 2, 21].Value = schedule.FridayTimeOut?.ToString();
        //        }

        //        // Convert Excel to byte array
        //        var stream = new MemoryStream();
        //        package.SaveAs(stream);
        //        var excelData = stream.ToArray();

        //        return new GenericResponse<byte[]>(true, "Schedule Excel generated successfully.", excelData);
        //        //return new GenericResponse<List<ScheduleDto>>(true, "Personel data retrieved successfully.", schedules);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while retrieving personel data.");
        //        return new GenericResponse<byte[]>(false, ex.Message, null);
        //        //return new GenericResponse<List<ScheduleDto>>(false, ex.Message, null);
        //    }
        //}

        public async Task<GenericResponse<Dictionary<string, byte[]>>> GetPersonelScheduleExcel()
        {
            try
            {
                // Fetch data from the database
                var personelList = await _context.Personel.ToListAsync();
                var sites = await _context.Sites.ToListAsync();
                var districts = await _context.Districts.ToListAsync();

                var schedules = new List<ScheduleDto>();
                var unscheduledPersonnel = new List<Personel>();

                foreach (var person in personelList)
                {
                    bool hasPersonSchedule = false;
                    var weeklyData = new List<(string Day, string? TimeIn_B, string? TimeOut_B, string? TimeIn_D, string? TimeOut_D, string? TimeIn_A, string? TimeOut_A)>
                            {
                                ("Monday", person.MON_1_B, person.MON_1_E, person.MON_2_B, person.MON_2_E, person.MON_3_B, person.MON_3_E),
                                ("Tuesday", person.TUE_1_B, person.TUE_1_E, person.TUE_2_B, person.TUE_2_E, person.TUE_3_B, person.TUE_3_E),
                                ("Wednesday", person.WED_1_B, person.WED_1_E, person.WED_2_B, person.WED_2_E, person.WED_3_B, person.WED_3_E),
                                ("Thursday", person.THU_1_B, person.THU_1_E, person.THU_2_B, person.THU_2_E, person.THU_3_B, person.THU_3_E),
                                ("Friday", person.FRI_1_B, person.FRI_1_E, person.FRI_2_B, person.FRI_2_E, person.FRI_3_B, person.FRI_3_E)
                            };

                    foreach (var (day, timeIn_B, timeOut_B, timeIn_D, timeOut_D, timeIn_A, timeOut_A) in weeklyData)
                    {
                        var siteMappings = new List<(string? TimeIn, string? TimeOut, string? SiteNum, string SiteType, string SitePosition)>
                                {
                                    (timeIn_B, timeOut_B, person.SITE_NUM_B?.ToString(), "Before", person.SITE_POS_B),
                                    (timeIn_D, timeOut_D, person.SITE_NUM_D?.ToString(), "During", person.SITE_POS_D),
                                    (timeIn_A, timeOut_A, person.SITE_NUM_A?.ToString(), "After", person.SITE_POS_A)
                                };

                        foreach (var (timeIn, timeOut, siteNum, siteType, sitePosition) in siteMappings.Where(s => !string.IsNullOrEmpty(s.TimeIn) && !string.IsNullOrEmpty(s.TimeOut) && !string.IsNullOrEmpty(s.SiteNum)))
                        {
                            // Extract district and site
                            string districtNum = string.Empty;
                            if (!string.IsNullOrEmpty(siteNum) && siteNum.Length >= 3)
                            {
                                districtNum = siteNum.Substring(0, 3);
                            }
                            else
                            {
                                continue;
                            }
                            var district = districts.FirstOrDefault(d => d.DIST_NUM.ToString() == districtNum);
                            if(district == null ) 
                            {
                                continue;
                            }
                            var site = sites.FirstOrDefault(s => s.SITE_NUM.ToString() == siteNum);
                            if (site == null)
                            {
                                continue;
                            }

                            hasPersonSchedule = true;

                            // Check if a schedule already exists for this PersonID, SiteType, and Date
                            var existingSchedule = schedules.FirstOrDefault(s => s.PersonID == person.PersonalID && s.SiteType == siteType);

                            if (existingSchedule != null)
                            {
                                // Update the existing schedule
                                switch (day)
                                {
                                    case "Monday":
                                        existingSchedule.MondayTimeIn = TimeSpan.TryParse(timeIn, out var mondayIn) ? mondayIn : existingSchedule.MondayTimeIn;
                                        existingSchedule.MondayTimeOut = TimeSpan.TryParse(timeOut, out var mondayOut) ? mondayOut : existingSchedule.MondayTimeOut;
                                        break;
                                    case "Tuesday":
                                        existingSchedule.TuesdayTimeIn = TimeSpan.TryParse(timeIn, out var tuesdayIn) ? tuesdayIn : existingSchedule.TuesdayTimeIn;
                                        existingSchedule.TuesdayTimeOut = TimeSpan.TryParse(timeOut, out var tuesdayOut) ? tuesdayOut : existingSchedule.TuesdayTimeOut;
                                        break;
                                    case "Wednesday":
                                        existingSchedule.WednesdayTimeIn = TimeSpan.TryParse(timeIn, out var wednesdayIn) ? wednesdayIn : existingSchedule.WednesdayTimeIn;
                                        existingSchedule.WednesdayTimeOut = TimeSpan.TryParse(timeOut, out var wednesdayOut) ? wednesdayOut : existingSchedule.WednesdayTimeOut;
                                        break;
                                    case "Thursday":
                                        existingSchedule.ThursdayTimeIn = TimeSpan.TryParse(timeIn, out var thursdayIn) ? thursdayIn : existingSchedule.ThursdayTimeIn;
                                        existingSchedule.ThursdayTimeOut = TimeSpan.TryParse(timeOut, out var thursdayOut) ? thursdayOut : existingSchedule.ThursdayTimeOut;
                                        break;
                                    case "Friday":
                                        existingSchedule.FridayTimeIn = TimeSpan.TryParse(timeIn, out var fridayIn) ? fridayIn : existingSchedule.FridayTimeIn;
                                        existingSchedule.FridayTimeOut = TimeSpan.TryParse(timeOut, out var fridayOut) ? fridayOut : existingSchedule.FridayTimeOut;
                                        break;
                                }
                            }
                            else
                            {
                                // Create a new schedule
                                var schedule = new ScheduleDto
                                {
                                    PersonID = person.PersonalID,
                                    Date = null,
                                    StartDate = DateTime.Now,
                                    EndDate = DateTime.Now.AddYears(5),
                                    SiteID = site?.SiteID,
                                    Position = sitePosition,
                                    SiteName = site?.SITE_NAM,
                                    DistNumber = district?.DIST_NUM,
                                    DistName = district?.DIST_NAM,
                                    SiteType = siteType,
                                    Notes = "",
                                    //Notes = $"Scheduled for {day} ({siteType})",
                                    Paycode = null,
                                    UpdatedDate = null,
                                    DeletedDate = null,
                                    DeletedSiteType = null,
                                };

                                // Assign timeIn and timeOut to the corresponding weekday
                                switch (day)
                                {
                                    case "Monday":
                                        schedule.MondayTimeIn = TimeSpan.TryParse(timeIn, out var mondayIn) ? mondayIn : null;
                                        schedule.MondayTimeOut = TimeSpan.TryParse(timeOut, out var mondayOut) ? mondayOut : null;
                                        break;
                                    case "Tuesday":
                                        schedule.TuesdayTimeIn = TimeSpan.TryParse(timeIn, out var tuesdayIn) ? tuesdayIn : null;
                                        schedule.TuesdayTimeOut = TimeSpan.TryParse(timeOut, out var tuesdayOut) ? tuesdayOut : null;
                                        break;
                                    case "Wednesday":
                                        schedule.WednesdayTimeIn = TimeSpan.TryParse(timeIn, out var wednesdayIn) ? wednesdayIn : null;
                                        schedule.WednesdayTimeOut = TimeSpan.TryParse(timeOut, out var wednesdayOut) ? wednesdayOut : null;
                                        break;
                                    case "Thursday":
                                        schedule.ThursdayTimeIn = TimeSpan.TryParse(timeIn, out var thursdayIn) ? thursdayIn : null;
                                        schedule.ThursdayTimeOut = TimeSpan.TryParse(timeOut, out var thursdayOut) ? thursdayOut : null;
                                        break;
                                    case "Friday":
                                        schedule.FridayTimeIn = TimeSpan.TryParse(timeIn, out var fridayIn) ? fridayIn : null;
                                        schedule.FridayTimeOut = TimeSpan.TryParse(timeOut, out var fridayOut) ? fridayOut : null;
                                        break;
                                }

                                schedules.Add(schedule);
                            }
                        }

                    }

                    if (!hasPersonSchedule)
                    {
                        unscheduledPersonnel.Add(person);
                    }
                }

                // Generate the Excel files
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // 1. Excel for scheduled personnel
                var scheduledExcel = GenerateExcel(schedules);

                // 2. Excel for unscheduled personnel
                var unscheduledExcel = GenerateUnscheduledExcel(unscheduledPersonnel);

                // Combine the two files in a response
                var result = new Dictionary<string, byte[]>
                {
                     { "Scheduled.xlsx", scheduledExcel },
                     { "Unscheduled.xlsx", unscheduledExcel }
                };

                return new GenericResponse<Dictionary<string, byte[]>>(true, "Excel files generated successfully.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating Excel files.");
                return new GenericResponse<Dictionary<string, byte[]>>(false, ex.Message, null);
            }
        }


        private byte[] GenerateExcel(List<ScheduleDto> schedules)
        {

            // Generate Excel File
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Schedules");

            // Add Headers
            var headers = new[]
                    {
                        "PersonID", "StartDate", "EndDate", "SiteID", "Position", "SiteName",
                        "DistNumber", "DistName", "SiteType", "Notes", "MondayTimeIn", "MondayTimeOut",
                        "TuesdayTimeIn", "TuesdayTimeOut", "WednesdayTimeIn", "WednesdayTimeOut",
                        "ThursdayTimeIn", "ThursdayTimeOut", "FridayTimeIn", "FridayTimeOut"
                    };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            // Populate Data
            for (int i = 0; i < schedules.Count; i++)
            {
                var schedule = schedules[i];
                worksheet.Cells[i + 2, 1].Value = schedule.PersonID;
                worksheet.Cells[i + 2, 2].Value = schedule.StartDate?.ToShortDateString();
                worksheet.Cells[i + 2, 3].Value = schedule.EndDate?.ToShortDateString();
                worksheet.Cells[i + 2, 4].Value = schedule.SiteID;
                worksheet.Cells[i + 2, 5].Value = schedule.Position;
                worksheet.Cells[i + 2, 6].Value = schedule.SiteName;
                worksheet.Cells[i + 2, 7].Value = schedule.DistNumber;
                worksheet.Cells[i + 2, 8].Value = schedule.DistName;
                worksheet.Cells[i + 2, 9].Value = schedule.SiteType;
                worksheet.Cells[i + 2, 10].Value = schedule.Notes;
                worksheet.Cells[i + 2, 11].Value = schedule.MondayTimeIn?.ToString();
                worksheet.Cells[i + 2, 12].Value = schedule.MondayTimeOut?.ToString();
                worksheet.Cells[i + 2, 13].Value = schedule.TuesdayTimeIn?.ToString();
                worksheet.Cells[i + 2, 14].Value = schedule.TuesdayTimeOut?.ToString();
                worksheet.Cells[i + 2, 15].Value = schedule.WednesdayTimeIn?.ToString();
                worksheet.Cells[i + 2, 16].Value = schedule.WednesdayTimeOut?.ToString();
                worksheet.Cells[i + 2, 17].Value = schedule.ThursdayTimeIn?.ToString();
                worksheet.Cells[i + 2, 18].Value = schedule.ThursdayTimeOut?.ToString();
                worksheet.Cells[i + 2, 19].Value = schedule.FridayTimeIn?.ToString();
                worksheet.Cells[i + 2, 20].Value = schedule.FridayTimeOut?.ToString();
            }

            // Convert Excel to byte array
            var stream = new MemoryStream();
            package.SaveAs(stream);
            return stream.ToArray();
        }

        // Helper to generate unscheduled Excel
        private byte[] GenerateUnscheduledExcel(List<Personel> unscheduledPersonnel)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Unscheduled");

            // Add headers
            var headers = new[] { "PersonID", "FirstName", "LastName", "SSN", "DOB" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            // Populate data
            for (int i = 0; i < unscheduledPersonnel.Count; i++)
            {
                var person = unscheduledPersonnel[i];
                worksheet.Cells[i + 2, 1].Value = person.PersonalID;
                worksheet.Cells[i + 2, 2].Value = person.FIRSTNAME;
                worksheet.Cells[i + 2, 3].Value = person.LASTNAME;
                worksheet.Cells[i + 2, 4].Value = person.SSN;
                worksheet.Cells[i + 2, 5].Value = person.DOB?.ToShortDateString();
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            return stream.ToArray();
        }


        // Helper to determine the date for the given day
        private DateTime DetermineDate(string day)
        {
            var currentDate = DateTime.Now.Date;
            var daysOfWeek = new Dictionary<string, DayOfWeek>
            {
                { "Monday", DayOfWeek.Monday },
                { "Tuesday", DayOfWeek.Tuesday },
                { "Wednesday", DayOfWeek.Wednesday },
                { "Thursday", DayOfWeek.Thursday },
                { "Friday", DayOfWeek.Friday }
             };

            return currentDate.AddDays((int)daysOfWeek[day] - (int)currentDate.DayOfWeek);
        }

    }
}
