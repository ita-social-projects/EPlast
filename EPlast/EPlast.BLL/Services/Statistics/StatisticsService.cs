using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
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

        public async Task<CityStatistics> GetCityStatisticsAsync(int cityId, int year, IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            return await GetCityStatisticsAsync(cityId, year);
        }

        public async Task<CityStatistics> GetCityStatisticsAsync(int cityId, int minYear, int maxYear, IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            return await GetCityStatisticsAsync(cityId, minYear, maxYear);
        }

        public async Task<IEnumerable<CityStatistics>> GetCityStatisticsAsync(IEnumerable<int> citiesIds, int year, IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            var citiesStatistics = new List<CityStatistics>();
            foreach (var cityId in citiesIds)
            {
                citiesStatistics.Add(await GetCityStatisticsAsync(cityId, year));
            }
            return citiesStatistics;
        }

        public async Task<IEnumerable<CityStatistics>> GetCityStatisticsAsync(IEnumerable<int> citiesIds, int minYear, int maxYear, IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            var citiesStatistics = new List<CityStatistics>();
            foreach (var cityId in citiesIds)
            {
                citiesStatistics.Add(await GetCityStatisticsAsync(cityId, minYear, maxYear));
            }
            return citiesStatistics;
        }

        public async Task<RegionStatistics> GetRegionStatisticsAsync(int regionId, int year, IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            return await GetRegionStatisticsAsync(regionId, year);
        }

        public async Task<RegionStatistics> GetRegionStatisticsAsync(int regionId, int minYear, int maxYear, IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            return await GetRegionStatisticsAsync(regionId, minYear, maxYear);
        }

        public async Task<IEnumerable<RegionStatistics>> GetRegionStatisticsAsync(IEnumerable<int> regionsIds, int year, IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            var regionStatistics = new List<RegionStatistics>();
            foreach (var regionId in regionsIds)
            {
                regionStatistics.Add(await GetRegionStatisticsAsync(regionId, year));
            }
            return regionStatistics;
        }

        public async Task<IEnumerable<RegionStatistics>> GetRegionStatisticsAsync(IEnumerable<int> regionsIds, int minYear, int maxYear, IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            var regionStatistics = new List<RegionStatistics>();
            foreach (var regionId in regionsIds)
            {
                regionStatistics.Add(await GetRegionStatisticsAsync(regionId, minYear, maxYear));
            }
            return regionStatistics;
        }

        public async Task<IEnumerable<MembersStatistic>> GetAllCitiesStatisticsAsync()
        {
            var annualReports = await _repositoryWrapper.AnnualReports.GetAllAsync(
                    include: source => source
                        .Include(ar => ar.Creator)
                        .Include(ar => ar.MembersStatistic)
                        .Include(ar => ar.Date)
                        .Include(ar => ar.City)
                            .ThenInclude(c => c. Region));

            var membersStatistics = new List<MembersStatistic>();
            foreach (var report in annualReports)
            {
                membersStatistics.Add(report.MembersStatistic);
            }
            return membersStatistics;
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
                StatisticsItems = statisticsItems
            };
        }

        private YearStatistics GetYearStatistics(int year, IEnumerable<MembersStatistic> membersStatistic)
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
                StatisticsItems = statisticsItems
            };
        }

        private async Task<CityStatistics> GetCityStatisticsAsync(int cityId, int year)
        {
            var city = await _repositoryWrapper.City.GetFirstOrDefaultAsync(
                    predicate: c => c.ID == cityId,
                    include: source => source
                        .Include(c => c.Region)
                );
            var membersStatistics = await _repositoryWrapper.MembersStatistics.GetFirstOrDefaultAsync(
                    predicate: m => m.AnnualReport.CityId == city.ID && m.AnnualReport.Date.Year == year
                );
            return new CityStatistics
            {
                City = _mapper.Map<DatabaseEntities.City, DTOs.City>(city),
                YearStatistics = new List<YearStatistics>
                {
                    GetYearStatistics(year, membersStatistics)
                }
            };
        }

        private async Task<CityStatistics> GetCityStatisticsAsync(int cityId, int minYear, int maxYear)
        {
            var city = await _repositoryWrapper.City.GetFirstOrDefaultAsync(
                    predicate: c => c.ID == cityId,
                    include: source => source
                        .Include(c => c.Region)
                );
            var membersStatistics = await _repositoryWrapper.MembersStatistics.GetAllAsync(
                    predicate: m => m.AnnualReport.CityId == city.ID && m.AnnualReport.Date.Year >= minYear && m.AnnualReport.Date.Year <= maxYear,
                    include: source => source
                        .Include(m => m.AnnualReport)
                );
            var yearStatistics = new List<YearStatistics>();
            for (int year = minYear; year <= maxYear; year++)
            {
                var membersStatistic = membersStatistics.FirstOrDefault(m => m.AnnualReport.Date.Year == year);
                yearStatistics.Add(GetYearStatistics(year, membersStatistic));
            }
            return new CityStatistics
            {
                City = _mapper.Map<DatabaseEntities.City, DTOs.City>(city),
                YearStatistics = yearStatistics
            };
        }

        private async Task<RegionStatistics> GetRegionStatisticsAsync(int regionId, int year)
        {
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(
                    predicate: r => r.ID == regionId
                );
            var membersStatistics = await _repositoryWrapper.MembersStatistics.GetAllAsync(
                    predicate: m => m.AnnualReport.City.RegionId == regionId && m.AnnualReport.Date.Year == year
                );
            return new RegionStatistics
            {
                Region = _mapper.Map<DatabaseEntities.Region, DTOs.Region>(region),
                YearStatistics = new List<YearStatistics>
                {
                    GetYearStatistics(year, membersStatistics)
                }
            };
        }

        private async Task<RegionStatistics> GetRegionStatisticsAsync(int regionId, int minYear, int maxYear)
        {
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(
                    predicate: r => r.ID == regionId
                );
            var membersStatistics = await _repositoryWrapper.MembersStatistics.GetAllAsync(
                    predicate: m => m.AnnualReport.City.RegionId == regionId && m.AnnualReport.Date.Year >= minYear && m.AnnualReport.Date.Year <= maxYear,
                    include: source => source
                        .Include(m => m.AnnualReport)
                );
            var yearStatistics = new List<YearStatistics>();
            for (int year = minYear; year <= maxYear; year++)
            {
                var membersStatistic = membersStatistics.FirstOrDefault(m => m.AnnualReport.Date.Year == year);
                yearStatistics.Add(GetYearStatistics(year, membersStatistic));
            }
            return new RegionStatistics
            {
                Region = _mapper.Map<DatabaseEntities.Region, DTOs.Region>(region),
                YearStatistics = yearStatistics
            };
        }
    }
}