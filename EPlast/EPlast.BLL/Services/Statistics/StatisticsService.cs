using AutoMapper;
using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Interfaces.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;
using DTOs = EPlast.BLL.DTO.Statistics;

namespace EPlast.BLL.Services.Statistics
{
    public class StatisticsService : ICityStatisticsService, IRegionStatisticsService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        private readonly Dictionary<StatisticsItemIndicator, IStatisticsItem> _minorStatisticsItems;
        private readonly Dictionary<StatisticsItemIndicator, MajorStatisticsItem> _majorStatisticsItems;

        public StatisticsService(IRepositoryWrapper repositoryWrapper, StatisticsServiceSettings settings, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _minorStatisticsItems = settings.GetMinorItems();
            _majorStatisticsItems = settings.GetMajorItems();
        }

        
        public async Task<IEnumerable<CityStatistics>> GetCitiesStatisticsAsync(IEnumerable<int> cityIds,
                                                                                IEnumerable<int> years,
                                                                                IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            var citiesStatistics = new List<CityStatistics>();
            foreach (var cityId in cityIds)
            {
                citiesStatistics.Add(await GetCityStatisticsAsync(cityId, years));
            }
            return citiesStatistics.OrderBy(x => x.City.Name);
        }
        
        public async Task<IEnumerable<RegionStatistics>> GetRegionsStatisticsAsync(IEnumerable<int> regionIds,
                                                                                   IEnumerable<int> years,
                                                                                   IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            var regionStatistics = new List<RegionStatistics>();
            foreach (var regionId in regionIds)
            {
                regionStatistics.Add(await GetRegionStatisticsAsync(regionId, years));
            }
            return regionStatistics;
        }

        private async Task<CityStatistics> GetCityStatisticsAsync(int cityId, IEnumerable<int> years)
        {
            var city = await _repositoryWrapper.City.GetFirstOrDefaultAsync(
                    predicate: c => c.ID == cityId,
                    include: source => source
                        .Include(c => c.Region));
            var membersStatistics = await _repositoryWrapper.MembersStatistics.GetAllAsync(
                    predicate: m => m.AnnualReport.CityId == city.ID && years.Contains(m.AnnualReport.Date.Year),
                    include: source => source
                        .Include(m => m.AnnualReport));
            var yearStatistics = new List<YearStatistics>();

            foreach (var year in years)
            {
                var cityYearStats = membersStatistics.FirstOrDefault(m =>
                    m.AnnualReport.Date.Year == year &&
                    m.AnnualReport.Status != AnnualReportStatus.Unconfirmed);

                if (cityYearStats == null) continue;

                var cityYearMemberStats = GetYearStatistics(year, cityYearStats);
                yearStatistics.Add(cityYearMemberStats);
            }
            return new CityStatistics
            {
                City = _mapper.Map<DatabaseEntities.City, DTOs.City>(city),
                YearStatistics = yearStatistics.OrderBy(x => x.Year)
            };
        }

        private async Task<RegionStatistics> GetRegionStatisticsAsync(int regionId, IEnumerable<int> years)
        {
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(
                    predicate: r => r.ID == regionId);
            var regionsAnnualReports = await _repositoryWrapper.RegionAnnualReports.GetAllAsync(
                    predicate: m => m.RegionId == regionId && years.Contains(m.Date.Year));
            var yearStatistics = new List<YearStatistics>();

            foreach (var year in years)
            {
                var annualReport = regionsAnnualReports.FirstOrDefault(report =>
                    report.Date.Year == year &&
                    report.Status != AnnualReportStatus.Unconfirmed);

                if (annualReport == null) continue;

                var yearMemberStats = GetYearStatistics(year, GetMembersStatisticAsync(annualReport));
                yearStatistics.Add(yearMemberStats);
            }
            return new RegionStatistics
            {
                Region = _mapper.Map<DatabaseEntities.Region, DTOs.Region>(region),
                YearStatistics = yearStatistics.OrderBy(x => x.Year)
            };
        }

        private void SelectStatisticsItems(IEnumerable<StatisticsItemIndicator> indicators)
        {
            foreach (var key in _minorStatisticsItems.Keys)
            {
                if (!indicators.Contains(key))
                {
                    _minorStatisticsItems.Remove(key);
                }
            }
            foreach (var key in _majorStatisticsItems.Keys)
            {
                if (!indicators.Contains(key))
                {
                    _majorStatisticsItems.Remove(key);
                }
                else
                {
                    _majorStatisticsItems[key].RemoveMinors(_minorStatisticsItems);
                }
            }
        }

        private MembersStatistic GetMembersStatisticAsync(RegionAnnualReport regionAnnualReport)
        {
            if(regionAnnualReport != null)
            {
                return new MembersStatistic()
                {
                    NumberOfPtashata = regionAnnualReport.NumberOfPtashata,
                    NumberOfNovatstva = regionAnnualReport.NumberOfNovatstva,
                    NumberOfUnatstvaNoname = regionAnnualReport.NumberOfUnatstvaNoname,
                    NumberOfUnatstvaSupporters = regionAnnualReport.NumberOfUnatstvaSupporters,
                    NumberOfUnatstvaMembers = regionAnnualReport.NumberOfUnatstvaMembers,
                    NumberOfUnatstvaProspectors = regionAnnualReport.NumberOfUnatstvaProspectors,
                    NumberOfUnatstvaSkobVirlyts = regionAnnualReport.NumberOfUnatstvaSkobVirlyts,
                    NumberOfSeniorPlastynSupporters = regionAnnualReport.NumberOfSeniorPlastynSupporters,
                    NumberOfSeniorPlastynMembers = regionAnnualReport.NumberOfSeniorPlastynMembers,
                    NumberOfSeigneurSupporters = regionAnnualReport.NumberOfSeigneurSupporters,
                    NumberOfSeigneurMembers = regionAnnualReport.NumberOfSeigneurMembers
                };
            }
            return new MembersStatistic();
        }

        private YearStatistics GetYearStatistics(int year, MembersStatistic membersStatistic)
        {
            var statisticsItems = new List<StatisticsItem>();
            foreach (var key in _minorStatisticsItems.Keys)
            {
                statisticsItems.Add(_minorStatisticsItems[key].GetValue(membersStatistic));
            }
            foreach (var key in _majorStatisticsItems.Keys)
            {
                statisticsItems.Add(_majorStatisticsItems[key].GetValue(membersStatistic));
            }
            return new YearStatistics
            {
                Year = year,
                StatisticsItems = statisticsItems.OrderBy(x => x.Indicator)
            };
        }
    }
}