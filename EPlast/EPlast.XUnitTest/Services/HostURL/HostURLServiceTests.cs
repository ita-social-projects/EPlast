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
            // Assert
            Assert.IsAssignableFrom<string>(service.CitiesURL);

        }

        [Fact]
        public void SignInURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Assert
            Assert.IsAssignableFrom<string>(service.SignInURL);

        }

        [Fact]
        public void UserTableURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            // Assert
            Assert.IsAssignableFrom<string>(service.UserTableURL);

        }
        [Fact]
        public void ResetPasswordURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            // Assert
            Assert.IsAssignableFrom<string>(service.ResetPasswordURL);

        }
        [Fact]
        public void UserPageMainURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            // Assert
            Assert.IsAssignableFrom<string>(service.UserPageMainURL);

        }
        [Fact]
        public void FrontEndURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            // Assert
            Assert.Null(service.FrontEndURL);
        }
        [Fact]
        public void BackEndApiURLTest()
        {
            // Arrange
            options.Setup(x => x.Value).Returns(_hostUrl);
            var service = new HostUrlService(options.Object);
            // Act
            // Assert
            Assert.IsAssignableFrom<string>(service.BackEndApiURL);

        }
    }
}
