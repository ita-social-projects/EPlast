using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Services.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.AboutBase;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.AboutBase
{
    class SubsectionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private AboutBaseSubsectionService AboutBaseSubsectionService;
        private Mock<UserManager<User>> userManager;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles());
            AboutBaseSubsectionService = new AboutBaseSubsectionService(mockRepoWrapper.Object, mockMapper.Object, userManager.Object);
        }

        [Test]
        public async Task GetSubsection_ById_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSubsection.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                    It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
                .ReturnsAsync(new Subsection());
            mockMapper.Setup(m => m.Map<SubsectionDTO>(It.IsAny<Subsection>()))
                .Returns(new SubsectionDTO());

            //Act
            var result = await AboutBaseSubsectionService.GetSubsection(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public async Task GetSubsection_ById_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSubsection.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                    It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
                .ReturnsAsync(new Subsection());
            mockMapper.Setup(m => m.Map<SubsectionDTO>(It.IsAny<Section>()))
                .Returns(new SubsectionDTO());

            //Act
            var result = await AboutBaseSubsectionService.GetSubsection(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<SubsectionDTO>(result);
        }

        [Test]
        public async Task GetSubsection_ById_Null()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSubsection.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                    It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
                .ReturnsAsync(nullSubsection);
            mockMapper.Setup(m => m.Map<SubsectionDTO>(It.IsAny<Subsection>()))
                .Returns(nullSubsectionDTO);

            //Act
            var result = await AboutBaseSubsectionService.GetSubsection(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllSubsections_NotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSubsection.GetAllAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                    It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
                .ReturnsAsync(GetTestAboutBaseSubsection());
            mockMapper.Setup(m => m.Map<IEnumerable<SubsectionDTO>>(It.IsAny<IEnumerable<Subsection>>()))
                .Returns(GetTestAboutBaseSubsectionDTO());
            //Act
            var result = await AboutBaseSubsectionService.GetAllSubsectionAsync();
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllSubsections_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSubsection.GetAllAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                    It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
                .ReturnsAsync(GetTestAboutBaseSubsection());
            mockMapper.Setup(m => m.Map<IEnumerable<SubsectionDTO>>(It.IsAny<IEnumerable<Subsection>>()))
                .Returns(GetTestAboutBaseSubsectionDTO());
            //Act
            var result = await AboutBaseSubsectionService.GetAllSubsectionAsync();
            //Assert
            Assert.IsInstanceOf<IEnumerable<SubsectionDTO>>(result);
        }

        [Test]
        public async Task GetAllSubsections_IsEmpty()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSubsection.GetAllAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                    It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
                .ReturnsAsync(nullListSubsection);
            mockMapper.Setup(m => m.Map<IEnumerable<SubsectionDTO>>(It.IsAny<IEnumerable<Subsection>>()))
                .Returns(nullListSubsectionDTO);
            //Act

            var result = await AboutBaseSubsectionService.GetAllSubsectionAsync();
            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void DeleteSubsection_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSubsection.GetFirstAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                   It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
               .ReturnsAsync(Subsection);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());
            //Assert

            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await AboutBaseSubsectionService.DeleteSubsection(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void DeleteSubsection_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSubsection.GetFirstAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                   It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
               .ReturnsAsync(new Subsection());

            //Assert
            Assert.DoesNotThrowAsync(async () => { await AboutBaseSubsectionService.DeleteSubsection(It.IsAny<int>(), It.IsAny<User>()); });
        }

        [Test]
        public void ChangeSubsection_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSubsection.GetFirstAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                   It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
               .ReturnsAsync(Subsection);

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await AboutBaseSubsectionService.ChangeSubsection(SubsectionDTO, It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void ChangeSubsection_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSubsection.GetFirstAsync(It.IsAny<Expression<Func<Subsection, bool>>>(),
                   It.IsAny<Func<IQueryable<Subsection>, IIncludableQueryable<Subsection, object>>>()))
               .ReturnsAsync(new Subsection());

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await AboutBaseSubsectionService.ChangeSubsection(SubsectionDTO, It.IsAny<User>()); });
        }

        [Test]
        public void AddSubsection_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSubsection.CreateAsync(It.IsAny<Subsection>()));

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await AboutBaseSubsectionService.AddSubsection(It.IsAny<SubsectionDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void AddSubsection_IfAdmin_WorksCorrectly()
        {
            mockRepoWrapper
               .Setup(x => x.AboutBaseSubsection.CreateAsync(It.IsAny<Subsection>()));

            Assert.DoesNotThrowAsync(async () => { await AboutBaseSubsectionService.AddSubsection(new SubsectionDTO(), new User()); });
        }

        Subsection nullSubsection = null;
        SubsectionDTO nullSubsectionDTO = null;
        List<SubsectionDTO> nullListSubsectionDTO = null;
        List<Subsection> nullListSubsection = null;
        Subsection Subsection = new Subsection
        {
            Id = 1,
            Title = "Title"
        };
        SubsectionDTO SubsectionDTO = new SubsectionDTO
        {
            Id = 1,
            Title = "Title"
        };

        private IEnumerable<Subsection> GetTestAboutBaseSubsection()
        {
            return new List<Subsection>
            {
                new Subsection{ Id = 1, Title = "Title1"},
                new Subsection{ Id = 2, Title = "Title2"},
                new Subsection{ Id = 3, Title = "Title3"}
            }.AsEnumerable();
        }
        private IEnumerable<SubsectionDTO> GetTestAboutBaseSubsectionDTO()
        {
            return new List<SubsectionDTO>
            {
                new SubsectionDTO{ Id = 1, Title = "Title1"},
                new SubsectionDTO{ Id = 2, Title = "Title2"},
                new SubsectionDTO{ Id = 3, Title = "Title3"}
            }.AsEnumerable();
        }

        private IList<string> GetRoles()
        {
            return new List<string>
            {
                Roles.Admin,
                "Htos",
                "Nixto"

            };
        }
        private IList<string> GetRolesWithoutAdmin()
        {
            return new List<string>
            {
                "Htos",
                "Nixto"

            };
        }
    }
}
