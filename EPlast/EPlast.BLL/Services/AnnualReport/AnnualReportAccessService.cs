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

        public async Task<bool> CanViewReportDetailsAsync(User user, bool defaultStatus, ReportType? reportType,
            int? reportId)
        {
            if (defaultStatus || reportType == null || reportId == null) return defaultStatus;
            switch (reportType)
            {
                case ReportType.City:
                    var cityAnnualReport =
                        await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(x => x.ID == reportId);
                    var cityAnnualReportCityAdministration = cityAnnualReport != null
                        ? await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(x =>
                            x.CityId == cityAnnualReport.CityId && x.UserId == user.Id &&
                            x.AdminType.AdminTypeName != null &&
                            Roles.AdminAndCityHeadAndCityHeadDeputy.Contains(x.AdminType.AdminTypeName))
                        : null;
                    if (cityAnnualReportCityAdministration == null)
                    {
                        var cityUnderRegion = cityAnnualReport != null
                            ? await _repositoryWrapper.RegionFollowers.GetFirstOrDefaultAsync(x =>
                                x.ID == cityAnnualReport.CityId)
                            : null;
                        var regionOverCityAnnualReportRegionAdministration =
                            cityUnderRegion != null
                                ? await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(x =>
                                    x.RegionId == cityUnderRegion.RegionId && x.UserId == user.Id &&
                                    x.AdminType.AdminTypeName != null &&
                                    Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy.Contains(x.AdminType.AdminTypeName))
                                : null;
                        return regionOverCityAnnualReportRegionAdministration != null;
                    }

                    return cityAnnualReportCityAdministration != null;
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
                            x.RegionId == regionAnnualReport.RegionId && x.UserId == user.Id &&
                            x.AdminType.AdminTypeName != null &&
                            Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy.Contains(x.AdminType.AdminTypeName))
                        : null;
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
                    var cityAnnualReport =
                        await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(x => x.ID == reportId);
                    var cityAnnualReportCityAdministration = cityAnnualReport != null
                        ? await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(x =>
                            x.CityId == cityAnnualReport.CityId && x.UserId == user.Id &&
                            x.AdminType.AdminTypeName != null &&
                            Roles.AdminAndCityHeadAndCityHeadDeputy.Contains(x.AdminType.AdminTypeName))
                        : null;
                    return cityAnnualReportCityAdministration != null;
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