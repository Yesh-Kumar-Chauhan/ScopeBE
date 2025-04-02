using Core.DTOs.Report;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IReportService
    {
        //crud services
        Task<GenericResponse<IEnumerable<ReportDto>>> GetAllReportsAsync();
        Task<GenericResponse<ReportDto>> GetReportByIdAsync(long id);
        Task<GenericResponse<ReportDto>> CreateReportAsync(ReportDto reportDto);
        Task<GenericResponse<ReportDto>> UpdateReportAsync(long id, ReportDto reportDto);
        Task<GenericResponse<bool>> DeleteReportAsync(long id);

        //generate reports
        Task<GenericResponse<object>> GenerateReportAsync(GenerateReportDto reportDto);

    }
}
