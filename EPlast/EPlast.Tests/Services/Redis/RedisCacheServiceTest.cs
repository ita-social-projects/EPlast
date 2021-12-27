using EPlast.BLL.Interfaces.Cache;
using EPlast.BLL.Services.Redis;
using EPlast.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Redis
{
    [TestFixture]
    class RedisCacheServiceTest
    {
        private Mock<IConnectionMultiplexer> _connectionMultiplexer;
        private Mock<IDatabase> _db;
        private Mock<IServer> _server;
        private Mock<IConfiguration> _configuration;

        private RedisCacheService _redisCacheService;

        [SetUp]
        public void SetUp()
        {
            _connectionMultiplexer = new Mock<IConnectionMultiplexer>();
            _db = new Mock<IDatabase>();
            _connectionMultiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_db.Object);
            _server = new Mock<IServer>();
            _connectionMultiplexer.Setup(x => x.GetServer(It.IsAny<string>(), It.IsAny<object>())).Returns(_server.Object);
            _configuration = new Mock<IConfiguration>();
            _redisCacheService = new RedisCacheService(_connectionMultiplexer.Object, _configuration.Object);
        }

        [Test]
        public async Task CheckIfKeyExistsAsync_ReturnsTrue()
        {
            //Arrange
            _db.Setup(x => x.KeyExistsAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);

            //Act
            var result = await _redisCacheService.CheckIfKeyExistsAsync(Key);

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task CheckIfKeyExistsAsync_ReturnFalseBecauseOfError()
        {
            //Arrange
            _db.Setup(x => x.KeyExistsAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ThrowsAsync<IDatabase, bool>(It.IsAny<RedisConnectionException>());

            //Act
            var result = await _redisCacheService.CheckIfKeyExistsAsync(Key);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetRecordByKeyAsync_ReturnNullBecauseOfJSonConvertError()
        {
            //Arrange
            _db.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ThrowsAsync<IDatabase, RedisValue>(It.IsAny<RedisConnectionException>());

            //Act
            var result = await _redisCacheService.GetRecordByKeyAsync<Tuple<IEnumerable<RegionObject>, int>>(new RedisValue());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoveRecordAsync_ReturnsCorrect()
        {
            //Arrange
            _db.Setup(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()));

            //Act
            await _redisCacheService.RemoveRecordAsync(Key);

            //Assert
            _db.Verify(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once);
        }

        [Test]
        public async Task RemoveRecordByPatternAsync_ReturnsEptyListOfKeys()
        {
            //Arrange
            _server
                .Setup(x => x.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(), It.IsAny<CommandFlags>()))
                .Returns(new List<RedisKey> {Key});

            //Act
            await _redisCacheService.RemoveRecordsByPatternAsync(Key);

            //Assert
            _db.Verify(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Never);
        }

        [Test]
        public async Task SetCacheRecordAsync_ReturnsCorrect()
        {
            //Arrange
            _db
                .Setup(x => x.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan>(),It.IsAny<When>(), It.IsAny<CommandFlags>()));

            //Act
            await _redisCacheService.SetCacheRecordAsync<Tuple<IEnumerable<RegionObject>, int>>(Key, CreateTuple);

            //Assert
            _db.Verify(x => x.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
        }

        private readonly RedisKey Key = "Key";
        private Tuple<IEnumerable<RegionObject>, int> CreateTuple => new Tuple<IEnumerable<RegionObject>, int>(CreateRegionObjects, 100);
        private IEnumerable<RegionObject> CreateRegionObjects => new List<RegionObject>()
        {
            new RegionObject(),
            new RegionObject()
        };


    }
}
