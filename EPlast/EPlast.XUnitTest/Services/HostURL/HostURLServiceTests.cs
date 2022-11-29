using EPlast.BLL.Services.HostURL;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.HostURL
{
    public class HostURLServiceTests
    {
        private HostUrlOptions _hostUrl;
        private Mock<IOptions<HostUrlOptions>> options;
        public HostURLServiceTests()
        {
            _hostUrl = new HostUrlOptions();
            options = new Mock<IOptions<HostUrlOptions>>();
        }
       
        [Fact]
        public void GetFrontEndURLTest()
        {
            // Arrange
            string tail = "/api";
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.GetFrontEndURL(tail);
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);
         
        }

        [Fact]
        public void GetSignInURLTest()
        {
            // Arrange
            int error = 404;
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.GetSignInURL(error);
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);

        }
        [Fact]
        public void GetResetPasswordURLTest()
        {
            // Arrange
            string token = "kkhkf57jgdu58";
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.GetResetPasswordURL(token);
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);

        }

        [Fact]
        public void GetUserTableURLTest()
        {
            // Arrange
            string search = "search";
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.GetUserTableURL(search);
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);
        }
        [Fact]
        public void GetUserTableURLTest2()
        {
            // Arrange
            string firstName = "Леся";
            string lastName = "Українка";
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.GetUserTableURL((firstName, lastName));
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);
        }
        [Fact]
        public void GetConfirmEmailApiURLTest()
        {
            // Arrange
            string token = "kkhkf57jgdu58";
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.GetConfirmEmailApiURL("1", token);
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);
        }
        [Fact]
        public void GetUserPageMainURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.GetUserPageMainURL("1");
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);
        }
        [Fact]
        public void GetCitiesURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.GetCitiesURL(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);

        }

        [Fact]
        public void CitiesURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            //Act
            var result = service.CitiesURL;
            // Assert
            Assert.IsAssignableFrom<string>(result);

        }

        [Fact]
        public void SignInURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            //Act
            var result = service.SignInURL;
            // Assert
            Assert.IsAssignableFrom<string>(result);

        }

        [Fact]
        public void UserTableURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.UserTableURL;
            // Assert
            Assert.IsAssignableFrom<string>(result);

        }
        [Fact]
        public void ResetPasswordURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.ResetPasswordURL;
            // Assert
            Assert.IsAssignableFrom<string>(result);

        }
        [Fact]
        public void UserPageMainURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.UserPageMainURL;
            // Assert
            Assert.IsAssignableFrom<string>(result);

        }
        [Fact]
        public void FrontEndURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.FrontEndURL;
            // Assert
            Assert.Null(result);
        }
        [Fact]
        public void BackEndApiURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            var result = service.BackEndApiURL;
            // Assert
            Assert.IsAssignableFrom<string>(result);

        }
    }
}
