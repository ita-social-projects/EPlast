using EPlast.BLL.DTO.AnnualReport;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAnnualReportService
    {
        Task<AnnualReportDTO> GetByIdAsync(ClaimsPrincipal claimsPrincipal, int id);
        Task<IEnumerable<AnnualReportDTO>> GetAllAsync(ClaimsPrincipal claimsPrincipal);
        Task CreateAsync(ClaimsPrincipal claimsPrincipal, AnnualReportDTO annualReportDTO);
        Task EditAsync(ClaimsPrincipal claimsPrincipal, AnnualReportDTO annualReportDTO);
        Task ConfirmAsync(ClaimsPrincipal claimsPrincipal, int id);
        Task CancelAsync(ClaimsPrincipal claimsPrincipal, int id);
        Task DeleteAsync(ClaimsPrincipal claimsPrincipal, int id);
        Task<bool> HasUnconfirmedAsync(int cityId);
        Task<bool> HasCreatedAsync(int cityId);
        Task CheckCanBeCreatedAsync(int cityId);
    }
}