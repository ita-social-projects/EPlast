using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAnnualReportAccessService
    {
        Task<bool> CanEditReportAsync(User user, bool defaultStatus, ReportType? reportType, int? reportId);
    }
}
