using AutoMapper;
using EPlast.BussinessLayer;
using EPlast.BussinessLayer.DTO;
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
using EPlast.BussinessLayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EPlast.XUnitTest
{
    public class DecisionServiceTests
    {
        private DecisionService _decisionService;

        private static DecisionService CreateDecisionService(int decisionId = 1)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var hostingEnvironment = new Mock<IHostingEnvironment>();
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

            repository.Setup(rep => rep.DecesionTarget.FindAll()).Returns(GetTestDecisionTargetsQueryable);
            repository.Setup(rep => rep.Decesion.FindByCondition(It.IsAny<Expression<Func<Decesion, bool>>>()))
                .Returns(
                    (Expression<Func<Decesion, bool>> condition) =>
                        GetTestDecesionQueryable().Where(condition)
                );
            repository.Setup(rep => rep.Decesion.Include(x => x.DecesionTarget, x => x.Organization))
                .Returns(GetTestDecesionQueryable());
            repository.Setup(rep => rep.Decesion.Attach(new Decesion()));
            repository.Setup(rep => rep.Decesion.Create(new Decesion()));
            repository.Setup(rep => rep.Decesion.Update(new Decesion()));
            repository.Setup(rep => rep.Save());

            mapper.Setup(m => m.Map<List<DecisionTargetDTO>>(It.IsAny<List<DecesionTarget>>()))
                .Returns(GetTestDecisionTargetsDtoList);
            mapper.Setup(m => m.Map<DecisionDTO>(It.IsAny<Decesion>()))
                .Returns(() => GetTestDecisionsDtoListElement(decisionId));
            mapper.Setup(m => m.Map<List<DecisionDTO>>(It.IsAny<List<Decesion>>())).Returns(GetTestDecisionsDtoList);
            return new DecisionService(repository.Object, hostingEnvironment.Object, directoryManager.Object,
                fileManager.Object, fileStreamManager.Object, mapper.Object, decisionVmCreator.Object, logger.Object);
        }

        [Fact]
        public void CreateDecisionTest()
        {
            _decisionService = CreateDecisionService();

            var decision = _decisionService.CreateDecision();

            Assert.IsType<DecisionWrapperDTO>(decision);
        }

        [Fact]
        public void CreateDecisionDecisionTargetsCountTest()
        {
            _decisionService = CreateDecisionService();

            var decision = _decisionService.CreateDecision();

            Assert.Equal(GetTestDecisionTargetsDtoList().Count, decision.DecisionTargets.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetDecisionTest(int decisionId)
        {
            _decisionService = CreateDecisionService(decisionId);

            var decision = _decisionService.GetDecision(decisionId);

            Assert.Equal(decisionId, decision.ID);
        }

        [Fact]
        public void GetDecisionTestForNull()
        {
            const int decisionId = 0;

            _decisionService = CreateDecisionService(decisionId);

            var decision = _decisionService.GetDecision(decisionId);

            Assert.Null(decision);
        }

        [Fact]
        public void GetDecisionListTest()
        {
            _decisionService = CreateDecisionService();

            var decision = _decisionService.GetDecisionList();

            Assert.IsType<List<DecisionWrapperDTO>>(decision);
        }

        [Fact]
        public void GetDecisionListCountTest()
        {
            _decisionService = CreateDecisionService();

            var decision = _decisionService.GetDecisionList();

            Assert.Equal(GetTestDecisionsDtoList().Count, decision.Count);
        }

        [Theory]
        [InlineData(1, "new name", "new text")]
        [InlineData(1, "", "new text")]
        [InlineData(1, "new name", "")]
        [InlineData(1, "", "")]
        public void ChangeDecisionTest(int decisionId, string decisionNewName, string decisionNewDescription)
        {
            _decisionService = CreateDecisionService();

            var changingDecisionDto = _decisionService.GetDecision(decisionId);
            changingDecisionDto.Name = decisionNewName;
            changingDecisionDto.Description = decisionNewDescription;
            var changeDecisionState = _decisionService.ChangeDecision(changingDecisionDto);

            Assert.True(changeDecisionState);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(40, false)]
        public void DeleteDecisionTest(int decisionId, bool expected)
        {
            _decisionService = CreateDecisionService();

            var actual = _decisionService.DeleteDecision(decisionId);

            Assert.Equal(expected, actual);
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
                Decision = GetTestDecisionsDtoListElement(decisionId),
            };
            var actualReturn = await _decisionService.SaveDecisionAsync(decision);

            Assert.True(actualReturn);
        }

        [Theory]
        [InlineData(1, new byte[] { 1, 2, 3, 4, 5 })]
        [InlineData(3, new byte[] { 5, 4, 3, 2, 1 })]
        public async Task DownloadDecisionFileTest(int decisionId, byte[] exceptedBytesCount)
        {
            _decisionService = CreateDecisionService();

            var result = await _decisionService.DownloadDecisionFileAsync(decisionId);

            Assert.Equal(exceptedBytesCount.Length, result.Length);
        }

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
                new Decesion  {ID = 1,HaveFile = true,Description = "old"},
                new Decesion  {ID = 2,Description = "old"},
                new Decesion  {ID = 3,HaveFile = true,Description = "old"},
                new Decesion  {ID = 4,Description = "old"}
            }.AsQueryable();
        }

        private static List<DecisionDTO> GetTestDecisionsDtoList()
        {
            return new List<DecisionDTO>
            {
                new DecisionDTO {ID = 1,HaveFile = true,Description = "old"},
                new DecisionDTO {ID = 2,Description = "old"},
                new DecisionDTO {ID = 3,HaveFile = true,Description = "old"},
                new DecisionDTO {ID = 4,Description = "old"}
            };
        }

        private static DecisionDTO GetTestDecisionsDtoListElement(int id)
        {
            return GetTestDecisionsDtoList().First(x => x.ID == id);
        }
    }
}