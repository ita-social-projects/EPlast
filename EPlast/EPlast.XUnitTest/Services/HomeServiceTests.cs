using EPlast.BussinessLayer;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Home
{
    public class HomeServiceTests
    {
        public (IHomeService, Mock<IEmailConfirmation>) CreateHomeService()
        {
            var mockEmailConfirmation = new Mock<IEmailConfirmation>();
            var homeService = new HomeService(mockEmailConfirmation.Object);

            return (homeService, mockEmailConfirmation);
        }

        [Fact]
        public void SendEmailAdminReturnNull()
        {
            //Arrange
            var (homeService, mockEmailConfirmation) = CreateHomeService();
            mockEmailConfirmation
                .Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((Task)null);

            //Act
            var result = homeService.SendEmailAdmin(GetTestContactDtoWithAllFields());

            //Assert
            Assert.Null(result);
        }

        private ContactDTO GetTestContactDtoWithAllFields()
        {
            return new ContactDTO()
            {
                Name = "Іван",
                Email = "ivan@gmail.com",
                PhoneNumber = "0935456137",
                FeedBackDescription = "Хотів би стати вашим волонтером."
            };
        }
    }
}