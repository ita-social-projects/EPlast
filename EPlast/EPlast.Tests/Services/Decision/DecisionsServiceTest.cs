using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Decision
{
    class DecisionsServiceTest
    {
        private DecisionService _decisionService;
        private static Mock<IRepositoryWrapper> _repository;
        private static Mock<IDecisionBlobStorageRepository> _decisionBlobStorage;
        private static Mock<IDecisionVmInitializer> _decisionVmCreator;
        private static Mock<IUniqueIdService> _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _decisionBlobStorage = new Mock<IDecisionBlobStorageRepository>();
            _repository = new Mock<IRepositoryWrapper>();
            var mapper = new Mock<IMapper>();
            _decisionVmCreator = new Mock<IDecisionVmInitializer>();
            _uniqueId = new Mock<IUniqueIdService>();
            _repository.Setup(rep => rep.Decesion.Attach(new Decesion()));
            _repository.Setup(rep => rep.Decesion.Create(new Decesion()));
            _repository.Setup(rep => rep.Decesion.Update(new Decesion()));
            _repository.Setup(rep => rep.SaveAsync());
            _repository.Setup(rep => rep.DecesionTarget.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DecesionTarget, bool>>>(),
                   It.IsAny<Func<IQueryable<DecesionTarget>, IIncludableQueryable<DecesionTarget, object>>>())).ReturnsAsync(new DecesionTarget());
            mapper.Setup(m => m.Map<IEnumerable<DecisionTargetDTO>>(It.IsAny<IEnumerable<DecesionTarget>>()))
                .Returns(GetTestDecisionTargetsDtoList);
            mapper.Setup(m => m.Map<DecisionDTO>(It.IsAny<Decesion>()))
                .Returns(() => GetTestDecisionsDtoListElement());
            mapper.Setup(m => m.Map<IEnumerable<DecisionDTO>>(It.IsAny<IEnumerable<Decesion>>())).Returns(GetTestDecisionsDtoList);
            mapper.Setup(m => m.Map<GoverningBodyDTO>(It.IsAny<Organization>()))
                .Returns(GetTestOrganizationDtoList()[0]);
            mapper.Setup(m => m.Map<IEnumerable<GoverningBodyDTO>>(It.IsAny<IEnumerable<Organization>>()))
                .Returns(GetTestOrganizationDtoList());
            _decisionService = new DecisionService(_repository.Object, mapper.Object, _decisionVmCreator.Object, _decisionBlobStorage.Object, _uniqueId.Object);
        }

        [Test]
        public void CreateDecisionTest_ReturnsNewDecision()
        {
            //Arrange
            _repository.Setup(rep => rep.DecesionTarget.GetAllAsync(It.IsAny<Expression<Func<DecesionTarget, bool>>>(),
                    It.IsAny<Func<IQueryable<DecesionTarget>, IIncludableQueryable<DecesionTarget, object>>>()))
                .ReturnsAsync(GetTestDecisionTargetsQueryable);
          
            //Act
            var decision = _decisionService.CreateDecision();

            //Assert
            Assert.IsNotNull(decision);
            Assert.IsInstanceOf<DecisionWrapperDTO>(decision);
        }

        [Test]
        public async Task GetDecisionTest_ReturnsObj()
        {
            //Act
            var decision = await _decisionService.GetDecisionAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<DecisionDTO>(decision);
        }

        [Test]
        public async Task GetDecisionListTest_ReturnDecisionList()
        {
            //Arrange
            _repository.Setup(rep => rep.Decesion.GetAllAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().AsEnumerable);

            //Act
            var decision = (await _decisionService.GetDecisionListAsync()).ToList();

            //Assert
            Assert.IsInstanceOf<List<DecisionWrapperDTO>>(decision);
        }

        [Test]
        public async Task GetDecisionListCount_Valid_Test()
        {
            //Arrange
            _repository.Setup(rep => rep.Decesion.GetAllAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable());

            //Act
            var decision = (await _decisionService.GetDecisionListAsync()).ToList();

            //Assert
            Assert.AreEqual(GetTestDecisionsDtoList().Count, decision.Count);
        }

        [TestCase("new name", "new text")]
        [TestCase("", "new text")]
        [TestCase("new name", "")]
        [TestCase("", "")]
        public async Task ChangeDecision_Valid_Test(string decisionNewName, string decisionNewDescription)
        {
            //Arrange
            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().FirstOrDefault());

            //Act
            var changingDecisionDto = new DecisionDTO();
            changingDecisionDto.Name = decisionNewName;
            changingDecisionDto.Description = decisionNewDescription;
            await _decisionService.ChangeDecisionAsync(changingDecisionDto);

            //Assert
            _repository.Verify(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                   It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()), Times.Once);
        }
        
        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteDecisionTest(int decisionId)
        {
            //Arrange
            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().FirstOrDefault(d => d.ID == decisionId));

            //Act
            await _decisionService.DeleteDecisionAsync(decisionId);

            //Assert
            _repository.Verify(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()), Times.Once);
        }

        [TestCase(2)]
        [TestCase(4)]
        [TestCase(1)]
        [TestCase(3)]
        public async Task SaveDecisionTest(int decisionId)
        {
            //Arrange
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

            //Act
            var actualReturn = await _decisionService.SaveDecisionAsync(decision);

            //Assert
            Assert.AreEqual(decisionId, actualReturn);
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task GetDecisionOrganizationAsyncWithEmptyOrNullParameterTest(string organizationName)
        {
            //Arrange
            GoverningBodyDTO governingBody = GetTestOrganizationDtoList()[0];
            governingBody.GoverningBodyName = organizationName;
            _repository.Setup(rep => rep.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new Organization() { ID = governingBody.ID });

            //Act
            var actualReturn = await _decisionService.GetDecisionOrganizationAsync(governingBody);

            //Assert
            Assert.AreEqual(governingBody.ID, actualReturn.ID);
        }

        [Test]
        public async Task GetDecisionOrganizationAsyncWithRightParameterTest()
        {
            //Arrange
            GoverningBodyDTO organization = GetTestOrganizationDtoList()[0];
            _repository.Setup(rep => rep.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new Organization() { OrganizationName = organization.GoverningBodyName });

            //Act
            var actualReturn = await _decisionService.GetDecisionOrganizationAsync(organization);

            //Assert
            Assert.AreEqual(organization.GoverningBodyName, actualReturn.GoverningBodyName);
        }

        [TestCase("filename1")]
        [TestCase("filename2")]
        public async Task DownloadDecisionFileFromBlobAsyncTest(string fileName)
        {
            //Arrange
            _decisionBlobStorage.Setup(blobStorage => blobStorage.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(fileName);

            //Act
            var actualReturn = await _decisionService.DownloadDecisionFileFromBlobAsync(fileName);
            
            //Assert
            Assert.AreEqual(fileName, actualReturn);
        }

        [Test]
        public async Task GetOrganizationListAsyncTest()
        {
            //Arrange
            List<GoverningBodyDTO> governingBodyDtos = GetTestOrganizationDtoList();
            _repository.Setup(rep => rep.GoverningBody.GetAllAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new List<Organization>());

            //Act
            var actualReturn = await _decisionService.GetGoverningBodyListAsync();

            //Assert
            Assert.AreEqual(governingBodyDtos.Aggregate("", (x, y) => x += y.GoverningBodyName), actualReturn.Aggregate("", (x, y) => x += y.GoverningBodyName));
        }

        [Test]
        public async Task GetDecisionTargetListAsyncTest()
        {
            //Arrange
            List<DecisionTargetDTO> decisionTargets = GetTestDecisionTargetsDtoList();
            _repository.Setup(rep => rep.DecesionTarget.GetAllAsync(It.IsAny<Expression<Func<DecesionTarget, bool>>>(),
                It.IsAny<Func<IQueryable<DecesionTarget>, IIncludableQueryable<DecesionTarget, object>>>())).ReturnsAsync(new List<DecesionTarget>());

            //Act
            var actualReturn = await _decisionService.GetDecisionTargetListAsync();

            //Assert
            Assert.AreEqual(decisionTargets.Aggregate("", (x, y) => x += y.TargetName), actualReturn.Aggregate("", (x, y) => x += y.TargetName));
        }

        [Test]
        public void GetDecisionStatusTypesTest()
        {
            //Arrange
            _decisionVmCreator.Setup(vm => vm.GetDecesionStatusTypes()).Returns(new List<SelectListItem>());
           
            //Act
            var actualReturn = _decisionService.GetDecisionStatusTypes();

            //Assert
            _decisionVmCreator.Verify();
            Assert.IsInstanceOf<List<SelectListItem>>(actualReturn);
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

        private static DecisionDTO GetTestDecisionsDtoListElement(int id = 1)
        {
            return GetTestDecisionsDtoList().First(x => x.ID == id);
        }

        private static List<GoverningBodyDTO> GetTestOrganizationDtoList()
        {
            return new List<GoverningBodyDTO>
            {
                new GoverningBodyDTO {ID = 1,GoverningBodyName = "Organization1"},
                new GoverningBodyDTO {ID = 2,GoverningBodyName = "Organization2"},
            };
        }
    }
}


