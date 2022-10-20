using System.Threading.Tasks;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services
{
    public class AnnualReportAccessService : IAnnualReportAccessService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;

        public AnnualReportAccessService(IRepositoryWrapper repositoryWrapper, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
        }

        public async Task<bool> CanViewReportDetailsAsync(User user, bool defaultStatus, ReportType? reportType, int? reportId)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            switch (reportType)
            {
                case ReportType.City:
                    var cityAnnualReport = await GetCertainCityAnnualReport(reportId);
                    var cityAnnualReportCityAdministration = await GetCertainCityAdministration(user, cityAnnualReport);
                    var regionOverCityAnnualReportRegionAdministration = await GetRegionOverCertainCityAdministration(cityAnnualReport, user);
                    if (cityAnnualReportCityAdministration != null || regionOverCityAnnualReportRegionAdministration != null ||
                       userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin))
                    {
                        return true;
                    }
                    else
                        return false;
                case ReportType.Club:
                    var clubAnnualReport = await GetCertainClubAnnualReport(reportId);
                    var clubAnnualReportClubAdministration = await GetCertainClubAdministration(user, clubAnnualReport);
                    if (clubAnnualReportClubAdministration != null ||
                       userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin))
                    {
                        return true;
                    }
                    else
                        return false;
                case ReportType.Region:
                    var regionAnnualReport = await GetCertainRegionAnnualReport(reportId);
                    var regionAnnualReportRegionAdministration = await GetCertainRegionAdministration(user, regionAnnualReport);
                    if (regionAnnualReportRegionAdministration != null ||
                         userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin))
                    {
                        return true;
                    }
                    else
                        return false;
                default:
                    return defaultStatus;
            }
        }

        public async Task<bool> CanEditReportAsync(User user, bool defaultStatus, ReportType? reportType, int? reportId)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            switch (reportType)
            {
                case ReportType.City:
                    var cityAnnualReport = await GetCertainCityAnnualReport(reportId);
                    var cityAnnualReportCityAdministration = await GetCertainCityAdministration(user, cityAnnualReport);
                    var regionOverCityAnnualReportRegionAdministration = await GetRegionOverCertainCityAdministration(cityAnnualReport, user);
                    if (((cityAnnualReportCityAdministration != null || regionOverCityAnnualReportRegionAdministration != null)
                         && user.Id == cityAnnualReport.CreatorId) ||
                         userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin)){
                        return true;
                    }
                    else
                        return false;
                case ReportType.Club:
                    var clubAnnualReport = await GetCertainClubAnnualReport(reportId);
                    var clubAnnualReportClubAdministration = await GetCertainClubAdministration(user, clubAnnualReport);
                    if (clubAnnualReportClubAdministration != null && user.Id == clubAnnualReport.CreatorId
                        && clubAnnualReportClubAdministration.ClubId == clubAnnualReport.ClubId ||
                         userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin))
                    {
                        return true;
                    }
                    else
                        return false;
                case ReportType.Region:
                    var regionAnnualReport = await GetCertainRegionAnnualReport(reportId);
                    var regionAnnualReportRegionAdministration = await GetCertainRegionAdministration(user, regionAnnualReport);
                    if (regionAnnualReportRegionAdministration != null && user.Id == regionAnnualReport.CreatorId
                        && regionAnnualReportRegionAdministration.RegionId == regionAnnualReport.RegionId  ||
                         userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin)) {
                        return true;
                    }
                    else
                        return false;
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
                    x.Status &&
                    x.AdminType.AdminTypeName != null &&
                    Roles.AdminAndCityHeadAndCityHeadDeputy.Contains(x.AdminType.AdminTypeName))
                : null;
            return certainCityAdministration;
        }

        private async Task<RegionAdministration> GetRegionOverCertainCityAdministration(AnnualReport cityAnnualReport, User user)
        {
            var certainCityUnderRegion = cityAnnualReport != null
                ? await _repositoryWrapper.City.GetFirstOrDefaultAsync(x => x.ID == cityAnnualReport.CityId)
                : null;

            var certainRegionAdministration =
                certainCityUnderRegion != null
                    ? await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(x =>
                        x.RegionId == certainCityUnderRegion.RegionId &&
                        x.UserId == user.Id &&
                        x.Status &&
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
                    x.Status &&
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

        private async Task<DataAccess.Entities.RegionAdministration> GetCertainRegionAdministration(User user,
            RegionAnnualReport regionAnnualReport)
        {
            var certainRegionAdministration = regionAnnualReport != null
                ? await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(x =>
                    x.RegionId == regionAnnualReport.RegionId &&
                    x.UserId == user.Id &&
                    x.Status &&
                    x.AdminType.AdminTypeName != null &&
                    Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy.Contains(x.AdminType.AdminTypeName))
                : null;
            return certainRegionAdministration;
        }

      
    }
}