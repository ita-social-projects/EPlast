using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Repositories;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class AnnualReportAccessService : IAnnualReportAccessService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AnnualReportAccessService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<bool> CanEditCityReportAsync(string userId, int reportId)
        {
            var annualReport = await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(x => (x.CreatorId == userId && x.ID == reportId));
            return annualReport != null;
        }
    }
}
