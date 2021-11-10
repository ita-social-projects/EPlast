using EPlast.BLL.DTO.AnnualReport;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAnnualReportAccessService
    {
        Task<bool> CanEditCityReportAsync(string userId, int reportId);
    }
}