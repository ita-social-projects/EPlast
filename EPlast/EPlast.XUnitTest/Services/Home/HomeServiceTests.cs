using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Home
{
    public class HomeServiceTests
    {
        public (IHomeService, Mock<IEmailSendingService>) CreateHomeService()
        {
            var mockEmailConfirmation = new Mock<IEmailSendingService>();
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
                .Returns((Task<bool>)null);

            //Act
            var result = homeService.SendEmailAdmin(GetTestContactDtoWithAllFields());

            //Assert
            Assert.Null(result);
        }

        private ContactsDto GetTestContactDtoWithAllFields()
        {
            return new ContactsDto()
            {
                Name = "Іван",
                Email = "ivan@gmail.com",
                PhoneNumber = "0935456137",
                FeedBackDescription = "Хотів би стати вашим волонтером."
            };
        }
    }
}