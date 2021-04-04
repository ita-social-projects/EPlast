using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Models;
using EPlast.BLL.Services.EmailSending;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.EmailSending
{
    internal class EmailContentServiceTests
    {
        private EmailContentService emailContentService;
        private Mock<IUserService> _mockUserService;

        [Test]
        public void GetAuthFacebookRegisterEmail_ReturnsEmailModel()
        {
            // Act
            var result = emailContentService.GetAuthFacebookRegisterEmail();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [Test]
        public void GetAuthGoogleRegisterEmail_ReturnsEmailModel()
        {
            // Act
            var result = emailContentService.GetAuthGoogleRegisterEmail();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("citiesUrl")]
        public void GetAuthGreetingEmail_ReturnsEmailModel(string citiesUrl)
        {
            // Act
            var result = emailContentService.GetAuthGreetingEmail(citiesUrl);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("citiesUrl", "userId", UserGenders.Male)]
        [TestCase("citiesUrl", "userId", UserGenders.Female)]
        [TestCase("citiesUrl", "userId", UserGenders.Other)]
        public async Task GetAuthJoinToCityReminderEmailAsync_ReturnsEmailModel(string citiesUrl, string userId, string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await emailContentService.GetAuthJoinToCityReminderEmailAsync(citiesUrl, userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("confirmationLink")]
        public void GetAuthRegisterEmail_ReturnsEmailModel(string confirmationLink)
        {
            // Act
            var result = emailContentService.GetAuthRegisterEmail(confirmationLink);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("confirmationLink")]
        public void GetAuthResetPasswordEmail_ReturnsEmailModel(string confirmationLink)
        {
            // Act
            var result = emailContentService.GetAuthResetPasswordEmail(confirmationLink);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase(UserGenders.Male)]
        [TestCase(UserGenders.Female)]
        [TestCase(UserGenders.Other)]
        public async Task GetCanceledUserEmailAsync_ReturnsEmailModel(string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await emailContentService.GetCanceledUserEmailAsync(new User(), new User());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("cityUrl", "cityName", true)]
        public void GetCityApproveEmail_ReturnsEmailModel(string cityUrl, string cityName, bool isApproved)
        {
            // Act
            var result = emailContentService.GetCityApproveEmail(cityUrl, cityName, isApproved);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase(UserGenders.Male)]
        [TestCase(UserGenders.Female)]
        [TestCase(UserGenders.Other)]
        public async Task GetConfirmedUserEmailAsync_ReturnsEmailModel(string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await emailContentService.GetConfirmedUserEmailAsync(new User(), new User());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [TestCase("userId", UserGenders.Male)]
        [TestCase("userId", UserGenders.Female)]
        [TestCase("userId", UserGenders.Other)]
        public async Task GetGreetingForNewPlastMemberEmailAsync_ReturnsEmailModel(string userId, string userGender)
        {
            // Arrange
            _mockUserService.Setup(x => x.GetUserGenderAsync(It.IsAny<string>()))
                .ReturnsAsync(userGender);

            // Act
            var result = await emailContentService.GetGreetingForNewPlastMemberEmailAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<EmailModel>(result);
        }

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();

            emailContentService = new EmailContentService(_mockUserService.Object);
        }
    }
}
