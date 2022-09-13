using System;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Models;
using EPlast.BLL.Services.EmailSending;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.EmailSending
{
    internal class EmailContentServiceTests
    {
        private EmailContentService _emailContentService;
        private Mock<IUserService> _mockUserService;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [Test]
        public void GetAuthFacebookRegisterEmail_ReturnsEmailModel()
        {
            // Act
            var result = _emailContentService.GetAuthFacebookRegisterEmail();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [Test]
        public void GetAuthGoogleRegisterEmail_ReturnsEmailModel()
        {
            // Act
            var result = _emailContentService.GetAuthGoogleRegisterEmail();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("citiesUrl")]
        public void GetAuthGreetingEmail_ReturnsEmailModel(string citiesUrl)
        {
            // Act
            var result = _emailContentService.GetAuthGreetingEmail(citiesUrl);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("citiesUrl", "userId", UserGenders.Male)]
        [TestCase("citiesUrl", "userId", UserGenders.Female)]
        [TestCase("citiesUrl", "userId", UserGenders.Undefined)]
        public async Task GetAuthJoinToCityReminderEmailAsync_ReturnsEmailModel(string citiesUrl, string userId, string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await _emailContentService.GetAuthJoinToCityReminderEmailAsync(citiesUrl, userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("confirmationLink")]
        public void GetAuthRegisterEmail_ReturnsEmailModel(string confirmationLink)
        {
            // Act
            var result = _emailContentService.GetAuthRegisterEmail(confirmationLink);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("confirmationLink")]
        public void GetAuthResetPasswordEmail_ReturnsEmailModel(string confirmationLink)
        {
            // Act
            var result = _emailContentService.GetAuthResetPasswordEmail(confirmationLink);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase(UserGenders.Male)]
        [TestCase(UserGenders.Female)]
        [TestCase(UserGenders.Undefined)]
        public async Task GetCanceledUserEmailAsync_ReturnsEmailModel(string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await _emailContentService.GetCanceledUserEmailAsync(new User(), new User());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("cityUrl", "cityName", "userId", UserGenders.Male)]
        [TestCase("cityUrl", "cityName", "userId", UserGenders.Female)]
        [TestCase("cityUrl", "cityName", "userId", UserGenders.Undefined)]
        public async Task GetCityApproveEmailAsync_ReturnsEmailModel(string cityUrl, string cityName, string userId, string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await _emailContentService.GetCityApproveEmailAsync(userId, cityUrl, cityName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("cityUrl", "cityName", "userId", UserGenders.Male)]
        [TestCase("cityUrl", "cityName", "userId", UserGenders.Female)]
        [TestCase("cityUrl", "cityName", "userId", UserGenders.Undefined)]
        public async Task GetCityExcludeEmailAsync_ReturnsEmailModel(string cityUrl, string cityName, string userId, string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await _emailContentService.GetCityExcludeEmailAsync(userId, cityUrl, cityName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("Name", "Surname")]
        public void GetCityAdminAboutNewPlastMemberEmail_ReturnsEmailModel(string userFirstName, string userLastName)
        {
            // Act
            var result = _emailContentService.GetCityAdminAboutNewPlastMemberEmail(userFirstName, userLastName, DateTime.Now);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("cityUrl", "cityName", "comment")]
        public void GetCityRemoveFollowerEmail_ReturnsEmailModel(string cityUrl, string cityName, string comment)
        {
            // Act
            var result = _emailContentService.GetCityRemoveFollowerEmail(cityUrl, cityName, comment);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [Test]
        public void GetCityToSupporterRoleOnApproveEmail_ReturnsEmailModel()
        {
            // Act
            var result = _emailContentService.GetCityToSupporterRoleOnApproveEmail();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase(UserGenders.Male)]
        [TestCase(UserGenders.Female)]
        [TestCase(UserGenders.Undefined)]
        public async Task GetConfirmedUserEmailAsync_ReturnsEmailModel(string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await _emailContentService.GetConfirmedUserEmailAsync(new User(), new User());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("Вінниця")]
        [TestCase("Бар")]
        [TestCase("Тернопіль")]
        public void GetGreetingForNewPlastMemberEmail_ReturnsEmailModel(string cityName)
        {
            // Act
            var result = _emailContentService.GetGreetingForNewPlastMemberEmailAsync(cityName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("userId", "userFirstName", "userLastName", UserGenders.Male, true)]
        [TestCase("userId", "userFirstName", "userLastName", UserGenders.Female ,false)]
        [TestCase("userId", "userFirstName", "userLastName", UserGenders.Undefined, true)]
        public async Task GetCityAdminAboutNewFollowerEmailAsync_ReturnsEmailModel(string userId, string userFirstName,
            string userLastName, string userGender, bool isReminder)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await _emailContentService.GetCityAdminAboutNewFollowerEmailAsync(userId, userFirstName, 
                userLastName, isReminder);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("cityName")]
        public void GetUserRenewalConfirmationEmail_ReturnsEmailModel (string cityName)
        {
            // Act
            var result = _emailContentService.GetUserRenewalConfirmationEmail(cityName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("userId", "Вінниця", 0, 210)]
        [TestCase("userId", "Бар", 0, 244)]
        [TestCase("userId", "Тернопіль", 0, 280)]
        public void GetGreetingForNewPlastMemberMessage_ReturnsUserNotificationModel(string userId, string cityName, int notificationType, int cityId)
        {
            // Act
            var result = _emailContentService.GetGreetingForNewPlastMemberMessageAsync(userId, cityName, notificationType, cityId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<UserNotification>(result);
        }

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock.Setup(m => m.HttpContext).Returns(Mock.Of<HttpContext>());

            _emailContentService = new EmailContentService(_mockUserService.Object, _mockRepositoryWrapper.Object, _httpContextAccessorMock.Object);
        }
    }
}
