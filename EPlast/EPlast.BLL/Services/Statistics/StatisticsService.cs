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

        
        public async Task<IEnumerable<CityStatistics>> GetCityStatisticsAsync(IEnumerable<int> citiesIds,
                                                                              IEnumerable<int> years,
                                                                              IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            var citiesStatistics = new List<CityStatistics>();
            foreach (var cityId in citiesIds)
            {
                citiesStatistics.Add(await GetCityStatisticsAsync(cityId, years));
            }
            return citiesStatistics.OrderBy(x => x.City.Name);
        }
        
        public async Task<IEnumerable<RegionStatistics>> GetRegionStatisticsAsync(IEnumerable<int> regionsIds,
                                                                                  IEnumerable<int> years,
                                                                                  IEnumerable<StatisticsItemIndicator> indicators)
        {
            SelectStatisticsItems(indicators);
            var regionStatistics = new List<RegionStatistics>();
            foreach (var regionId in regionsIds)
            {
                regionStatistics.Add(await GetRegionStatisticsAsync(regionId, years));
            }
            return regionStatistics;
        }
        
        private async Task<RegionStatistics> GetRegionStatisticsAsync(int regionId, IEnumerable<int> years)
        {
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(
                    predicate: r => r.ID == regionId
                );
            var membersStatistics = await _repositoryWrapper.MembersStatistics.GetAllAsync(
                    predicate: m => m.AnnualReport.City.RegionId == regionId && years.Contains(m.AnnualReport.Date.Year),
                    include: source => source
                        .Include(m => m.AnnualReport)
                );
            var yearStatistics = new List<YearStatistics>();
            foreach (var year in years)
            {
                var membersStatistic = membersStatistics.First(m => m.AnnualReport.Date.Year == year);
                yearStatistics.Add(GetYearStatistics(year, membersStatistic));
            }
            return new RegionStatistics
            {
                Region = _mapper.Map<DatabaseEntities.Region, DTOs.Region>(region),
                YearStatistics = yearStatistics.OrderBy(x => x.Year)
            };
        }

        private async Task<CityStatistics> GetCityStatisticsAsync(int cityId, IEnumerable<int> years)
        {
            var city = await _repositoryWrapper.City.GetFirstOrDefaultAsync(
                    predicate: c => c.ID == cityId,
                    include: source => source
                        .Include(c => c.Region)
                );
            var membersStatistics = await _repositoryWrapper.MembersStatistics.GetAllAsync(
                    predicate: m => m.AnnualReport.CityId == city.ID && years.Contains(m.AnnualReport.Date.Year),
                    include: source => source
                        .Include(m => m.AnnualReport)
                );
            var yearStatistics = new List<YearStatistics>();
            foreach (var year in years)
            {
                var membersStatistic = membersStatistics.FirstOrDefault(m => m.AnnualReport.Date.Year == year);
                yearStatistics.Add(GetYearStatistics(year, membersStatistic));
            }
            return new CityStatistics
            {
                City = _mapper.Map<DatabaseEntities.City, DTOs.City>(city),
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