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

        public async Task<bool> CanViewReportDetailsAsync(User user, bool defaultStatus, ReportType? reportType, int? reportId)
        {
            if (defaultStatus || reportType == null || reportId == null) return defaultStatus;
            switch (reportType)
            {
                case ReportType.City:
                    var cityAnnualReport = await GetCertainCityAnnualReport(reportId);
                    var cityAnnualReportCityAdministration = await GetCertainCityAdministration(user, cityAnnualReport);
                    var regionOverCityAnnualReportRegionAdministration = await GetRegionOverCertainCityAdministration(cityAnnualReport, user);
                    return cityAnnualReportCityAdministration != null || regionOverCityAnnualReportRegionAdministration != null;
                case ReportType.Club:
                    var clubAnnualReport = await GetCertainClubAnnualReport(reportId);
                    var clubAnnualReportClubAdministration = await GetCertainClubAdministration(user, clubAnnualReport);
                    return clubAnnualReportClubAdministration != null;
                case ReportType.Region:
                    var regionAnnualReport = await GetCertainRegionAnnualReport(reportId);
                    var regionAnnualReportRegionAdministration = await GetCertainRegionAdministration(user, regionAnnualReport);
                    return regionAnnualReportRegionAdministration != null;
                default:
                    return defaultStatus;
            }
        }

        public async Task<bool> CanEditReportAsync(User user, bool defaultStatus, ReportType? reportType, int? reportId)
        {
            if (defaultStatus || reportType == null || reportId == null) return defaultStatus;
            switch (reportType)
            {
                case ReportType.City:
                    var cityAnnualReport = await GetCertainCityAnnualReport(reportId);
                    var cityAnnualReportCityAdministration = await GetCertainCityAdministration(user, cityAnnualReport);
                    return cityAnnualReportCityAdministration != null;
                case ReportType.Club:
                    var clubAnnualReport = await GetCertainClubAnnualReport(reportId);
                    var clubAnnualReportClubAdministration = await GetCertainClubAdministration(user, clubAnnualReport);
                    return clubAnnualReportClubAdministration != null;
                case ReportType.Region:
                    var regionAnnualReport = await GetCertainRegionAnnualReport(reportId);
                    var regionAnnualReportRegionAdministration =
                        await GetCertainRegionAdministration(user, regionAnnualReport);
                    return regionAnnualReportRegionAdministration != null;
                default:
                    return defaultStatus;
            }
        }

        private async Task<AnnualReport> GetCertainCityAnnualReport(int? reportId)
        {
            var certainCityAnnualReport =
                await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(x => x.ID == reportId);
            return certainCityAnnualReport;
        }

        private async Task<CityAdministration> GetCertainCityAdministration(User user, AnnualReport cityAnnualReport)
        {
            var certainCityAdministration = cityAnnualReport != null
                ? await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(x =>
                    x.CityId == cityAnnualReport.CityId &&
                    x.UserId == user.Id &&
                    x.AdminType.AdminTypeName != null &&
                    Roles.AdminAndCityHeadAndCityHeadDeputy.Contains(x.AdminType.AdminTypeName))
                : null;
            return certainCityAdministration;
        }

        private async Task<RegionAdministration> GetRegionOverCertainCityAdministration(AnnualReport cityAnnualReport, User user)
        {
            var certainCityUnderRegion = cityAnnualReport != null
                ? await _repositoryWrapper.RegionFollowers.GetFirstOrDefaultAsync(x => x.ID == cityAnnualReport.CityId)
                : null;
            var certainRegionAdministration =
                certainCityUnderRegion != null
                    ? await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(x =>
                        x.RegionId == certainCityUnderRegion.RegionId &&
                        x.UserId == user.Id &&
                        x.AdminType.AdminTypeName != null &&
                        Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy.Contains(x.AdminType.AdminTypeName))
                    : null;
            return certainRegionAdministration;
        }

        private async Task<ClubAnnualReport> GetCertainClubAnnualReport(int? reportId)
        {
            var certainClubAnnualReport =
                await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(x => x.ID == reportId);
            return certainClubAnnualReport;
        }

        private async Task<ClubAdministration> GetCertainClubAdministration(User user, ClubAnnualReport clubAnnualReport)
        {
            var certainClubAdministration = clubAnnualReport != null
                ? await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(x =>
                    x.ClubId == clubAnnualReport.ClubId &&
                    x.UserId == user.Id &&
                    x.AdminType.AdminTypeName != null &&
                    Roles.AdminAndKurinHeadAndKurinHeadDeputy.Contains(x.AdminType.AdminTypeName))
                : null;
            return certainClubAdministration;
        }

        private async Task<RegionAnnualReport> GetCertainRegionAnnualReport(int? reportId)
        {
            var certainRegionAnnualReport =
                await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(x => x.ID == reportId);
            return certainRegionAnnualReport;
        }

        private async Task<RegionAdministration> GetCertainRegionAdministration(User user,
            RegionAnnualReport regionAnnualReport)
        {
            var certainRegionAdministration = regionAnnualReport != null
                ? await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(x =>
                    x.RegionId == regionAnnualReport.RegionId &&
                    x.UserId == user.Id &&
                    x.AdminType.AdminTypeName != null &&
                    Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy.Contains(x.AdminType.AdminTypeName))
                : null;
            return certainRegionAdministration;
        }

      
    }
}