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
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using Xunit;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;

namespace EPlast.XUnitTest
{
    public class DecisionServiceTests
    {
        private DecisionService _decisionService;
        private static Mock<IRepositoryWrapper> _repository;
        private static Mock<IDecisionBlobStorageRepository> _decisionBlobStorage;
        private static Mock<IDecisionVmInitializer> _decisionVmCreator;
        private static Mock<IUniqueIdService> _uniqueId;

        private static DecisionService CreateDecisionService(int decisionId = 1)
        {
            _decisionBlobStorage = new Mock<IDecisionBlobStorageRepository>();
            _repository = new Mock<IRepositoryWrapper>();
            var hostingEnvironment = new Mock<IWebHostEnvironment>();
            var directoryManager = new Mock<IDirectoryManager>();
            var fileManager = new Mock<IFileManager>();
            var fileStreamManager = new Mock<IFileStreamManager>();
            var mapper = new Mock<IMapper>();
            _decisionVmCreator = new Mock<IDecisionVmInitializer>();
            var logger = new Mock<ILoggerService<DecisionService>>();
            _uniqueId = new Mock<IUniqueIdService>();
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
            mapper.Setup(m => m.Map<GoverningBodyDTO>(It.IsAny<Organization>()))
                .Returns(GetTestOrganizationDtoList()[0]);
            mapper.Setup(m => m.Map<IEnumerable<GoverningBodyDTO>>(It.IsAny<IEnumerable<Organization>>()))
                .Returns(GetTestOrganizationDtoList());
            return new DecisionService(_repository.Object, mapper.Object, _decisionVmCreator.Object, _decisionBlobStorage.Object, _uniqueId.Object);
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetDecisionOrganizationAsyncWithEmptyOrNullParameterTest(string organizationName)
        {
            _decisionService = CreateDecisionService();
            GoverningBodyDTO organization = GetTestOrganizationDtoList()[0];
            organization.GoverningBodyName = organizationName;
            _repository.Setup(rep => rep.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new Organization() { ID = organization.ID });

            var actualReturn = await _decisionService.GetDecisionOrganizationAsync(organization);

            Assert.Equal(organization.ID, actualReturn.ID);
        }

        [Fact]
        public async Task GetDecisionOrganizationAsyncWithRightParameterTest()
        {
            _decisionService = CreateDecisionService();
            GoverningBodyDTO organization = GetTestOrganizationDtoList()[0];
            _repository.Setup(rep => rep.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new Organization() { OrganizationName = organization.GoverningBodyName });

            var actualReturn = await _decisionService.GetDecisionOrganizationAsync(organization);

            Assert.Equal(organization.GoverningBodyName, actualReturn.GoverningBodyName);
        }
        [Theory]
        [InlineData("filename1")]
        [InlineData("filename2")]
        public async Task DownloadDecisionFileFromBlobAsyncTest(string fileName)
        {
            _decisionService = CreateDecisionService();
            _decisionBlobStorage.Setup(blobStorage => blobStorage.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(fileName);


            var actualReturn = await _decisionService.DownloadDecisionFileFromBlobAsync(fileName);

            Assert.Equal(fileName, actualReturn);
        }

        [Fact]
        public async Task GetOrganizationListAsyncTest()
        {
            _decisionService = CreateDecisionService();
            List<GoverningBodyDTO> organizations = GetTestOrganizationDtoList();
            _repository.Setup(rep => rep.GoverningBody.GetAllAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new List<Organization>());

            var actualReturn = await _decisionService.GetGoverningBodyListAsync();

            Assert.Equal(organizations.Aggregate("", (x, y) => x += y.GoverningBodyName), actualReturn.Aggregate("", (x, y) => x += y.GoverningBodyName));
        }

        [Fact]
        public async Task GetDecisionTargetListAsyncTest()
        {
            _decisionService = CreateDecisionService();
            List<DecisionTargetDTO> decisionTargets = GetTestDecisionTargetsDtoList();
            _repository.Setup(rep => rep.DecesionTarget.GetAllAsync(It.IsAny<Expression<Func<DecesionTarget, bool>>>(),
                It.IsAny<Func<IQueryable<DecesionTarget>, IIncludableQueryable<DecesionTarget, object>>>())).ReturnsAsync(new List<DecesionTarget>());

            var actualReturn = await _decisionService.GetDecisionTargetListAsync();

            Assert.Equal(decisionTargets.Aggregate("", (x, y) => x += y.TargetName), actualReturn.Aggregate("", (x, y) => x += y.TargetName));
        }

        [Fact]
        public void GetDecisionStatusTypesTest()
        {
            _decisionService = CreateDecisionService();
            _decisionVmCreator.Setup(vm => vm.GetDecesionStatusTypes()).Returns(new List<SelectListItem>());
            var actualReturn = _decisionService.GetDecisionStatusTypes();

            _decisionVmCreator.Verify();
            Assert.IsType<List<SelectListItem>>(actualReturn);
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
                new DecisionDTO {ID = 1,Description = "old", GoverningBody = new GoverningBodyDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 2,Description = "old", GoverningBody = new GoverningBodyDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 3,Description = "old", GoverningBody = new GoverningBodyDTO(), DecisionTarget = new DecisionTargetDTO()},
                new DecisionDTO {ID = 4,Description = "old", GoverningBody = new GoverningBodyDTO(), DecisionTarget = new DecisionTargetDTO()}
            };
        }

        private static DecisionDTO GetTestDecisionsDtoListElement(int id)
        {
            return GetTestDecisionsDtoList().First(x => x.ID == id);
        }
        private static List<GoverningBodyDTO> GetTestOrganizationDtoList()
        {
            return new List<GoverningBodyDTO>
            {
                new GoverningBodyDTO {ID = 1, GoverningBodyName = "Organization1"},
                new GoverningBodyDTO {ID = 2, GoverningBodyName = "Organization2"},
            };
        }
    }
}