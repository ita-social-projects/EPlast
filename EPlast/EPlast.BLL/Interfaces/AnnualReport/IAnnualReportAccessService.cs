using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAnnualReportAccessService
    {
        Task<bool> CanEditCityReportAsync(string userId, int reportId);
    }
}
