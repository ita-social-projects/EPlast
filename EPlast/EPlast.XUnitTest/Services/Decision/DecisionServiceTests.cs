using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Logging;
using Microsoft.EntityFrameworkCore.Query;
using Xunit;

namespace EPlast.XUnitTest
{
    public class DecisionServiceTests
    {
        private DecisionService _decisionService;
        private static Mock<IRepositoryWrapper> _repository;

        private static DecisionService CreateDecisionService(int decisionId = 1)
        {
            _repository = new Mock<IRepositoryWrapper>();
            var hostingEnvironment = new Mock<IWebHostEnvironment>();
            var directoryManager = new Mock<IDirectoryManager>();
            var fileManager = new Mock<IFileManager>();
            var fileStreamManager = new Mock<IFileStreamManager>();
            var mapper = new Mock<IMapper>();
            var decisionVmCreator = new Mock<IDecisionVmInitializer>();
            var logger = new Mock<ILoggerService<DecisionService>>();
            directoryManager.Setup(dir => dir.Exists(It.IsAny<string>())).Returns(true);
            directoryManager.Setup(dir => dir.GetFiles(It.IsAny<string>())).Returns(new[] { "yes", "stonks", "files" });

            fileStreamManager.Setup(f => f.GenerateFileStreamManager(It.IsAny<string>(), It.IsAny<FileMode>()))
                .Returns<string, FileMode>((path, mode) => new FileStreamManager());
            fileStreamManager.Setup(f => f.GetStream()).Returns(new MemoryStream());
            fileStreamManager.Setup(f => f.CopyToAsync(It.IsAny<MemoryStream>()))
                .Callback<MemoryStream>(mem => mem.SetLength(5));
            fileStreamManager.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>((memFrom, memTo) => memTo.SetLength(5));

            fileManager.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);

            _repository.Setup(rep => rep.Decesion.Attach(new Decesion()));
            _repository.Setup(rep => rep.Decesion.Create(new Decesion()));
            _repository.Setup(rep => rep.Decesion.Update(new Decesion()));
            _repository.Setup(rep => rep.SaveAsync());
            _repository.Setup(rep => rep.DecesionTarget.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DecesionTarget, bool>>>(),
                   It.IsAny<Func<IQueryable<DecesionTarget>, IIncludableQueryable<DecesionTarget, object>>>())).ReturnsAsync(new DecesionTarget());
            mapper.Setup(m => m.Map<IEnumerable<DecisionTargetDTO>>(It.IsAny<IEnumerable<DecesionTarget>>()))
                .Returns(GetTestDecisionTargetsDtoList);
            mapper.Setup(m => m.Map<DecisionDTO>(It.IsAny<Decesion>()))
                .Returns(() => GetTestDecisionsDtoListElement(decisionId));
            mapper.Setup(m => m.Map<IEnumerable<DecisionDTO>>(It.IsAny<IEnumerable<Decesion>>())).Returns(GetTestDecisionsDtoList);
            return new DecisionService(_repository.Object, mapper.Object, decisionVmCreator.Object, null);
        }

        [Fact]
        public void CreateDecisionTest()
        {
            _decisionService = CreateDecisionService();
            _repository.Setup(rep => rep.DecesionTarget.GetAllAsync(It.IsAny<Expression<Func<DecesionTarget, bool>>>(),
                    It.IsAny<Func<IQueryable<DecesionTarget>, IIncludableQueryable<DecesionTarget, object>>>()))
                .ReturnsAsync(GetTestDecisionTargetsQueryable);

            var decision = _decisionService.CreateDecision();

            Assert.IsType<DecisionWrapperDTO>(decision);
        }
        /*
                [Fact]
                public async Task CreateDecisionDecisionTargetsCountTest()
                {
                    _decisionService = CreateDecisionService();
                    _repository.Setup(rep => rep.DecesionTarget.GetAllAsync(It.IsAny<Expression<Func<DecesionTarget, bool>>>(),
                            It.IsAny<Func<IQueryable<DecesionTarget>, IIncludableQueryable<DecesionTarget, object>>>()))
                        .ReturnsAsync(GetTestDecisionTargetsQueryable);
                    var decision = await _decisionService.CreateDecisionAsync();

                    Assert.Equal(GetTestDecisionTargetsDtoList().Count, decision.DecisionTargets.Count());
                }
                */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetDecisionTest(int decisionId)
        {
            _decisionService = CreateDecisionService(decisionId);

            var decision = await _decisionService.GetDecisionAsync(decisionId);

            Assert.Equal(decisionId, decision.ID);
        }



        [Fact]
        public async Task GetDecisionListTest()
        {
            _decisionService = CreateDecisionService();
            _repository.Setup(rep => rep.Decesion.GetAllAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().AsEnumerable);

            var decision = (await _decisionService.GetDecisionListAsync()).ToList();

            Assert.IsType<List<DecisionWrapperDTO>>(decision);
        }

        [Fact]
        public async Task GetDecisionListCountTest()
        {
            _decisionService = CreateDecisionService();
            _repository.Setup(rep => rep.Decesion.GetAllAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable());
            var decision = (await _decisionService.GetDecisionListAsync()).ToList();

            Assert.Equal(GetTestDecisionsDtoList().Count, decision.Count);
        }

        [Theory]
        [InlineData("new name", "new text")]
        [InlineData("", "new text")]
        [InlineData("new name", "")]
        [InlineData("", "")]
        public async Task ChangeDecisionTest(string decisionNewName, string decisionNewDescription)
        {
            _decisionService = CreateDecisionService();
            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().FirstOrDefault());
            var changingDecisionDto = new DecisionDTO();

            changingDecisionDto.Name = decisionNewName;
            changingDecisionDto.Description = decisionNewDescription;
            await _decisionService.ChangeDecisionAsync(changingDecisionDto);

            _repository.Verify(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                   It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteDecisionTest(int decisionId)
        {
            _decisionService = CreateDecisionService();
            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().FirstOrDefault(d => d.ID == decisionId));

            await _decisionService.DeleteDecisionAsync(decisionId);

            _repository.Verify(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()), Times.Once);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(1)]
        [InlineData(3)]
        public async Task SaveDecisionTest(int decisionId)
        {
            _decisionService = CreateDecisionService();

            var decision = new DecisionWrapperDTO
            {
                Decision = new DecisionDTO
                {
                    ID = decisionId,
                    DecisionTarget = new DecisionTargetDTO
                    {
                        ID = new Random().Next(),
                        TargetName = Guid.NewGuid().ToString()
                    }
                },
            };
            var actualReturn = await _decisionService.SaveDecisionAsync(decision);

            Assert.Equal(decisionId, actualReturn);
        }
        /*
        [Theory]
        [InlineData(1, new byte[] { 1, 2, 3, 4, 5 })]
        [InlineData(3, new byte[] { 5, 4, 3, 2, 1 })]
        public async Task DownloadDecisionFileTest(int decisionId, byte[] exceptedBytesCount)
        {
            _decisionService = CreateDecisionService();

            var result = await _decisionService.DownloadDecisionFileAsync(decisionId);

            Assert.Equal(exceptedBytesCount.Length, result.Length);
        }
        */
        private static IQueryable<DecesionTarget> GetTestDecisionTargetsQueryable()
        {
            return new List<DecesionTarget>
            {
                new DecesionTarget {ID = 1, TargetName = "First DecesionTarget"},
                new DecesionTarget {ID = 2, TargetName = "Second DecesionTarget"},
                new DecesionTarget {ID = 3, TargetName = "Third DecesionTarget"}
            }.AsQueryable();
        }

        private static List<DecisionTargetDTO> GetTestDecisionTargetsDtoList()
        {
            return new List<DecisionTargetDTO>
            {
                new DecisionTargetDTO {ID = 1, TargetName = "First DecesionTarget"},
                new DecisionTargetDTO {ID = 2, TargetName = "Second DecesionTarget"},
                new DecisionTargetDTO {ID = 3, TargetName = "Third DecesionTarget"}
            };
        }

        private static IQueryable<Decesion> GetTestDecesionQueryable()
        {
            return new List<Decesion>
            {
                new Decesion  {ID = 1,Description = "old"},
                new Decesion  {ID = 2,Description = "old"},
                new Decesion  {ID = 3,Description = "old"},
                new Decesion  {ID = 4,Description = "old"}
            }.AsQueryable();
        }

        private static List<DecisionDTO> GetTestDecisionsDtoList()
        {
            return new List<DecisionDTO>
            {
                new DecisionDTO {ID = 1,Description = "old", Organization = new OrganizationDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 2,Description = "old", Organization = new OrganizationDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 3,Description = "old", Organization = new OrganizationDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 4,Description = "old", Organization = new OrganizationDTO(), DecisionTarget = new DecisionTargetDTO()}
            };
        }

        private static DecisionDTO GetTestDecisionsDtoListElement(int id)
        {
            return GetTestDecisionsDtoList().First(x => x.ID == id);
        }
    }
}