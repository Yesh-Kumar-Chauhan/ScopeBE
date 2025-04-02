using Application.Services.Personal;
using Core.Entities;
using Core.Entities.Brainyclock;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services.Brainyclock;
using Core.Modals;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Brainyclock
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IPersonelRepository _personelRepository;
        private readonly RestClientService _restClientService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            IPersonelRepository personelRepository,
            RestClientService restClientService,
            ILogger<EmployeeService> logger)
        {
            _personelRepository = personelRepository;
            _restClientService = restClientService;
            _logger = logger;
        }

        public async Task<List<Employee>> GetEmployeesFromPersonel()
        {
            _logger.LogInformation("Fetching personnel records from the database.");
            var personelRecords = await _personelRepository.GetAllPersonelAsync();
            var employees = new List<Employee>();

            var validPersonelRecords = personelRecords.Where(x => x.EMAIL != null && x.DOTERM == null).ToList();
            foreach (var person in validPersonelRecords)
            {
                var employee = new Employee
                {
                    CompanyId = null,
                    DepartmentId = null,
                    CreatedBy = null,
                    FName = $"{person.FIRSTNAME} {person.LASTNAME}",
                    FirstName = person.FIRSTNAME,
                    LastName = person.LASTNAME,
                    Email = person.EMAIL,
                    Password = "Test@123",
                    ShiftId1 = null,
                    ShiftId2 = null,
                    ShiftId3 = null,
                    LocationId = null,
                    OverTime = null,
                    HourlyRate = null,
                    Type = 5,
                    EmployeeId = person.PersonalID.ToString(),
                };
                employees.Add(employee);
            }

            _logger.LogInformation("Mapped {Count} personnel records to employee entities.", employees.Count);
            return employees;
        }

        public async Task<GenericResponse<List<Employee>>> PostEmployeesToExternalApi()
        {
            using var transaction = await _personelRepository.BeginTransactionAsync(); // Start a transaction

            try
            {
                _logger.LogInformation("Preparing to sync employees with the external API.");
                var employees = await GetEmployeesFromPersonel();

                var response = await _restClientService.PostAsync<List<Employee>>("Employee/sync-employees", employees);

                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Successfully synced employees with the external API.");
                    await transaction.CommitAsync(); // Commit transaction if API call is successful
                    return new GenericResponse<List<Employee>>(true, "Employees synced successfully.", employees);
                }
                else
                {
                    string errorMessage = "Failed to sync employees with external API.";
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
                    return new GenericResponse<List<Employee>>(false, errorMessage, null);
                }

                // return new GenericResponse<List<Employee>>(true, "Employees", employees);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback transaction on exception
                _logger.LogError(ex, "An error occurred while syncing employees with the external API.");
                return new GenericResponse<List<Employee>>(false, "An error occurred while syncing employees.", null);
            }
        }
    }
}
