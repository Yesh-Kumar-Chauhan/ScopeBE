using Core.Entities;
using DataMigrationLibrary.Data;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationLibrary.Services
{
    public class DataMigrationService
    {
        private readonly LegacyDbContext _legacyContext;
        private readonly NewAppDbContext _newContext;

        public DataMigrationService(LegacyDbContext legacyContext, NewAppDbContext newContext)
        {
            _legacyContext = legacyContext;
            _newContext = newContext;
        }

        //Migrate Districts
        public async Task MigrateDistrictsAsync()
        {
            var transaction = await _newContext.Database.BeginTransactionAsync();
            try
            {
                var districts = _legacyContext.Districts.ToList();

                // Temporarily enable IDENTITY_INSERT for the Districts table
                await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Districts ON");

                foreach (var district in districts)
                {
                    _newContext.Districts.Add(district);
                }

                await _newContext.SaveChangesAsync();

                // Disable IDENTITY_INSERT after inserting
                await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Districts OFF");

                // Commit the transaction
                await transaction.CommitAsync();

                Console.WriteLine("Data migration completed successfully.");
            }
            catch (Exception ex)
            {
                // If there's an error, make sure to disable IDENTITY_INSERT and rollback the transaction
                await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Districts OFF");
                await transaction.RollbackAsync();
                throw ex;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        //Migrate Sites
        public async Task MigrateSitesAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var sites = _legacyContext.Sites.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Sites table
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Sites ON");

                        foreach (var site in sites)
                        {
                            _newContext.Sites.Add(site);
                        }

                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Sites OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Sites migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // If there's an error, rollback the transaction
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }
            });
        }

        public async Task MigratePersonelAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all personnel from the legacy context
                        var personnel = _legacyContext.Personel.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Personel table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Personel ON");

                        // Add each personnel to the new context
                        foreach (var person in personnel)
                        {
                            _newContext.Personel.Add(person);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Personel OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Personnel migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during personnel migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }


        public async Task MigrateReportsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all personnel from the legacy context
                        var reports = _legacyContext.Reports.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Personel table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Reports ON");

                        // Add each personnel to the new context
                        foreach (var report in reports)
                        {
                            _newContext.Reports.Add(report);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Reports OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Personnel migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during personnel migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }

        public async Task MigrateSchoolsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all schools from the legacy context
                        var schools = _legacyContext.Schools.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Schools table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Schools ON");

                        // Add each school to the new context
                        foreach (var school in schools)
                        {
                            _newContext.Schools.Add(school);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Schools OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("School migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during school migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }


        public async Task MigrateWorkshopTypesAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all workshop types from the legacy context
                        var workshopTypes = _legacyContext.WorkshopType.ToList();

                        // Temporarily enable IDENTITY_INSERT for the WorkshopTypes table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT WorkshopType ON");

                        // Add each workshop type to the new context
                        foreach (var workshopType in workshopTypes)
                        {
                            _newContext.WorkshopType.Add(workshopType);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT WorkshopType OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Workshop type migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during workshop type migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }

        public async Task MigrateTopicTypesAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all topic types from the legacy context
                        var topicTypes = _legacyContext.TopicType.ToList();

                        // Temporarily enable IDENTITY_INSERT for the TopicTypes table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT TopicType ON");

                        // Add each topic type to the new context
                        foreach (var topicType in topicTypes)
                        {
                            _newContext.TopicType.Add(topicType);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT TopicType OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Topic type migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during topic type migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }

        public async Task MigrateInservicesAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all inservices from the legacy context
                        var inservices = _legacyContext.Inservices.AsNoTracking().ToList();

                        //// Temporarily enable IDENTITY_INSERT for the Inservices table if needed
                        //await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Inservices ON");

                        //// Perform bulk insert using EFCore.BulkExtensions with explicit identity handling
                        //var bulkConfig = new BulkConfig
                        //{
                        //    PreserveInsertOrder = true, // This ensures the insert order is preserved
                        //    SetOutputIdentity = true    // This enables setting the identity manually
                        //};

                        await _newContext.BulkInsertAsync(inservices);

                        // Disable IDENTITY_INSERT after inserting
                        //await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Inservices OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Inservice migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during inservice migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }

        //closing
        public async Task MigrateClosingsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all closings from the legacy context
                        var closings = _legacyContext.Closings.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Closings table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Closings ON");

                        // Add each closing to the new context
                        foreach (var closing in closings)
                        {
                            _newContext.Closings.Add(closing);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Closings OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Closing migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during closing migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }

        //status
        public async Task MigrateStatusesAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all statuses from the legacy context
                        var statuses = _legacyContext.Status.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Status table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Status ON");

                        // Add each status to the new context
                        foreach (var status in statuses)
                        {
                            _newContext.Status.Add(status);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Status OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Status migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during status migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }

        public async Task MigrateTimesheetsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all timesheets from the legacy context
                        var timesheets = _legacyContext.Timesheet.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Timesheet table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Timesheet ON");

                        // Add each timesheet to the new context
                        foreach (var timesheet in timesheets)
                        {
                            _newContext.Timesheet.Add(timesheet);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Timesheet OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Timesheet migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during timesheet migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }

        public async Task MigrateVisitsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all visits from the legacy context
                        var visits = _legacyContext.Visits.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Visit table if needed
                        //await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Visits ON");

                        // Add each visit to the new context
                        //foreach (var visit in visits)
                        //{
                        //    _newContext.Visits.Add(visit);
                        //}

                        //// Save all changes to the new context
                        //await _newContext.SaveChangesAsync();
                        await _newContext.BulkInsertAsync(visits);

                        // Disable IDENTITY_INSERT after inserting
                        //await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Visits OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Visit migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during visit migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }


        public async Task MigrateContactsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all contacts from the legacy context
                        var contacts = _legacyContext.Contacts.ToList();

                        // Temporarily enable IDENTITY_INSERT for the Contacts table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Contacts ON");

                        // Add each contact to the new context
                        foreach (var contact in contacts)
                        {
                            _newContext.Contacts.Add(contact);
                        }

                        // Save all changes to the new context
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT after inserting
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Contacts OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Contact migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during contact migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }


        public async Task MigrateWorkshopsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve all workshops from the legacy context
                        var workshops = _legacyContext.Workshops
                            .Include(w => w.WorkshopMembers)
                            .Include(w => w.WorkshopTopics)
                            .ToList();

                        // Temporarily enable IDENTITY_INSERT for the Workshops table if needed
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Workshops ON");

                        // Migrate each workshop
                        foreach (var workshop in workshops)
                        {
                            // Reset entity state to added to avoid conflict with the same primary key
                            _newContext.Entry(workshop).State = EntityState.Added;

                            // Add the workshop to the new context
                            _newContext.Workshops.Add(workshop);
                        }

                        // Save changes to Workshops table
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT for the Workshops table
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Workshops OFF");

                        // Temporarily enable IDENTITY_INSERT for the WorkshopMembers table
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT WorkshopMembers ON");

                        // Migrate related WorkshopMembers
                        foreach (var workshop in workshops)
                        {
                            foreach (var member in workshop.WorkshopMembers)
                            {
                                // Reset entity state to added to avoid conflict with the same primary key
                                _newContext.Entry(member).State = EntityState.Added;
                                _newContext.WorkshopMembers.Add(member);
                            }
                        }

                        // Save changes to WorkshopMembers table
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT for the WorkshopMembers table
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT WorkshopMembers OFF");

                        // Temporarily enable IDENTITY_INSERT for the WorkshopTopics table
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT WorkshopTopics ON");

                        // Migrate related WorkshopTopics
                        foreach (var workshop in workshops)
                        {
                            foreach (var topic in workshop.WorkshopTopics)
                            {
                                // Reset entity state to added to avoid conflict with the same primary key
                                _newContext.Entry(topic).State = EntityState.Added;
                                _newContext.WorkshopTopics.Add(topic);
                            }
                        }

                        // Save changes to WorkshopTopics table
                        await _newContext.SaveChangesAsync();

                        // Disable IDENTITY_INSERT for the WorkshopTopics table
                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT WorkshopTopics OFF");

                        // Commit the transaction
                        await transaction.CommitAsync();

                        Console.WriteLine("Workshop migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during workshop migration: {ex.Message}");
                        throw; // Rethrow the exception to ensure proper handling/logging outside this method
                    }
                }
            });
        }


        public async Task MigrateAbsencesAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var absences = _legacyContext.Absences.ToList();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Absences ON");

                        foreach (var absence in absences)
                        {
                            _newContext.Absences.Add(absence);
                        }

                        await _newContext.SaveChangesAsync();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Absences OFF");

                        await transaction.CommitAsync();

                        Console.WriteLine("Absences migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during absences migration: {ex.Message}");
                        throw;
                    }
                }
            });
        }

        public async Task MigrateAbsenceReasonsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var absenceReasons = _legacyContext.AbsenceReasons.ToList();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT AbsenceReasons ON");

                        foreach (var absenceReason in absenceReasons)
                        {
                            _newContext.AbsenceReasons.Add(absenceReason);
                        }

                        await _newContext.SaveChangesAsync();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT AbsenceReasons OFF");

                        await transaction.CommitAsync();

                        Console.WriteLine("Absence reasons migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during absence reasons migration: {ex.Message}");
                        throw;
                    }
                }
            });
        }

        public async Task MigrateAttendanceAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var attendances = _legacyContext.Attendance.ToList();

                        //await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Attendance ON");

                        //foreach (var attendance in attendances)
                        //{
                        //    _newContext.Attendance.Add(attendance);
                        //}

                        //await _newContext.SaveChangesAsync();

                        //await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Attendance OFF");

                        await _newContext.BulkInsertAsync(attendances);
                        await transaction.CommitAsync();

                        Console.WriteLine("Attendance migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during attendance migration: {ex.Message}");
                        throw;
                    }
                }
            });
        }



        public async Task MigrateWavierReceivedAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var waiversReceived = _legacyContext.WaiversReceived.ToList();

                        //await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Attendance ON");

                        //foreach (var attendance in attendances)
                        //{
                        //    _newContext.Attendance.Add(attendance);
                        //}

                        //await _newContext.SaveChangesAsync();

                        //await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Attendance OFF");

                        await _newContext.BulkInsertAsync(waiversReceived);
                        await transaction.CommitAsync();

                        Console.WriteLine("Attendance migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during attendance migration: {ex.Message}");
                        throw;
                    }
                }
            });
        }

        public async Task MigrateCertificateTypeAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var certificateTypes = _legacyContext.CertificateType.ToList();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT CertificateType ON");

                        foreach (var certificateType in certificateTypes)
                        {
                            _newContext.CertificateType.Add(certificateType);
                        }

                        await _newContext.SaveChangesAsync();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT CertificateType OFF");

                        //await _newContext.BulkInsertAsync(waiversReceived);
                        await transaction.CommitAsync();

                        Console.WriteLine("CertificateType migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during CertificateType migration: {ex.Message}");
                        throw;
                    }
                }
            });
        }

        public async Task MigrateCertificatesAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var certificates = _legacyContext.Certificates.ToList();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Certificates ON");

                        foreach (var certificate in certificates)
                        {
                            _newContext.Certificates.Add(certificate);
                        }

                        await _newContext.SaveChangesAsync();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Certificates OFF");

                        //await _newContext.BulkInsertAsync(waiversReceived);
                        await transaction.CommitAsync();

                        Console.WriteLine("Certificates migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during Certificates migration: {ex.Message}");
                        throw;
                    }
                }
            });
        }

        public async Task MigrateWaiversSentAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var waiversSents = _legacyContext.WaiversSent.ToList();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT WaiversSent ON");

                        foreach (var waiversSent in waiversSents)
                        {
                            _newContext.WaiversSent.Add(waiversSent);
                        }

                        await _newContext.SaveChangesAsync();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT WaiversSent OFF");

                        //await _newContext.BulkInsertAsync(waiversReceived);
                        await transaction.CommitAsync();

                        Console.WriteLine("WaiversSent migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during WaiversSent migration: {ex.Message}");
                        throw;
                    }
                }
            });
        }

        public async Task MigrateDirectorsAsync()
        {
            var strategy = _newContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _newContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var directors = _legacyContext.Directors.ToList();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Directors ON");

                        foreach (var director in directors)
                        {
                            _newContext.Directors.Add(director);
                        }

                        await _newContext.SaveChangesAsync();

                        await _newContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Directors OFF");

                        //await _newContext.BulkInsertAsync(waiversReceived);
                        await transaction.CommitAsync();

                        Console.WriteLine("WaiversSent migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"An error occurred during WaiversSent migration: {ex.Message}");
                        throw;
                    }
                }
            });
        }
    }
}
