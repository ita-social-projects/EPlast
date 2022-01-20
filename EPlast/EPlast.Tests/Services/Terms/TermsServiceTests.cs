using AutoMapper;
using EPlast.BLL.DTO.Terms;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Services.TermsOfUse;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using EPlast.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace EPlast.Tests.Services.Terms
{
    class TermsServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private TermsService TermsService;
        private Mock<UserManager<User>> userManager;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithAdmin());
            TermsService = new TermsService(mockRepoWrapper.Object, mockMapper.Object, userManager.Object);
        }

        [Test]
        public async Task Get_First_Record_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.TermsOfUse.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Terms, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Terms>, IIncludableQueryable<DataAccess.Entities.Terms, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.Terms());
            mockMapper.Setup(m => m.Map<TermsDTO>(It.IsAny<DataAccess.Entities.Terms>()))
                .Returns(new TermsDTO());
            //Act
            var result = await TermsService.GetFirstRecordAsync();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Get_First_Record_IsNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.TermsOfUse.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.Terms, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Terms>, IIncludableQueryable<DataAccess.Entities.Terms, object>>>()))
                .ReturnsAsync(nullTerms);
            mockMapper.Setup(m => m.Map<TermsDTO>(It.IsAny<DataAccess.Entities.AboutBase.Subsection>()))
                .Returns(nullTermsDTO);

            //Act
            var result = await TermsService.GetFirstRecordAsync();

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Get_First_Record_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.TermsOfUse.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Terms, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Terms>, IIncludableQueryable<DataAccess.Entities.Terms, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.Terms());
            mockMapper.Setup(m => m.Map<TermsDTO>(It.IsAny<DataAccess.Entities.Terms>()))
                .Returns(new TermsDTO());

            //Act
            var result = await TermsService.GetFirstRecordAsync();

            //Assert
            Assert.IsInstanceOf<TermsDTO>(result);
        }

        [Test]
        public async Task GetAllUsersIdWithoutSender_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserProfile.GetAllAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(),
                    It.IsAny<Func<IQueryable<UserProfile>, IIncludableQueryable<UserProfile, object>>>()))
                .ReturnsAsync(GetTestUsersId());

            //Act
            var actualResult = await TermsService.GetAllUsersIdWithoutAdminIdAsync(FakeUser());

            //Assert
            Assert.DoesNotThrowAsync(async () => { await TermsService.GetAllUsersIdWithoutAdminIdAsync(new User()); });
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf(typeof(IEnumerable<string>), actualResult);
            Assert.AreEqual(GetTestUsersId().Select(x => x.UserID), actualResult);
        }

        [Test]
        public void GetAllUsersIdWithoutSender_IfNotAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserProfile.GetAllAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(),
                    It.IsAny<Func<IQueryable<UserProfile>, IIncludableQueryable<UserProfile, object>>>()))
                .ReturnsAsync(GetTestUsersId());
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Act
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await TermsService.GetAllUsersIdWithoutAdminIdAsync(FakeUser()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void ChangeTerms_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.TermsOfUse.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Terms, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Terms>, IIncludableQueryable<DataAccess.Entities.Terms, object>>>()))
                .ReturnsAsync(terms);

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await TermsService.ChangeTermsAsync(termsDTO, It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void ChangeTerms_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.TermsOfUse.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Terms, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Terms>, IIncludableQueryable<DataAccess.Entities.Terms, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.Terms());

            //Assert
            Assert.DoesNotThrowAsync(async () => { await TermsService.ChangeTermsAsync(termsDTO, It.IsAny<User>()); });
        }

        [Test]
        public void AddTerms_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.TermsOfUse.CreateAsync(It.IsAny<DataAccess.Entities.Terms>()));

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await TermsService.AddTermsAsync(It.IsAny<TermsDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void AddTerms_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.TermsOfUse.CreateAsync(It.IsAny<DataAccess.Entities.Terms>()));

            //Assert
            Assert.DoesNotThrowAsync(async () => { await TermsService.AddTermsAsync(new TermsDTO(), new User()); });
        }

        DataAccess.Entities.Terms nullTerms = null;

        TermsDTO nullTermsDTO = null;

        TermsDTO termsDTO = new TermsDTO
        {
            TermsId = 1,
            TermsTitle = "Title",
            TermsText = "Text",
            DatePublication = new DateTime()
        };

        DataAccess.Entities.Terms terms = new DataAccess.Entities.Terms
        {
            TermsId = 1,
            TermsTitle = "Title",
            TermsText = "Text",
            DatePublication = new DateTime()
        };

        private IList<string> GetRolesWithoutAdmin()
        {
            return new List<string>
            {
                "Htos",
                "Nixto"
            };
        }

        private IList<string> GetRolesWithAdmin()
        {
            return new List<string>
            {
                Roles.Admin,
                "Htos",
                "Nixto"
            };
        }

        private IEnumerable<UserProfile> GetTestUsersId()
        {
            return new List<UserProfile>
            {
                new UserProfile{ UserID = "963b1137-d8b5-4de7-b83f-66791b7ca4d8", GenderID = 1},
                new UserProfile{ UserID = "99dbe3c2-6108-43cc-bac2-e8efe7e08481", GenderID = 1}
            }.AsEnumerable();
        }

        private User FakeUser()
        {
            return new User
            {
                Id = "963b1137-d8b5-4de7-b83f-66791b7ca4d8"
            };
        }
    }
}