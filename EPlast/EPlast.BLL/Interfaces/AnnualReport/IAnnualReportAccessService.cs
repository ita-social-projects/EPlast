using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAnnualReportAccessService
    {
        Task<bool> CanEditReportAsync(User user, int reportType, int reportId);
    }
}
