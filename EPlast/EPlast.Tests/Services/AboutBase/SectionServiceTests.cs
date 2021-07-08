using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Services.AboutBase;
using EPlast.DataAccess.Entities;
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
    class SectionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private AboutBaseSectionService AboutBaseSectionService;
        private Mock<UserManager<User>> userManager;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles());
            AboutBaseSectionService = new AboutBaseSectionService(mockRepoWrapper.Object, mockMapper.Object, userManager.Object);
        }

        [Test]
        public async Task GetSection_ById_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSection.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                    It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
                .ReturnsAsync(new Section());
            mockMapper.Setup(m => m.Map<SectionDTO>(It.IsAny<Section>()))
                .Returns(new SectionDTO());

            //Act
            var result = await AboutBaseSectionService.GetSection(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public async Task GetSection_ById_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSection.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                    It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
                .ReturnsAsync(new Section());
            mockMapper.Setup(m => m.Map<SectionDTO>(It.IsAny<Section>()))
                .Returns(new SectionDTO());

            //Act
            var result = await AboutBaseSectionService.GetSection(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<SectionDTO>(result);
        }

        [Test]
        public async Task GetSection_ById_Null()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSection.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                    It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
                .ReturnsAsync(nullSection);
            mockMapper.Setup(m => m.Map<SectionDTO>(It.IsAny<Section>()))
                .Returns(nullSectionDTO);

            //Act
            var result = await AboutBaseSectionService.GetSection(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllSections_NotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSection.GetAllAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                    It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
                .ReturnsAsync(GetTestAboutBaseSection());
            mockMapper.Setup(m => m.Map<IEnumerable<SectionDTO>>(It.IsAny<IEnumerable<Section>>()))
                .Returns(GetTestAboutBaseSectionDTO());
            //Act
            var result = await AboutBaseSectionService.GetAllSectionAsync();
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllSections_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSection.GetAllAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                    It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
                .ReturnsAsync(GetTestAboutBaseSection());
            mockMapper.Setup(m => m.Map<IEnumerable<SectionDTO>>(It.IsAny<IEnumerable<Section>>()))
                .Returns(GetTestAboutBaseSectionDTO());
            //Act
            var result = await AboutBaseSectionService.GetAllSectionAsync();
            //Assert
            Assert.IsInstanceOf<IEnumerable<SectionDTO>>(result);
        }

        [Test]
        public async Task GetAllSections_IsEmpty()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.AboutBaseSection.GetAllAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                    It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
                .ReturnsAsync(nullListSection);
            mockMapper.Setup(m => m.Map<IEnumerable<SectionDTO>>(It.IsAny<IEnumerable<Section>>()))
                .Returns(nullListSectionDTO);
            //Act

            var result = await AboutBaseSectionService.GetAllSectionAsync();
            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void DeleteSection_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSection.GetFirstAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                   It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
               .ReturnsAsync(Section);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());
            //Assert

            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await AboutBaseSectionService.DeleteSection(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void DeleteSection_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSection.GetFirstAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                   It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
               .ReturnsAsync(new Section());

            //Assert
            Assert.DoesNotThrowAsync(async () => { await AboutBaseSectionService.DeleteSection(It.IsAny<int>(), It.IsAny<User>()); });
        }

        [Test]
        public void ChangeSection_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSection.GetFirstAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                   It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
               .ReturnsAsync(Section);

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await AboutBaseSectionService.ChangeSection(SectionDTO, It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void ChangeSection_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSection.GetFirstAsync(It.IsAny<Expression<Func<Section, bool>>>(),
                   It.IsAny<Func<IQueryable<Section>, IIncludableQueryable<Section, object>>>()))
               .ReturnsAsync(new Section());

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await AboutBaseSectionService.ChangeSection(SectionDTO, It.IsAny<User>()); });
        }

        [Test]
        public void AddSection_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.AboutBaseSection.CreateAsync(It.IsAny<Section>()));

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await AboutBaseSectionService.AddSection(It.IsAny<SectionDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void AddSection_IfAdmin_WorksCorrectly()
        {
            mockRepoWrapper
               .Setup(x => x.AboutBaseSection.CreateAsync(It.IsAny<Section>()));

            Assert.DoesNotThrowAsync(async () => { await AboutBaseSectionService.AddSection(new SectionDTO(), new User()); });
        }

        Section nullSection = null;
        SectionDTO nullSectionDTO = null;
        List<SectionDTO> nullListSectionDTO = null;
        List<Section> nullListSection = null;
        Section Section = new Section
        {
            Id = 1,
            Title = "Title"
        };
        SectionDTO SectionDTO = new SectionDTO 
        { 
            Id = 1,
            Title = "Title"
        };

        private IEnumerable<Section> GetTestAboutBaseSection()
        {
            return new List<Section>
            {
                new Section{ Id = 1, Title = "Title1"},
                new Section{ Id = 2, Title = "Title2"},
                new Section{ Id = 3, Title = "Title3"}
            }.AsEnumerable();
        }
        private IEnumerable<SectionDTO> GetTestAboutBaseSectionDTO()
        {
            return new List<SectionDTO>
            {
                new SectionDTO{ Id = 1, Title = "Title1"},
                new SectionDTO{ Id = 2, Title = "Title2"},
                new SectionDTO{ Id = 3, Title = "Title3"}
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
