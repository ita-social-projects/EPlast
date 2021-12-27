using System.Threading.Tasks;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;

namespace EPlast.BLL.Services
{
    public class AnnualReportAccessService : IAnnualReportAccessService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AnnualReportAccessService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<bool> CanEditReportAsync(User user, bool defaultStatus, ReportType? reportType, int? reportId)
        {
            if (defaultStatus || reportType == null || reportId == null) return defaultStatus;
            switch (reportType)
            {
                case ReportType.City:
                    var annualReport =
                        await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(x =>
                            x.CreatorId == user.Id && x.ID == reportId);
                    return annualReport != null;
                case ReportType.Club:
                    var clubAnnualReport =
                        await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(x => x.ID == reportId);
                    var clubAnnualReportClubAdministration = clubAnnualReport != null
                        ? await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(x =>
                            x.ClubId == clubAnnualReport.ClubId && x.UserId == user.Id &&
                            x.AdminType.AdminTypeName != null &&
                            Roles.AdminAndKurinHeadAndKurinHeadDeputy.Contains(x.AdminType.AdminTypeName))
                        : null;
                    return clubAnnualReportClubAdministration != null;
                case ReportType.Region:
                    var regionAnnualReport =
                        await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(x => x.ID == reportId);
                    var regionAnnualReportRegionAdministration = regionAnnualReport != null
                        ? await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(x =>
                            x.RegionId == regionAnnualReport.ID && x.UserId == user.Id &&
                            x.AdminType.AdminTypeName != null &&
                            Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy.Contains(x.AdminType.AdminTypeName))
                        : null;
                    return regionAnnualReportRegionAdministration != null;
                default:
                    return defaultStatus;
            }
        }
    }
}