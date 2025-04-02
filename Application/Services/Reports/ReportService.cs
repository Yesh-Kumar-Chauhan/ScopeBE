using AutoMapper;
using Core.DTOs.Report;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Dapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Globalization;
using System.Text;

namespace Application.Services.Reports
{
    public partial class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;


        public ReportService(IReportRepository reportRepository, IMapper mapper, ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<IEnumerable<ReportDto>>> GetAllReportsAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllReportsAsync();

                var orderedReports = reports.OrderBy(r => r.Name);

                // Map the ordered reports to DTOs
                var reportDtos = _mapper.Map<IEnumerable<ReportDto>>(orderedReports);

                return new GenericResponse<IEnumerable<ReportDto>>(true, "Reports retrieved successfully.", reportDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all reports.");
                return new GenericResponse<IEnumerable<ReportDto>>(false, "An error occurred while retrieving reports.", null);
            }
        }

        public async Task<GenericResponse<ReportDto>> GetReportByIdAsync(long id)
        {
            try
            {
                var report = await _reportRepository.GetReportByIdAsync(id);
                if (report == null)
                {
                    _logger.LogWarning("Report with ID {ReportId} not found.", id);
                    return new GenericResponse<ReportDto>(false, "Report not found.", null);
                }

                var reportDto = _mapper.Map<ReportDto>(report);
                return new GenericResponse<ReportDto>(true, "Report retrieved successfully.", reportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report with ID {ReportId}.", id);
                return new GenericResponse<ReportDto>(false, "An error occurred while retrieving the report.", null);
            }
        }

        public async Task<GenericResponse<ReportDto>> CreateReportAsync(ReportDto reportDto)
        {
            try
            {
                var report = _mapper.Map<Report>(reportDto);
                var createdReport = await _reportRepository.AddReportAsync(report);
                var createdReportDto = _mapper.Map<ReportDto>(createdReport);
                return new GenericResponse<ReportDto>(true, "Report created successfully.", createdReportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new report.");
                return new GenericResponse<ReportDto>(false, "An error occurred while creating the report.", null);
            }
        }

        public async Task<GenericResponse<ReportDto>> UpdateReportAsync(long id, ReportDto reportDto)
        {
            try
            {
                var report = _mapper.Map<Report>(reportDto);
                report.ID = id;

                var updatedReport = await _reportRepository.UpdateReportAsync(report);
                if (updatedReport == null)
                {
                    _logger.LogWarning("Report with ID {ReportId} not found or update failed.", id);
                    return new GenericResponse<ReportDto>(false, "Report not found or update failed.", null);
                }

                var updatedReportDto = _mapper.Map<ReportDto>(updatedReport);
                return new GenericResponse<ReportDto>(true, "Report updated successfully.", updatedReportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating report with ID {ReportId}.", id);
                return new GenericResponse<ReportDto>(false, "An error occurred while updating the report.", null);
            }
        }

        public async Task<GenericResponse<bool>> DeleteReportAsync(long id)
        {
            try
            {
                var deleted = await _reportRepository.DeleteReportAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Report with ID {ReportId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "Report not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "Report deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting report with ID {ReportId}.", id);
                return new GenericResponse<bool>(false, "An error occurred while deleting the report.", false);
            }
        }

        public async Task<GenericResponse<object>> GenerateReportAsync(GenerateReportDto reportDto)
        {
            try
            {
                GenericResponse<Dictionary<string, object>> response;

                switch (reportDto.ReportId)
                {
                    case 1:
                        response = await GenerateSiteInformationAsync(reportDto);
                        break;
                    case 2:
                        response = await GenerateStaffScheduleAsync(reportDto);
                        break;
                    case 3:
                        response = await GenerateFirstAidExpirationDataAsync(reportDto);
                        break;
                    case 4:
                        response = await GenerateMATExpirationBySiteReportDataAsync(reportDto);
                        break;
                    case 5:
                        response = await GenerateSitePhoneDataAsync(reportDto);
                        break;
                    case 7:
                        response = await GenerateStaffMaximumHourDataAsync(reportDto);
                        break;
                    case 8:
                        response = await GenerateStaffScheduleAsync(reportDto); // for staff schedule blank same as staff schedule
                        break;
                    case 9:
                        response = await GenerateStaffSignInGroupAsync(reportDto);
                        break;
                    case 10:
                        response = await GenerateScopeTimeSheetReportAsync(reportDto);
                        break;
                    case 11:
                        response = await GenerateStaffInserviceAsync(reportDto);
                        break;
                    case 12:
                        response = await GenerateFingerprintWaiverAdditionalSitesAsync(reportDto);
                        break;
                    case 13:
                        response = await GenerateWaiverExpirationAsync(reportDto);
                        break;
                    case 14:
                        response = await GenerateFirstAidExpirationAsync(reportDto);
                        break;
                    case 15:
                        response = await GenerateVisitsDataAsync(reportDto);
                        break;
                    case 16:
                        response = await GenerateStaffSalaryDataAsync(reportDto);
                        break;
                    case 18:
                        response = await GenerateSiteListforHCPAsync(reportDto);
                        break;
                    case 19:
                        response = await GenerateExpungeLetterAsync(reportDto);
                        break;
                    case 20:
                        response = await GenerateMATExpirationBySiteReportDataAsync(reportDto);
                        break;
                    case 21:
                        response = await GenerateExpungeLetterAsync(reportDto);
                        break;
                    case 22:
                        response = await GenerateStaffEmailAddressAsync(reportDto);
                        break;
                    case 23:
                        response = await GenerateScopeTimeSheetReportAsync(reportDto); //SCOPE Employee - Timesheet without Code -23
                        break;
                    case 24:
                        response = await GenerateScopeTimeSheetReportAsync(reportDto); //1:1 Timesheet
                        break;
                    case 25:
                        response = await GenerateScopeTimeSheetReportAsync(reportDto); //Scope Substitute -Timesheet
                        break;
                    case 26:
                        response = await GenerateScopeTimeSheetReportAsync(reportDto); //TrainerTimesheet
                        break;
                    case 27:
                        response = await GenerateStaffMatCprFaDataAsync(reportDto);
                        break;
                    case 28:
                        response = await GenerateSiteAssignmentDataAsync(reportDto);
                        break;
                    case 29:
                        response = await GenerateSitePermitExpirationReportAsync(reportDto);
                        break;
                    case 30:
                        response = await GenerateEmergencyPhoneAsync(reportDto);
                        break;
                    case 31:
                         response = await GenerateWelcomeBackNewHireLetterAsync(reportDto, 31);
                        break;
                    case 32:
                        response = await GenerateSiteAddressListAsync(reportDto);
                        break;
                    case 33:
                        response = await GenerateSiteEmergencyInformationAsync(reportDto);
                        break;
                    case 34:
                        response = await GenerateWelcomeBackNewHireLetterAsync(reportDto, 34);
                        break;
                    case 35:
                        response = await GenerateWelcomeBackNewHireLetterAsync(reportDto, 35);
                        break;
                    case 36:
                        response = await GenerateContactReportAsync(reportDto);
                        break;
                    case 37:
                        response = await GenerateWavierReceivedAsync(reportDto);
                        break;
                    case 38:
                        response = await GenerateCPRExpirationAsync(reportDto);
                        break;
                    case 39:
                        response = await GenerateStaffTrackingAsync(reportDto);
                        break;
                    case 42:
                        response = await GenerateStaffAttendanceGroupAsync(reportDto);
                        break;
                    case 43:
                        response = await GenerateStaffSignInWithSiteAsync(reportDto);
                        break;
                    case 44:
                        response = await GenerateTotalAttendanceReportAsync(reportDto);
                        break;
                    case 45:
                        response = await GenerateFirstAidChildAbuseAsync(reportDto);
                        break;
                    case 48:
                        response = await GenerateWelcomeBackNewHireLetterAsync(reportDto, 48);
                        break;
                    case 49:
                        response = await GenerateSiteSpaceAsync(reportDto);
                        break;
                    case 50:
                        response = await GenerateStaffExpirationAlphaAsync(reportDto);
                        break;
                    case 51:
                        response = await GenerateEmergencyClosingAsync(reportDto);
                        break;
                    case 52:
                        response = await GenerateStaffAttendanceAllGroupAsync(reportDto);
                        break;
                    case 53:
                        response = await GenerateFoundationAsync(reportDto);
                        break;
                    case 54:
                        response = await GenerateEmptyFoundationAsync(reportDto);
                        break;




                    case 55:
                        response = await GenerateChangeOfStatusLetterAsync(reportDto);
                        break;
                    case 56:
                        response = await GenerateChangeOfStatusLetterAsync(reportDto);
                        break;

                    case 57:
                        response = await GeneratePromotionLetterAsync(reportDto);
                        break;
                    case 58:
                        response = await GeneratePromotionLetterAsync(reportDto);
                        break;

                    case 59:
                        response = await GenerateScheduleChangeLetterAsync(reportDto);
                        break;
                    case 60:
                        response = await GenerateScheduleChangeLetterAsync(reportDto);
                        break;



                    case 61:
                        response = await GenerateStaffAttendanceGroupEmptyReportAsync(reportDto);
                        break;
                    case 62:
                        response = await GenerateTotalAttendanceReportAsync(reportDto);
                        break;
                    case 65:
                        response = await GenerateSitePhoneDataAsync(reportDto);
                        break;
                    case 68:
                        response = await GenerateSiteNurseDataAsync(reportDto);
                        break;
                    case 69:
                        response = await GenerateChildAbuseExpirationAsync(reportDto);
                        break;
                    case 70:
                        response = await GenerateSiteLicensorsAsync(reportDto);
                        break;
                    case 71:
                        response = await GenerateStaffAttendanceGroupAsync(reportDto);
                        break;
                    case 72:
                        response = await GenerateStaffAttendanceAllGroupAsync(reportDto);
                        break;
                    case 73:
                        response = await GenerateStaffAttendanceGroupEmptyReportAsync(reportDto);
                        break;
                    case 74:
                        response = await GenerateStaffDataOfEmploymentAsync(reportDto);
                        break;
                    case 76:
                        response = await GenerateExportStaffInfoReportAsync(reportDto);// label 1
                        break;
                    case 77:
                        response = await GenerateExportStaffInfoReportAsync(reportDto); // label 2
                        break;
                    case 78:
                        response = await GenerateTotalAttendanceZeroReportAsync(reportDto);
                        break;
                    case 80:
                        response = await GenerateScopeTimeSheetReportAsync(reportDto);
                        break;
                    case 81:
                        response = await GenerateScopeTimeSheetAsync(reportDto);
                        break;
                    case 82:
                        response = await GenerateSiteAddressListAsync(reportDto);
                        break;
                    case 83:
                        response = await GenerateStaffScheduleWithInfoAsync(reportDto);
                        break;
                    case 84:
                        response = await GenerateScopeTimeSheetAsync(reportDto);
                        break;
                    case 85:
                        response = await GenerateScopeEmployeeTimeSheetAsync(reportDto);
                        break;
                    case 86:
                        response = await GenerateSexualHarassmentReportAsync(reportDto);
                        break;
                    case 87:
                        response = await GenerateStuffSignInWithoutSiteReportAsync(reportDto);
                        break;
                    case 88:
                        response = await GenerateCBCReportAsync(reportDto);
                        break;
                    case 89:
                        response = await GenerateWorkshopAsync(reportDto);
                        break;
                    case 90:
                        response = await GenerateStaffWorkshopsAsync(reportDto);
                        break;

                    case 91:
                        response = await GenerateScopeEmployeeTimeSheetAsync(reportDto);
                        break;
                    case 92:
                        response = await GenerateSitePermitNumberReportAsync(reportDto);
                        break;
                    case 93:
                        response = await GenerateInserviceStaffTotalsAsync(reportDto);
                        break;
                    case 94:
                        response = await GenerateStaffChecklistsAsync(reportDto);
                        break;
                    case 95:
                        response = await GenerateStaffBirthMonthsAsync(reportDto);
                        break;
                    default:
                        return new GenericResponse<object>(false, "Unsupported report type.", null);
                }

                if (response.Success && response.Data != null)
                {
                    // Check if the response contains multiple files
                    if (response.Data.ContainsKey("Files") && response.Data["Files"] is List<Dictionary<string, object>> files)
                    {
                        var fileList = new List<object>();

                        foreach (var file in files)
                        {
                            if (file.ContainsKey("FileContent") && file["FileContent"] is byte[] fileContent)
                            {
                                // Extract the file name
                                string fileName = file.ContainsKey("FileName") ? file["FileName"].ToString() : "Report.docx";

                                // Encode the file content as a base64 string
                                string base64FileContent = Convert.ToBase64String(fileContent);

                                // Add the file info to the file list
                                fileList.Add(new { FileName = fileName, FileContent = base64FileContent });
                            }
                        }

                        // Return a response containing the list of files
                        return new GenericResponse<object>(true, "Files generated successfully.", new { Files = fileList, FileContent = (object)null, FileName = (object)null, ReportData = (object)null });
                    }

                    // Check if the response contains a single file content
                    if (response.Data.ContainsKey("FileContent") && response.Data["FileContent"] is byte[] singleFileContent)
                    {
                        // Extract file name
                        string fileName = response.Data.ContainsKey("FileName") ? response.Data["FileName"].ToString() : "Report.docx";

                        // Encode file content as a base64 string
                        string base64FileContent = Convert.ToBase64String(singleFileContent);

                        // Return a response containing the file content, file name, and any additional data
                        return new GenericResponse<object>(true, "Report generated successfully.", new { Files = (object)null, FileName = fileName, FileContent = base64FileContent, ReportData = response.Data["ReportData"] });
                    }

                    // Check if the response contains data
                    if (response.Data.ContainsKey("ReportData"))
                    {
                        // Return a response containing the data
                        return new GenericResponse<object>(true, "Data retrieved successfully.", new { Files = (object)null, ReportData = response.Data["ReportData"], FileContent = (object)null, FileName = (object)null });
                    }
                }

                // Return a response indicating no data found if no file content
                return new GenericResponse<object>(true, response.Message, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the report.");
                return new GenericResponse<object>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<Dictionary<string, object>>> GeneratePromotionLetterAsync(GenerateReportDto reportDto)
        {
            //var parameters = new DynamicParameters();
            //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
            //parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
            //parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
            //parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
            //parameters.Add("@Type", reportDto.Type, DbType.Int32);

            //// Call the repository to execute the stored procedure and fetch the data
            //var promotionLetters = await _reportRepository.ExecuteStoredProcedureAsync("sp_WelcomeLatter_Report", parameters);
            var promotionLetters = await _reportRepository.GetWelcomeLettersAsync(
                reportDto.PersonelId,
                reportDto.SiteId,
                reportDto.DistrictId,
                reportDto.CountryId,
                reportDto.Type);

            // Check if promotionLetters is empty
            if (promotionLetters == null || !promotionLetters.Any())
            {
                return new GenericResponse<Dictionary<string, object>>(true, "No letters found.", null);
            }

            // List to hold the generated files
            var filesList = new List<Dictionary<string, object>>();
            string siteType = !string.IsNullOrEmpty(reportDto.Selections) ? reportDto.Selections : "0";
            // Iterate over each letter and generate the document
            foreach (var letter in promotionLetters)
            {
                byte[] reportData = GeneratePromotionWordDocument(letter, siteType);
                string fileName = $"PromotionLetter_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";

                // Add the file content and file name to the list
                filesList.Add(new Dictionary<string, object>
                {
                     { "FileContent", reportData },
                     { "FileName", fileName }
                });
            }

            // Prepare the final response
            var result = new Dictionary<string, object>
            {
                { "Files", filesList }
            };

            return new GenericResponse<Dictionary<string, object>>(true, "Report(s) generated successfully.", result);
        }


        private byte[] GeneratePromotionWordDocument(WelcomeLatter letter, string siteType)
        {
            // Load template file into a byte array
            string templatePath = GetTemplatePath(Core.Enums.Templates.PromotionLetter);
            byte[] templateBytes = File.ReadAllBytes(templatePath);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(templateBytes, 0, templateBytes.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    if (mainPart == null) throw new InvalidOperationException("MainDocumentPart is missing");

                    // Get the document body
                    var body = mainPart.Document.Body;

                    if (!string.IsNullOrWhiteSpace(letter.FirstName) || !string.IsNullOrWhiteSpace(letter.LastName))
                    {
                        // Replace each placeholder with actual data
                        ReplacePlaceholder(body, "«LatterDate»", DateTime.Today.ToString("MMMM d, yyyy"));
                        ReplacePlaceholder(body, "«FullName»", $"{letter.FirstName} {letter.LastName}");
                        ReplacePlaceholder(body, "«FirstName»", $"{letter.FirstName}");
                        ReplacePlaceholder(body, "«Address»", letter.Street);
                        ReplacePlaceholder(body, "«City»", letter.City);
                        ReplacePlaceholder(body, "«State»", letter.State);
                        ReplacePlaceholder(body, "«ZipCode»", letter.ZipCode);

                        // Get position details and bind them to placeholders
                        var positionDetails = GetPositionDetails(letter, siteType);
                        foreach (var detail in positionDetails)
                        {
                            ReplacePlaceholder(body, $"«{detail.Key}»", detail.Value);
                        }

                        // You can now bind each field individually like this:
                        ReplacePlaceholder(body, "«EffectiveDate»", GetEffectiveDate(letter, siteType));
                    }

                    mainPart.Document.Save();
                }

                return memoryStream.ToArray();
            }
        }


        public async Task<GenericResponse<Dictionary<string, object>>> GenerateChangeOfStatusLetterAsync(GenerateReportDto reportDto)
        {
            //var parameters = new DynamicParameters();
            //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
            //parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
            //parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
            //parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
            //parameters.Add("@Type", reportDto.Type, DbType.Int32);

            //// Call the repository to execute the stored procedure and fetch the data
            //var changeOfStatusLetters = await _reportRepository.ExecuteStoredProcedureAsync("sp_WelcomeLatter_Report", parameters);

            var changeOfStatusLetters = await _reportRepository.GetWelcomeLettersAsync(
                reportDto.PersonelId,
                reportDto.SiteId,
                reportDto.DistrictId,
                reportDto.CountryId,
                reportDto.Type);

            // Check if changeOfStatusLetters is empty
            if (changeOfStatusLetters == null || !changeOfStatusLetters.Any())
            {
                return new GenericResponse<Dictionary<string, object>>(true, "No letters found.", null);
            }

            
            // List to hold the generated files
            var filesList = new List<Dictionary<string, object>>();

            // Iterate over each letter and generate the document
            string siteType = !string.IsNullOrEmpty(reportDto.Selections) ? reportDto.Selections : "0";
            foreach (var letter in changeOfStatusLetters)
            {
                byte[] reportData = GenerateChangeOfStatusWordDocument(letter, siteType);
                string fileName = $"ChangeOfStatusLetter_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";

                // Add the file content and file name to the list
                filesList.Add(new Dictionary<string, object>
                {
                     { "FileContent", reportData },
                     { "FileName", fileName }
                });
            }

            // Prepare the final response
            var result = new Dictionary<string, object>
            {
                { "Files", filesList }
            };

            return new GenericResponse<Dictionary<string, object>>(true, "Report(s) generated successfully.", result);
        }

        private byte[] GenerateChangeOfStatusWordDocument(WelcomeLatter letter, string siteType)
        {
            // Load template file into a byte array
            string templatePath = GetTemplatePath(Core.Enums.Templates.ChangeStatusLetter);
            byte[] templateBytes = File.ReadAllBytes(templatePath);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(templateBytes, 0, templateBytes.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    if (mainPart == null) throw new InvalidOperationException("MainDocumentPart is missing");

                    // Get the document body
                    var body = mainPart.Document.Body;

                    if (!string.IsNullOrWhiteSpace(letter.FirstName) || !string.IsNullOrWhiteSpace(letter.LastName))
                    {
                        // Replace each placeholder with actual data
                        ReplacePlaceholder(body, "«LatterDate»", DateTime.Today.ToString("MMMM d, yyyy"));
                        ReplacePlaceholder(body, "«FullName»", $"{letter.FirstName} {letter.LastName}");
                        ReplacePlaceholder(body, "«FirstName»", $"{letter.FirstName}");
                        ReplacePlaceholder(body, "«Address»", letter.Street);
                        ReplacePlaceholder(body, "«City»", letter.City);
                        ReplacePlaceholder(body, "«State»", letter.State);
                        ReplacePlaceholder(body, "«ZipCode»", letter.ZipCode);

                        // Get position details and bind them to placeholders
                        var positionDetails = GetPositionDetails(letter, siteType);
                        foreach (var detail in positionDetails)
                        {
                            ReplacePlaceholder(body, $"«{detail.Key}»", detail.Value);
                        }

                        // You can now bind each field individually like this:
                        ReplacePlaceholder(body, "«EffectiveDate»", GetEffectiveDate(letter, siteType));
                    }


                    mainPart.Document.Save();
                }

                return memoryStream.ToArray();
            }
        }


        private Dictionary<string, string> GetPositionDetails(WelcomeLatter item, string siteType)
        {
            var details = new Dictionary<string, string>();
            string jan = "", feb = "", sep = "", effectiveDate = "N/A";

            // For Site B
            if (!string.IsNullOrEmpty(item.SiteNameB) && siteType.IndexOfAny(new char[] { '1', '0', '?' }) > -1)
            {
                jan = !string.IsNullOrEmpty(item.JAN_Rate_B) ? decimal.Parse(item.JAN_Rate_B, NumberStyles.Currency).ToString("N2") : "";
                feb = !string.IsNullOrEmpty(item.FEB_Rate_B) ? decimal.Parse(item.FEB_Rate_B, NumberStyles.Currency).ToString("N2") : "";
                sep = !string.IsNullOrEmpty(item.SEP_Rate_B) ? decimal.Parse(item.SEP_Rate_B, NumberStyles.Currency).ToString("N2") : "";
                effectiveDate = item.EffectiveDateBefore?.ToShortDateString() ?? "N/A";

                details["Position"] = item.SitePositionB;
                details["District"] = item.DistrictNameB;
                details["Program"] = item.SiteNameB;
                details["EffectiveDate"] = effectiveDate;
                details["Rate"] = sep;
                details["Schedule"] = $"Mon: {item.MON_1_B}-{item.MON_1_E} Tue: {item.TUE_1_B}-{item.TUE_1_E} Wed: {item.WED_1_B}-{item.WED_1_E} Thu: {item.THU_1_B}-{item.THU_1_E} Fri: {item.FRI_1_B}-{item.FRI_1_E}";
            }

            // For Site D
            if (!string.IsNullOrEmpty(item.SiteNameD) && siteType.IndexOfAny(new char[] { '2', '0', '?' }) > -1)
            {
                jan = !string.IsNullOrEmpty(item.JAN_Rate_D) ? decimal.Parse(item.JAN_Rate_D, NumberStyles.Currency).ToString("N2") : "";
                feb = !string.IsNullOrEmpty(item.FEB_Rate_D) ? decimal.Parse(item.FEB_Rate_D, NumberStyles.Currency).ToString("N2") : "";
                sep = !string.IsNullOrEmpty(item.SEP_Rate_D) ? decimal.Parse(item.SEP_Rate_D, NumberStyles.Currency).ToString("N2") : "";
                effectiveDate = item.EffectiveDateDuring?.ToShortDateString() ?? "N/A";

                details["Position"] = item.SitePositionD;
                details["District"] = item.DistrictNameD;
                details["Program"] = item.SiteNameD;
                details["EffectiveDate"] = effectiveDate;
                details["Rate"] = sep;
                details["Schedule"] = $"Mon: {item.MON_2_B}-{item.MON_2_E} Tue: {item.TUE_2_B}-{item.TUE_2_E} Wed: {item.WED_2_B}-{item.WED_2_E} Thu: {item.THU_2_B}-{item.THU_2_E} Fri: {item.FRI_2_B}-{item.FRI_2_E}";
            }

            // For Site A
            if (!string.IsNullOrEmpty(item.SiteNameA) && siteType.IndexOfAny(new char[] { '3', '0', '?' }) > -1)
            {
                jan = !string.IsNullOrEmpty(item.JAN_Rate_A) ? decimal.Parse(item.JAN_Rate_A, NumberStyles.Currency).ToString("N2") : "";
                feb = !string.IsNullOrEmpty(item.FEB_Rate_A) ? decimal.Parse(item.FEB_Rate_A, NumberStyles.Currency).ToString("N2") : "";
                sep = !string.IsNullOrEmpty(item.SEP_Rate_A) ? decimal.Parse(item.SEP_Rate_A, NumberStyles.Currency).ToString("N2") : "";
                effectiveDate = item.EffectiveDateAfter?.ToShortDateString() ?? "N/A";

                details["Position"] = item.SitePositionA;
                details["District"] = item.DistrictNameA;
                details["Program"] = item.SiteNameA;
                details["EffectiveDate"] = effectiveDate;
                details["Rate"] = sep;
                details["Schedule"] = $"Mon: {item.MON_3_B}-{item.MON_3_E} Tue: {item.TUE_3_B}-{item.TUE_3_E} Wed: {item.WED_3_B}-{item.WED_3_E} Thu: {item.THU_3_B}-{item.THU_3_E} Fri: {item.FRI_3_B}-{item.FRI_3_E}";
            }

            // Add comments if available
            if (!string.IsNullOrEmpty(item.Comment))
            {
                details["Comment"] = item.Comment;
            }

            return details;
        }





        public async Task<GenericResponse<Dictionary<string, object>>> GenerateScheduleChangeLetterAsync(GenerateReportDto reportDto)
        {
            //var parameters = new DynamicParameters();
            //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
            //parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
            //parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
            //parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
            //parameters.Add("@Type", reportDto.Type, DbType.Int32);

            //// Call the repository to execute the stored procedure and fetch the data
            //var scheduleLetters = await _reportRepository.ExecuteStoredProcedureAsync("sp_WelcomeLatter_Report", parameters);

            var scheduleLetters = await _reportRepository.GetWelcomeLettersAsync(
               reportDto.PersonelId,
               reportDto.SiteId,
               reportDto.DistrictId,
               reportDto.CountryId,
               reportDto.Type);

            // Check if scheduleLetters is empty
            if (scheduleLetters == null || !scheduleLetters.Any())
            {
                return new GenericResponse<Dictionary<string, object>>(true, "No letters found.", null);
            }

            // List to hold the generated files
            var filesList = new List<Dictionary<string, object>>();
            string siteType = !string.IsNullOrEmpty(reportDto.Selections) ? reportDto.Selections : "0";
            // Iterate over each letter and generate the document
            foreach (var letter in scheduleLetters)
            {
                byte[] reportData = GenerateScheduleChangeWordDocument(letter, siteType);
                string fileName = $"ScheduleChangeLetter_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";

                // Add the file content and file name to the list
                filesList.Add(new Dictionary<string, object>
                {
                    { "FileContent", reportData },
                    { "FileName", fileName }
                });
            }

            // Prepare the final response
            var result = new Dictionary<string, object>
            {
                { "Files", filesList }
            };

            return new GenericResponse<Dictionary<string, object>>(true, "Report(s) generated successfully.", result);
        }

        private string GetScheduleText(WelcomeLatter letter, string siteType)
        {
            if (siteType.IndexOfAny(new char[] { '1', '0', '?' }) > -1 && !string.IsNullOrEmpty(letter.SiteNameB))
            {
                return $"Mon: {letter.MON_1_B}-{letter.MON_1_E} Tue: {letter.TUE_1_B}-{letter.TUE_1_E} Wed: {letter.WED_1_B}-{letter.WED_1_E} Thu: {letter.THU_1_B}-{letter.THU_1_E} Fri: {letter.FRI_1_B}-{letter.FRI_1_E}\n";
            }
            if (siteType.IndexOfAny(new char[] { '2', '0', '?' }) > -1 && !string.IsNullOrEmpty(letter.SiteNameD))
            {
                return $"Mon: {letter.MON_2_B}-{letter.MON_2_E} Tue: {letter.TUE_2_B}-{letter.TUE_2_E} Wed: {letter.WED_2_B}-{letter.WED_2_E} Thu: {letter.THU_2_B}-{letter.THU_2_E} Fri: {letter.FRI_2_B}-{letter.FRI_2_E}\n";
            }
            if (siteType.IndexOfAny(new char[] { '3', '0', '?' }) > -1 && !string.IsNullOrEmpty(letter.SiteNameA))
            {
                return $"Mon: {letter.MON_3_B}-{letter.MON_3_E} Tue: {letter.TUE_3_B}-{letter.TUE_3_E} Wed: {letter.WED_3_B}-{letter.WED_3_E} Thu: {letter.THU_3_B}-{letter.THU_3_E} Fri: {letter.FRI_3_B}-{letter.FRI_3_E}\n";
            }
            return "N/A";
        }

        private string GetEffectiveDate(WelcomeLatter letter, string siteType)
        {
            if (siteType.IndexOfAny(new char[] { '1', '0', '?' }) > -1)
            {
                return letter.EffectiveDateBefore?.ToShortDateString() ?? "N/A";
            }
            if (siteType.IndexOfAny(new char[] { '2', '0', '?' }) > -1)
            {
                return letter.EffectiveDateDuring?.ToShortDateString() ?? "N/A";
            }
            if (siteType.IndexOfAny(new char[] { '3', '0', '?' }) > -1)
            {
                return letter.EffectiveDateAfter?.ToShortDateString() ?? "N/A";
            }
            return "N/A";
        }


        private string GetProgramText(WelcomeLatter letter, string siteType)
        {
            if (siteType.IndexOfAny(new char[] { '1', '0', '?' }) > -1)
            {
                return letter.SiteNameB ?? "N/A";
            }
            if (siteType.IndexOfAny(new char[] { '2', '0', '?' }) > -1)
            {
                return letter.SiteNameD ?? "N/A";
            }
            if (siteType.IndexOfAny(new char[] { '3', '0', '?' }) > -1)
            {
                return letter.SiteNameA ?? "N/A";
            }
            return "N/A";
        }


        private string GetDistrictText(WelcomeLatter letter, string siteType)
        {
            if (siteType.IndexOfAny(new char[] { '1', '0', '?' }) > -1)
            {
                return letter.DistrictNameB ?? "N/A";
            }
            if (siteType.IndexOfAny(new char[] { '2', '0', '?' }) > -1)
            {
                return letter.DistrictNameD ?? "N/A";
            }
            if (siteType.IndexOfAny(new char[] { '3', '0', '?' }) > -1)
            {
                return letter.DistrictNameA ?? "N/A";
            }
            return "N/A";
        }


        private byte[] GenerateScheduleChangeWordDocument(WelcomeLatter letter, string siteType)
        {
            // Load template file into a byte array
            string templatePath = GetTemplatePath(Core.Enums.Templates.ScheduleChangeLetter);
            byte[] templateBytes = File.ReadAllBytes(templatePath);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(templateBytes, 0, templateBytes.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    if (mainPart == null) throw new InvalidOperationException("MainDocumentPart is missing");

                    // Get the document body
                    var body = mainPart.Document.Body;

                    if (!string.IsNullOrWhiteSpace(letter.FirstName) || !string.IsNullOrWhiteSpace(letter.LastName))
                    {
                        // Replace each placeholder with actual data
                        ReplacePlaceholder(body, "«LatterDate»", DateTime.Today.ToString("MMMM d, yyyy"));
                        ReplacePlaceholder(body, "«FullName»", $"{letter.FirstName} {letter.LastName}");
                        ReplacePlaceholder(body, "«FirstName»", $"{letter.FirstName}");
                        ReplacePlaceholder(body, "«Address»", letter.Street);
                        ReplacePlaceholder(body, "«City»", letter.City);
                        ReplacePlaceholder(body, "«State»", letter.State);
                        ReplacePlaceholder(body, "«ZipCode»", letter.ZipCode);

                        // Get position details and bind them to placeholders
                        var positionDetails = GetPositionDetails(letter, siteType);
                        foreach (var detail in positionDetails)
                        {
                            ReplacePlaceholder(body, $"«{detail.Key}»", detail.Value);
                        }

                        // You can now bind each field individually like this:
                        ReplacePlaceholder(body, "«EffectiveDate»", GetEffectiveDate(letter, siteType));
                    }

                    mainPart.Document.Save();
                }

                return memoryStream.ToArray();
            }
        }


        public async Task<GenericResponse<Dictionary<string, object>>> GenerateIntentLetterAsync(GenerateReportDto reportDto, int reportType)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
            parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
            parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
            parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
            parameters.Add("@Type", reportDto.Type, DbType.Int32);

            // Call the repository to execute the stored procedure and fetch the data
            var intentLettersData = await _reportRepository.ExecuteStoredProcedureAsync("sp_IntentLatter_Report", parameters);

            // Convert dynamic data to List<ExpungeLetter>

            // Check if intentLetters is empty
            if (intentLettersData == null || !intentLettersData.Any())
            {
                return new GenericResponse<Dictionary<string, object>>(true, "No letter found.", null);
            }
            var intentLetters = new List<IntentLatter>();

            foreach (var intentLetter in intentLettersData)
            {
                intentLetters.Add(intentLetter);
            }
            // List to hold the generated files
            var filesList = new List<Dictionary<string, object>>();

            // Iterate over each intentLetter and generate the document
            foreach (var letter in intentLetters)
            {
              
                byte[] reportData;
                string fileName;

                if (reportType == 46)
                {
                    reportData = GenerateIntentWordDocument(letter);
                    fileName = $"IntentLetter_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";

                }
                if (reportType == 47)
                {
                    reportData = GenerateIntentWordDocument(letter);
                    fileName = $"IntentLetter_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";

                }
                else
                {
                    continue;
                }
                // Add the file content and file name to the list
                filesList.Add(new Dictionary<string, object>
                {
                    { "FileContent", reportData },
                    { "FileName", fileName }
                });
            }

            // Prepare the final response
            var result = new Dictionary<string, object>
            {
                { "Files", filesList }
            };

            return new GenericResponse<Dictionary<string, object>>(true, "Report(s) generated successfully.", result);
        }


        private byte[] GenerateIntentWordDocument(IntentLatter intentLetter)
        {
            // Load template file into a byte array
            string templatePath = GetTemplatePath(Core.Enums.Templates.SCOPETemplate_IntentLetter);
            byte[] templateBytes = File.ReadAllBytes(templatePath);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(templateBytes, 0, templateBytes.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    if (mainPart == null) throw new InvalidOperationException("MainDocumentPart is missing");

                    // Get the document body
                    var body = mainPart.Document.Body;

                    if (!string.IsNullOrWhiteSpace(intentLetter.FirstName) || !string.IsNullOrWhiteSpace(intentLetter.LastName))
                    {
                        // Replace each placeholder with actual data
                        ReplacePlaceholder(body, "«FULLNAME»", $"{intentLetter.FirstName} {intentLetter.LastName}");
                        ReplacePlaceholder(body, "«ADDRESS»", intentLetter.Street);
                        ReplacePlaceholder(body, "«CITY»", intentLetter.City);
                        ReplacePlaceholder(body, "«State»", intentLetter.State);
                        ReplacePlaceholder(body, "«ZipCode»", intentLetter.ZipCode);
                        ReplacePlaceholder(body, "«FirstName»", intentLetter.FirstName);
                    }

                    mainPart.Document.Save();
                }

                return memoryStream.ToArray();
            }
        }


        public async Task<GenericResponse<Dictionary<string, object>>> GenerateWelcomeBackNewHireLetterAsync(GenerateReportDto reportDto, int reportType)
        {
            var welcomeLetters = await _reportRepository.GetWelcomeLettersAsync(
                reportDto.PersonelId,
                reportDto.SiteId,
                reportDto.DistrictId,
                reportDto.CountryId,
                reportDto.Type);

            // Check if welcomeLetters is empty
            if (welcomeLetters == null || !welcomeLetters.Any())
            {
                return new GenericResponse<Dictionary<string, object>>(true, "No letter found.", null);
            }

            // List to hold the generated files
            var filesList = new List<Dictionary<string, object>>();

            // Iterate over each welcomeLetter and generate the document
            foreach (var letter in welcomeLetters)
            {
                byte[] reportData;
                string fileName;

                if (reportType == 48)
                {
                    reportData = GenerateWordDocument(letter);
                    fileName = $"WelcomeBackNewHire_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";
                }
                else if (reportType == 31)
                {
                    reportData = GenerateWelcomeLetterWordDocument(letter);
                    fileName = $"WelcomeBackHire_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";
                }
                else if (reportType == 34)
                {
                    reportData = GenerateWordDocument(letter);
                    fileName = $"WelcomeBackHire_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";
                }
                else if (reportType == 35)
                {
                    reportData = GenerateWelcomeLetterWordDocument(letter);
                    fileName = $"WelcomeBackHire_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";
                }
                else
                {
                    continue; // Skip unsupported report types
                }

                // Add the file content and file name to the list
                filesList.Add(new Dictionary<string, object>
                {
                     { "FileContent", reportData },
                     { "FileName", fileName }
                });
            }

            // Prepare the final response
            var result = new Dictionary<string, object>
            {
                 { "Files", filesList }
            };

            return new GenericResponse<Dictionary<string, object>>(true, "Report(s) generated successfully.", result);
        }

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateExpungeLetterAsync(GenerateReportDto reportDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
            parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
            parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
            parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
            parameters.Add("@Type", reportDto.Type, DbType.Int32);

            // Call the repository to execute the stored procedure and fetch the data
            var expungeData = await _reportRepository.ExecuteStoredProcedureAsync("sp_ExpungeLatter_Report", parameters);

            // Check if expungeData is empty
            if (expungeData == null || !expungeData.Any())
            {
                return new GenericResponse<Dictionary<string, object>>(true, "No letters found.", null);
            }

            // Convert dynamic data to List<ExpungeLetter>
            var expungeLetters = new List<ExpungeLetter>();
            foreach (var item in expungeData)
            {
                ExpungeLetter expungeLetter = new ExpungeLetter
                {
                    FirstName = item.FirstName?.ToString(),
                    LastName = item.LastName?.ToString(),
                    Street = item.Street?.ToString(),
                    City = item.City?.ToString(),
                    State = item.State?.ToString(),
                    ZipCode = item.ZipCode?.ToString(),
                    DOB = DateTime.TryParse(item.DOB?.ToString(), out DateTime dob) ? (DateTime?)dob : null,
                    DistrictNameB = item.DistrictNameB?.ToString(),
                    SiteNameB = item.SiteNameB?.ToString(),
                    SitePermitB = item.SitePermitB?.ToString(),
                    DistrictNameD = item.DistrictNameD?.ToString(),
                    SiteNameD = item.SiteNameD?.ToString(),
                    SitePermitD = item.SitePermitD?.ToString(),
                    DistrictNameA = item.DistrictNameA?.ToString(),
                    SiteNameA = item.SiteNameA?.ToString(),
                    SitePermitA = item.SitePermitA?.ToString(),
                    SiteAddressB = item.SiteAddressB?.ToString(),
                    SiteAddressD = item.SiteAddressD?.ToString(),
                    SiteAddressA = item.SiteAddressA?.ToString(),
                    SiteBeforeClosed = item.SiteBeforeClosed?.ToString(),
                    SiteDuringClosed = item.SiteDuringClosed?.ToString(),
                    SiteAfterClosed = item.SiteAfterClosed?.ToString(),
                };

                expungeLetters.Add(expungeLetter);
            }

            // Initialize a list to store each generated document's content and file name
            var filesList = new List<Dictionary<string, object>>();

            foreach (var letter in expungeLetters)
            {
                // Generate the Word document for each letter
                var reportData = GenerateExpungeWordDocument(letter);
                var fileName = $"ExpungeLetter_{letter.FirstName}_{letter.LastName}_{DateTime.Now:yyyyMMddHHmmss}.docx";

                // Add file content and name to the list
                filesList.Add(new Dictionary<string, object>
                {
                     { "FileContent", reportData },
                     { "FileName", fileName }
                });
            }

            // Wrap the files list in a dictionary to match GenericResponse<Dictionary<string, object>> format
            var result = new Dictionary<string, object>
            {
                { "Files", filesList }
            };

            return new GenericResponse<Dictionary<string, object>>(true, "Letters generated successfully.", result);
        }


        string GetTemplatePath(Core.Enums.Templates tmp)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates");
            string templatePath = Path.GetFullPath(Path.Combine(basePath, tmp.ToString() + ".docx"));
            // string templatePath = Path.GetFullPath(Path.Combine(basePath, tmp.ToString() + ".dotx"));
            return templatePath;
        }


        private byte[] GenerateWordDocument(WelcomeLatter welcomeLetter)
        {
            // Load template file into a byte array
            string templatePath = GetTemplatePath(Core.Enums.Templates.SCOPETemplate_NewHireLetter);
            byte[] templateBytes = File.ReadAllBytes(templatePath);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(templateBytes, 0, templateBytes.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    if (mainPart == null) throw new InvalidOperationException("MainDocumentPart is missing");

                    // Get the document body
                    var body = mainPart.Document.Body;

                    if (!string.IsNullOrWhiteSpace(welcomeLetter.FirstName) || !string.IsNullOrWhiteSpace(welcomeLetter.LastName))
                    {

                        // Replace each placeholder with actual data
                        ReplacePlaceholder(body, "«LatterDate»", DateTime.Today.ToString("MMMM d, yyyy"));
                        ReplacePlaceholder(body, "«FULLNAME»", $"{welcomeLetter.FirstName} {welcomeLetter.LastName}");
                        ReplacePlaceholder(body, "«ADDRESS»", welcomeLetter.Street);
                        ReplacePlaceholder(body, "«CITY»", welcomeLetter.City);
                        ReplacePlaceholder(body, "«State»", welcomeLetter.State);
                        ReplacePlaceholder(body, "«ZipCode»", welcomeLetter.ZipCode);
                        ReplacePlaceholder(body, "«FIRSTNAME»", welcomeLetter.FirstName);
                        ReplacePlaceholder(body, "«Position»", GetPositionText(welcomeLetter));

                        // If multiple letters are to be added, handle each appropriately
                    }

                    mainPart.Document.Save();
                }

                return memoryStream.ToArray();
            }
        }
        private byte[] GenerateWelcomeLetterWordDocument(WelcomeLatter item)
        {
            // Load template file into a byte array
            string templatePath = GetTemplatePath(Core.Enums.Templates.SCOPETemplate_HireLetter);
            byte[] templateBytes = File.ReadAllBytes(templatePath);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(templateBytes, 0, templateBytes.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    if (mainPart == null) throw new InvalidOperationException("MainDocumentPart is missing");

                    // Get the document body
                    var body = mainPart.Document.Body;

                    // Process only a single WelcomeLatter item now
                    if (!string.IsNullOrWhiteSpace(item.FirstName) || !string.IsNullOrWhiteSpace(item.LastName))
                    {
                        // Replace each placeholder with actual data
                        ReplacePlaceholder(body, "«LatterDate»", DateTime.Today.ToString("MMMM d, yyyy"));
                        ReplacePlaceholder(body, "«FULLNAME»", $"{item.FirstName} {item.LastName}");
                        ReplacePlaceholder(body, "«ADDRESS»", item.Street);
                        ReplacePlaceholder(body, "«CITY»", item.City);
                        ReplacePlaceholder(body, "«State»", item.State);
                        ReplacePlaceholder(body, "«ZipCode»", item.ZipCode);
                        ReplacePlaceholder(body, "«FIRSTNAME»", item.FirstName);
                        ReplacePlaceholder(body, "«CPR»", item.CPR?.ToShortDateString() ?? "N/A");
                        ReplacePlaceholder(body, "«FirstAid»", item.FirstAid?.ToShortDateString() ?? "N/A");
                        ReplacePlaceholder(body, "«MAT»", item.MatDate?.ToShortDateString() ?? "N/A");
                        ReplacePlaceholder(body, "«Abuse»", item.MatApp?.ToShortDateString() ?? "N/A");
                        ReplacePlaceholder(body, "«Foundations»", item.Foundations?.ToShortDateString() ?? "N/A");

                        // Dynamic position text
                        string positionText = GetWelcomeLetterPositionText(item);
                        ReplacePlaceholder(body, "«Position»", positionText);
                    }

                    mainPart.Document.Save();
                }

                return memoryStream.ToArray();
            }
        }


        // Helper method to get the dynamic position text as in your legacy function
        private string GetWelcomeLetterPositionText(WelcomeLatter item)
        {
            var positionBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(item.SiteNameB))
            {
                string sepRateB = !string.IsNullOrEmpty(item.SEP_Rate_B) ? Decimal.Parse(item.SEP_Rate_B).ToString("N2") : "";
                string febRateB = !string.IsNullOrEmpty(item.FEB_Rate_B) ? Decimal.Parse(item.FEB_Rate_B).ToString("N2") : "";
                positionBuilder.AppendFormat("Position: {0}, District: {1}, Program: {2}\nRate of Pay: Sep: {3}, Feb: {4}\nSchedule: Mon: {5}-{6}, Tue: {7}-{8}, Wed: {9}-{10}, Thu: {11}-{12}, Fri: {13}-{14}\n\n",
                                             item.SitePositionB, item.DistrictNameB, item.SiteNameB, sepRateB, febRateB,
                                             item.MON_1_B, item.MON_1_E, item.TUE_1_B, item.TUE_1_E, item.WED_1_B, item.WED_1_E,
                                             item.THU_1_B, item.THU_1_E, item.FRI_1_B, item.FRI_1_E);
            }

            if (!string.IsNullOrWhiteSpace(item.SiteNameD))
            {
                string sepRateD = !string.IsNullOrEmpty(item.SEP_Rate_D) ? Decimal.Parse(item.SEP_Rate_D).ToString("N2") : "";
                string febRateD = !string.IsNullOrEmpty(item.FEB_Rate_D) ? Decimal.Parse(item.FEB_Rate_D).ToString("N2") : "";
                positionBuilder.AppendFormat("Position: {0}, District: {1}, Program: {2}\nRate of Pay: Sep: {3}, Feb: {4}\nSchedule: Mon: {5}-{6}, Tue: {7}-{8}, Wed: {9}-{10}, Thu: {11}-{12}, Fri: {13}-{14}\n\n",
                                             item.SitePositionD, item.DistrictNameD, item.SiteNameD, sepRateD, febRateD,
                                             item.MON_2_B, item.MON_2_E, item.TUE_2_B, item.TUE_2_E, item.WED_2_B, item.WED_2_E,
                                             item.THU_2_B, item.THU_2_E, item.FRI_2_B, item.FRI_2_E);
            }

            if (!string.IsNullOrWhiteSpace(item.SiteNameA))
            {
                string sepRateA = !string.IsNullOrEmpty(item.SEP_Rate_A) ? Decimal.Parse(item.SEP_Rate_A).ToString("N2") : "";
                string febRateA = !string.IsNullOrEmpty(item.FEB_Rate_A) ? Decimal.Parse(item.FEB_Rate_A).ToString("N2") : "";
                positionBuilder.AppendFormat("Position: {0}, District: {1}, Program: {2}\nRate of Pay: Sep: {3}, Feb: {4}\nSchedule: Mon: {5}-{6}, Tue: {7}-{8}, Wed: {9}-{10}, Thu: {11}-{12}, Fri: {13}-{14}\n\n",
                                             item.SitePositionA, item.DistrictNameA, item.SiteNameA, sepRateA, febRateA,
                                             item.MON_3_B, item.MON_3_E, item.TUE_3_B, item.TUE_3_E, item.WED_3_B, item.WED_3_E,
                                             item.THU_3_B, item.THU_3_E, item.FRI_3_B, item.FRI_3_E);
            }

            return positionBuilder.ToString();
        }

        private void ReplacePlaceholder(Body body, string placeholder, string replacementText)
        {
            // Accumulate text in runs to ensure we handle split text runs
            foreach (var run in body.Descendants<Run>())
            {
                string concatenatedText = string.Concat(run.Elements<Text>().Select(t => t.Text));
                if (concatenatedText.Contains(placeholder))
                {
                    foreach (var textElement in run.Elements<Text>())
                    {
                        textElement.Text = textElement.Text.Replace(placeholder, replacementText);
                    }
                }
            }
        }

        private string GetPositionText(WelcomeLatter item)
        {
            string Position = "", jan = "", feb = "", sep = "";

            // For Site B
            if (!string.IsNullOrEmpty(item.SiteNameB))
            {
                jan = !string.IsNullOrEmpty(item.JAN_Rate_B) ? decimal.Parse(item.JAN_Rate_B, NumberStyles.Currency).ToString("N2") : "";
                feb = !string.IsNullOrEmpty(item.FEB_Rate_B) ? decimal.Parse(item.FEB_Rate_B, NumberStyles.Currency).ToString("N2") : "";
                sep = !string.IsNullOrEmpty(item.SEP_Rate_B) ? decimal.Parse(item.SEP_Rate_B, NumberStyles.Currency).ToString("N2") : "";

                Position += $"Position: {item.SitePositionB}\t Initial District: {item.DistrictNameB}\r\n" +
                            //$"Rate of Pay: ${sep}\t 1/1/2019: ${jan}\t 2/1/2019: ${feb}\r\n" +
                            $"Initial location: {item.SiteNameB}\r\n" +
                            $"Date of Hire: {(item.DOEMP.HasValue ? item.DOEMP.Value.ToShortDateString() : "N/A")}\r\n";
            }

            // For Site D
            if (!string.IsNullOrEmpty(item.SiteNameD))
            {
                jan = !string.IsNullOrEmpty(item.JAN_Rate_D) ? decimal.Parse(item.JAN_Rate_D, NumberStyles.Currency).ToString("N2") : "";
                feb = !string.IsNullOrEmpty(item.FEB_Rate_D) ? decimal.Parse(item.FEB_Rate_D, NumberStyles.Currency).ToString("N2") : "";
                sep = !string.IsNullOrEmpty(item.SEP_Rate_D) ? decimal.Parse(item.SEP_Rate_D, NumberStyles.Currency).ToString("N2") : "";

                Position += $"Position: {item.SitePositionD}\t Initial District: {item.DistrictNameD}\r\n" +
                            //$"Rate of Pay: ${sep}\t 1/1/2019: ${jan}\t 2/1/2019: ${feb}\r\n" +
                            $"Initial location: {item.SiteNameD}\r\n" +
                            $"Date of Hire: {(item.DOEMP.HasValue ? item.DOEMP.Value.ToShortDateString() : "N/A")}\r\n";
            }

            // For Site A
            if (!string.IsNullOrEmpty(item.SiteNameA))
            {
                jan = !string.IsNullOrEmpty(item.JAN_Rate_A) ? decimal.Parse(item.JAN_Rate_A, NumberStyles.Currency).ToString("N2") : "";
                feb = !string.IsNullOrEmpty(item.FEB_Rate_A) ? decimal.Parse(item.FEB_Rate_A, NumberStyles.Currency).ToString("N2") : "";
                sep = !string.IsNullOrEmpty(item.SEP_Rate_A) ? decimal.Parse(item.SEP_Rate_A, NumberStyles.Currency).ToString("N2") : "";

                Position += $"Position: {item.SitePositionA}\t Initial District: {item.DistrictNameA}\r\n" +
                            //$"Rate of Pay: ${sep}\t 1/1/2019: ${jan}\t 2/1/2019: ${feb}\r\n" +
                            $"Initial location: {item.SiteNameA}\r\n" +
                            $"Date of Hire: {(item.DOEMP.HasValue ? item.DOEMP.Value.ToShortDateString() : "N/A")}\r\n";
            }

            // Add comments if available
            if (!string.IsNullOrEmpty(item.Comment))
            {
                Position += $"Comment: {item.Comment}\r\n";
            }

            return Position;
        }



        private byte[] GenerateExpungeWordDocument(ExpungeLetter expungeLetter)
        {
            // Load template file into a byte array
            string templatePath = GetTemplatePath(Core.Enums.Templates.SCOPETemplate_ExpungeLetter);
            byte[] templateBytes = File.ReadAllBytes(templatePath);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(templateBytes, 0, templateBytes.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    if (mainPart == null) throw new InvalidOperationException("MainDocumentPart is missing");

                    // Get the document body
                    var body = mainPart.Document.Body;


                    if (string.IsNullOrWhiteSpace(expungeLetter.FirstName) && string.IsNullOrWhiteSpace(expungeLetter.LastName))
                        return null;

                    // Replace placeholders with actual data
                    ReplacePlaceholder(body, "«LatterDate»", DateTime.Today.ToString("MMMM d, yyyy"));
                    ReplacePlaceholder(body, "«FullName»", $"{expungeLetter.FirstName} {expungeLetter.LastName} {expungeLetter.MI}");
                    ReplacePlaceholder(body, "«FullAddress»", $"{expungeLetter.Street}, {expungeLetter.City}, {expungeLetter.State}, {expungeLetter.ZipCode}");
                    ReplacePlaceholder(body, "«DOB»", expungeLetter.DOB?.ToShortDateString() ?? "N/A");
                    ReplacePlaceholder(body, "«Facility»", GetFacilityText(expungeLetter));

                    // Additional replacements can be done as needed


                    mainPart.Document.Save();
                }

                return memoryStream.ToArray();
            }
        }

        private string GetFacilityText(ExpungeLetter item)
        {
            string facilityText = "";

            if (!string.IsNullOrEmpty(item.SiteNameB) && string.IsNullOrEmpty(item.SiteBeforeClosed))
            {
                facilityText += $"Facility Name: {item.SiteNameB}\nPermit: {item.SitePermitB ?? "N/A"}\nFacility Address: {item.SiteAddressB}\n";
            }
            if (!string.IsNullOrEmpty(item.SiteNameD) && string.IsNullOrEmpty(item.SiteDuringClosed))
            {
                facilityText += $"Facility Name: {item.SiteNameD}\nPermit: {item.SitePermitD ?? "N/A"}\nFacility Address: {item.SiteAddressD}\n";
            }
            if (!string.IsNullOrEmpty(item.SiteNameA) && string.IsNullOrEmpty(item.SiteAfterClosed))
            {
                facilityText += $"Facility Name: {item.SiteNameA}\nPermit: {item.SitePermitA ?? "N/A"}\nFacility Address: {item.SiteAddressA}\n";
            }

            return facilityText;
        }


    }
}
