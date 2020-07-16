using AutoMapper;
using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using DatabaseEntities = EPlast.DataAccess.Entities;
using DTOs = EPlast.BLL.DTO.Statistics;

namespace EPlast.XUnitTest.Services.Statistics
{
    public class StatisticsServiceTests
    {
        private readonly StatisticsService _statisticsService;

        private readonly Mock<IRepositoryWrapper> _repositoryWrapper = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        private readonly IEnumerable<Region> _regions = new List<Region>
        {
            new Region { ID = 1, RegionName = "Львівський" },
            new Region { ID = 2, RegionName = "Тернопільський" }
        };

        private readonly IEnumerable<DTOs.City> _cities = new List<DTOs.City>
        {
            new DTOs.City { ID = 1, Name = "Золочів" },
            new DTOs.City { ID = 2, Name = "Перемишляни" }
        };

        private readonly IEnumerable<StatisticsItemIndicator> _indicators = new List<StatisticsItemIndicator>
        {
            StatisticsItemIndicator.NumberOfNovatstva, StatisticsItemIndicator.NumberOfUnatstva, StatisticsItemIndicator.NumberOfUnatstvaMembers,
            StatisticsItemIndicator.NumberOfUnatstvaSkobVirlyts, StatisticsItemIndicator.NumberOfSeigneurMembers, StatisticsItemIndicator.NumberOfSeigneurSupporters,
            StatisticsItemIndicator.NumberOfSenior
        };

        private readonly IEnumerable<StatisticsItem> _expectedStatisticsItems = new List<StatisticsItem>
        {
            new StatisticsItem { Indicator = StatisticsItemIndicator.NumberOfNovatstva },
            new StatisticsItem { Indicator = StatisticsItemIndicator.NumberOfSeigneurMembers },
            new StatisticsItem { Indicator = StatisticsItemIndicator.NumberOfSeigneurSupporters },
            new StatisticsItem { Indicator = StatisticsItemIndicator.NumberOfUnatstva },
            new StatisticsItem { Indicator = StatisticsItemIndicator.NumberOfSenior }
        };

        public StatisticsServiceTests()
        {
            _statisticsService = new StatisticsService(_repositoryWrapper.Object, new StatisticsServiceSettings(), _mapper.Object);
        }

        [Fact]
        public async Task GetCityStatistics()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.MembersStatistic());
            _mapper.Setup(m => m.Map<DatabaseEntities.City, DTOs.City>(It.IsAny<DatabaseEntities.City>()))
                .Returns(_cities.First());
            int year = 2019;
            var expectedResult = new CityStatistics
            {
                City = _cities.First(),
                YearStatistics = new List<YearStatistics>
                {
                    new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                }
            };

            // Act
            var result = await _statisticsService.GetCityStatisticsAsync(It.IsAny<int>(), year, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetCityStatisticsCityNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync((DatabaseEntities.City)null);

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _statisticsService.GetCityStatisticsAsync(It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<IEnumerable<StatisticsItemIndicator>>()));
            _repositoryWrapper.Verify(r => r.MembersStatistics.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()), Times.Never);
        }

        [Fact]
        public async Task GetCityStatisticsStatisticsNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync((DatabaseEntities.MembersStatistic)null);
            _mapper.Setup(m => m.Map<DatabaseEntities.City, DTOs.City>(It.IsAny<DatabaseEntities.City>()))
                .Returns(_cities.First());
            int year = 2019;
            var expectedResult = new CityStatistics
            {
                City = _cities.First(),
                YearStatistics = new List<YearStatistics>
                {
                    new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                }
            };

            // Act
            var result = await _statisticsService.GetCityStatisticsAsync(It.IsAny<int>(), year, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetCityStatisticsPeriod()
        {
            // Arrange
            var memberStatistics = new List<DatabaseEntities.MembersStatistic>
            {
                new DatabaseEntities.MembersStatistic { AnnualReport = new DatabaseEntities.AnnualReport { Date = new DateTime(2019, 1, 1) } },
                new DatabaseEntities.MembersStatistic { AnnualReport = new DatabaseEntities.AnnualReport { Date = new DateTime(2020, 1, 1) } },
            };
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(memberStatistics);
            _mapper.Setup(m => m.Map<DatabaseEntities.City, DTOs.City>(It.IsAny<DatabaseEntities.City>()))
                .Returns(_cities.First());
            int minYear = 2019;
            int maxYear = 2020;
            var expectedResult = new CityStatistics
            {
                City = _cities.First(),
                YearStatistics = new List<YearStatistics>
                {
                    new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                    new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                }
            };

            // Act
            var result = await _statisticsService.GetCityStatisticsAsync(It.IsAny<int>(), minYear, maxYear, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetCityStatisticsPeriodCityNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync((DatabaseEntities.City)null);

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _statisticsService.GetCityStatisticsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<IEnumerable<StatisticsItemIndicator>>()));
            _repositoryWrapper.Verify(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()), Times.Never);
        }

        [Fact]
        public async Task GetCityStatisticsPeriodStatisticsNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(Enumerable.Empty<DatabaseEntities.MembersStatistic>());
            _mapper.Setup(m => m.Map<DatabaseEntities.City, DTOs.City>(It.IsAny<DatabaseEntities.City>()))
                .Returns(_cities.First());
            int minYear = 2019;
            int maxYear = 2020;
            var expectedResult = new CityStatistics
            {
                City = _cities.First(),
                YearStatistics = new List<YearStatistics>
                {
                    new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                    new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                }
            };

            // Act
            var result = await _statisticsService.GetCityStatisticsAsync(It.IsAny<int>(), minYear, maxYear, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetCitiesStatistics()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.MembersStatistic());
            _mapper.SetupSequence(m => m.Map<DatabaseEntities.City, DTOs.City>(It.IsAny<DatabaseEntities.City>()))
                .Returns(_cities.ToList()[0])
                .Returns(_cities.ToList()[1]);
            int year = 2019;
            var expectedResult = new List<CityStatistics>
            {
                new CityStatistics
                {
                    City = _cities.ToList()[0],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                    }
                },
                new CityStatistics
                {
                    City = _cities.ToList()[1],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                    }
                }
            };

            // Act
            var result = await _statisticsService.GetCityStatisticsAsync(new int[2], year, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetCitiesStatisticsCityNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync((DatabaseEntities.City)null);

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _statisticsService.GetCityStatisticsAsync(new int[2], It.IsAny<int>(),
                It.IsAny<IEnumerable<StatisticsItemIndicator>>()));
            _repositoryWrapper.Verify(r => r.MembersStatistics.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()), Times.Never);
        }

        [Fact]
        public async Task GetCitiesStatisticsStatisticsNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.MembersStatistic());
            _mapper.SetupSequence(m => m.Map<DatabaseEntities.City, DTOs.City>(It.IsAny<DatabaseEntities.City>()))
                .Returns(_cities.ToList()[0])
                .Returns(_cities.ToList()[1]);
            int year = 2019;
            var expectedResult = new List<CityStatistics>
            {
                new CityStatistics
                {
                    City = _cities.ToList()[0],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                    }
                },
                new CityStatistics
                {
                    City = _cities.ToList()[1],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                    }
                }
            };

            // Act
            var result = await _statisticsService.GetCityStatisticsAsync(new int[2], year, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetCitiesStatisticsPeriod()
        {
            // Arrange
            var memberStatistics = new List<DatabaseEntities.MembersStatistic>
            {
                new DatabaseEntities.MembersStatistic { AnnualReport = new DatabaseEntities.AnnualReport { Date = new DateTime(2019, 1, 1) } },
                new DatabaseEntities.MembersStatistic { AnnualReport = new DatabaseEntities.AnnualReport { Date = new DateTime(2020, 1, 1) } },
            };
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(memberStatistics);
            _mapper.SetupSequence(m => m.Map<DatabaseEntities.City, DTOs.City>(It.IsAny<DatabaseEntities.City>()))
                .Returns(_cities.ToList()[0])
                .Returns(_cities.ToList()[1]);
            int minYear = 2019;
            int maxYear = 2020;
            var expectedResult = new List<CityStatistics>
            {
                new CityStatistics
                {
                    City = _cities.ToList()[0],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                        new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                    }
                },
                new CityStatistics
                {
                    City = _cities.ToList()[1],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                        new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                    }
                }
            };

            // Act
            var result = await _statisticsService.GetCityStatisticsAsync(new int[2], minYear, maxYear, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetCitiesStatisticsPeriodCityNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync((DatabaseEntities.City)null);

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _statisticsService.GetCityStatisticsAsync(new int[2], It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<IEnumerable<StatisticsItemIndicator>>()));
            _repositoryWrapper.Verify(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()), Times.Never);
        }

        [Fact]
        public async Task GetCitiesStatisticsPeriodStatisticsNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(Enumerable.Empty<DatabaseEntities.MembersStatistic>());
            _mapper.SetupSequence(m => m.Map<DatabaseEntities.City, DTOs.City>(It.IsAny<DatabaseEntities.City>()))
                .Returns(_cities.ToList()[0])
                .Returns(_cities.ToList()[1]);
            int minYear = 2019;
            int maxYear = 2020;
            var expectedResult = new List<CityStatistics>
            {
                new CityStatistics
                {
                    City = _cities.ToList()[0],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                        new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                    }
                },
                new CityStatistics
                {
                    City = _cities.ToList()[1],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                        new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                    }
                }
            };

            // Act
            var result = await _statisticsService.GetCityStatisticsAsync(new int[2], minYear, maxYear, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetRegionStatistics()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.Region());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(new List<DatabaseEntities.MembersStatistic> { new DatabaseEntities.MembersStatistic() });
            _mapper.Setup(m => m.Map<DatabaseEntities.Region, DTOs.Region>(It.IsAny<DatabaseEntities.Region>()))
                .Returns(_regions.First());
            int year = 2019;
            var expectedResult = new RegionStatistics
            {
                Region = _regions.First(),
                YearStatistics = new List<YearStatistics>
                {
                    new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                }
            };

            // Act
            var result = await _statisticsService.GetRegionStatisticsAsync(It.IsAny<int>(), year, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetRegionStatisticsRegionNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync((DatabaseEntities.Region)null);

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _statisticsService.GetRegionStatisticsAsync(It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<IEnumerable<StatisticsItemIndicator>>()));
            _repositoryWrapper.Verify(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()), Times.Never);
        }

        [Fact]
        public async Task GetRegionStatisticsStatisticsNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.Region());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(Enumerable.Empty<DatabaseEntities.MembersStatistic>());
            _mapper.Setup(m => m.Map<DatabaseEntities.Region, DTOs.Region>(It.IsAny<DatabaseEntities.Region>()))
                .Returns(_regions.First());
            int year = 2019;
            var expectedResult = new RegionStatistics
            {
                Region = _regions.First(),
                YearStatistics = new List<YearStatistics>
                {
                    new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                }
            };

            // Act
            var result = await _statisticsService.GetRegionStatisticsAsync(It.IsAny<int>(), year, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetRegionStatisticsPeriod()
        {
            // Arrange
            var memberStatistics = new List<DatabaseEntities.MembersStatistic>
            {
                new DatabaseEntities.MembersStatistic { AnnualReport = new DatabaseEntities.AnnualReport { Date = new DateTime(2019, 1, 1) } },
                new DatabaseEntities.MembersStatistic { AnnualReport = new DatabaseEntities.AnnualReport { Date = new DateTime(2020, 1, 1) } },
            };
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.Region());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(memberStatistics);
            _mapper.Setup(m => m.Map<DatabaseEntities.Region, DTOs.Region>(It.IsAny<DatabaseEntities.Region>()))
                .Returns(_regions.First());
            int minYear = 2019;
            int maxYear = 2020;
            var expectedResult = new RegionStatistics
            {
                Region = _regions.First(),
                YearStatistics = new List<YearStatistics>
                {
                    new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                    new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                }
            };

            // Act
            var result = await _statisticsService.GetRegionStatisticsAsync(It.IsAny<int>(), minYear, maxYear, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetRegionStatisticsPeriodRegionNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync((DatabaseEntities.Region)null);

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _statisticsService.GetRegionStatisticsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<IEnumerable<StatisticsItemIndicator>>()));
            _repositoryWrapper.Verify(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()), Times.Never);
        }

        [Fact]
        public async Task GetRegionStatisticsPeriodStatisticsNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.Region());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(Enumerable.Empty<DatabaseEntities.MembersStatistic>());
            _mapper.Setup(m => m.Map<DatabaseEntities.Region, DTOs.Region>(It.IsAny<DatabaseEntities.Region>()))
                .Returns(_regions.First());
            int minYear = 2019;
            int maxYear = 2020;
            var expectedResult = new RegionStatistics
            {
                Region = _regions.First(),
                YearStatistics = new List<YearStatistics>
                {
                    new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                    new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                }
            };

            // Act
            var result = await _statisticsService.GetRegionStatisticsAsync(It.IsAny<int>(), minYear, maxYear, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetRegionsStatistics()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.Region());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(new List<DatabaseEntities.MembersStatistic> { new DatabaseEntities.MembersStatistic() });
            _mapper.SetupSequence(m => m.Map<DatabaseEntities.Region, DTOs.Region>(It.IsAny<DatabaseEntities.Region>()))
                .Returns(_regions.ToList()[0])
                .Returns(_regions.ToList()[1]);
            int year = 2019;
            var expectedResult = new List<RegionStatistics>
            {
                new RegionStatistics
                {
                    Region = _regions.ToList()[0],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                    }
                },
                new RegionStatistics
                {
                    Region = _regions.ToList()[1],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                    }
                }
            };

            // Act
            var result = await _statisticsService.GetRegionStatisticsAsync(new int[2], year, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetRegionsStatisticsRegionNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync((DatabaseEntities.Region)null);

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _statisticsService.GetRegionStatisticsAsync(new int[2], It.IsAny<int>(),
                It.IsAny<IEnumerable<StatisticsItemIndicator>>()));
            _repositoryWrapper.Verify(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()), Times.Never);
        }

        [Fact]
        public async Task GetRegionsStatisticsStatisticsNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.Region());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(Enumerable.Empty<DatabaseEntities.MembersStatistic>());
            _mapper.SetupSequence(m => m.Map<DatabaseEntities.Region, DTOs.Region>(It.IsAny<DatabaseEntities.Region>()))
                .Returns(_regions.ToList()[0])
                .Returns(_regions.ToList()[1]);
            int year = 2019;
            var expectedResult = new List<RegionStatistics>
            {
                new RegionStatistics
                {
                    Region = _regions.ToList()[0],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                    }
                },
                new RegionStatistics
                {
                    Region = _regions.ToList()[1],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = year, StatisticsItems = _expectedStatisticsItems }
                    }
                }
            };

            // Act
            var result = await _statisticsService.GetRegionStatisticsAsync(new int[2], year, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetRegionsStatisticsPeriod()
        {
            // Arrange
            var memberStatistics = new List<DatabaseEntities.MembersStatistic>
            {
                new DatabaseEntities.MembersStatistic { AnnualReport = new DatabaseEntities.AnnualReport { Date = new DateTime(2019, 1, 1) } },
                new DatabaseEntities.MembersStatistic { AnnualReport = new DatabaseEntities.AnnualReport { Date = new DateTime(2020, 1, 1) } },
            };
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.Region());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(memberStatistics);
            _mapper.SetupSequence(m => m.Map<DatabaseEntities.Region, DTOs.Region>(It.IsAny<DatabaseEntities.Region>()))
                .Returns(_regions.ToList()[0])
                .Returns(_regions.ToList()[1]);
            int minYear = 2019;
            int maxYear = 2020;
            var expectedResult = new List<RegionStatistics>
            {
                new RegionStatistics
                {
                    Region = _regions.ToList()[0],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                        new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                    }
                },
                new RegionStatistics
                {
                    Region = _regions.ToList()[1],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                        new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                    }
                }
            };

            // Act
            var result = await _statisticsService.GetRegionStatisticsAsync(new int[2], minYear, maxYear, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetRegionsStatisticsPeriodRegionNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync((DatabaseEntities.Region)null);

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _statisticsService.GetRegionStatisticsAsync(new int[2], It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<IEnumerable<StatisticsItemIndicator>>()));
            _repositoryWrapper.Verify(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()), Times.Never);
        }

        [Fact]
        public async Task GetRegionsStatisticsPeriodStatisticsNotFound()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Region>, IIncludableQueryable<DatabaseEntities.Region, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.Region());
            _repositoryWrapper.Setup(r => r.MembersStatistics.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.MembersStatistic>, IIncludableQueryable<DatabaseEntities.MembersStatistic, object>>>()))
                    .ReturnsAsync(Enumerable.Empty<DatabaseEntities.MembersStatistic>());
            _mapper.SetupSequence(m => m.Map<DatabaseEntities.Region, DTOs.Region>(It.IsAny<DatabaseEntities.Region>()))
                .Returns(_regions.ToList()[0])
                .Returns(_regions.ToList()[1]);
            int minYear = 2019;
            int maxYear = 2020;
            var expectedResult = new List<RegionStatistics>
            {
                new RegionStatistics
                {
                    Region = _regions.ToList()[0],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                        new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                    }
                },
                new RegionStatistics
                {
                    Region = _regions.ToList()[1],
                    YearStatistics = new List<YearStatistics>
                    {
                        new YearStatistics { Year = minYear, StatisticsItems = _expectedStatisticsItems },
                        new YearStatistics { Year = maxYear, StatisticsItems = _expectedStatisticsItems }
                    }
                }
            };

            // Act
            var result = await _statisticsService.GetRegionStatisticsAsync(new int[2], minYear, maxYear, _indicators);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }
    }
}