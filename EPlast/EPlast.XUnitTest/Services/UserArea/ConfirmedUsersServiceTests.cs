using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.UserArea
{
    public class ConfirmedUsersServiceTests
    {
        private readonly Mock<IEmailSendingService> _emailSendingService;
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IUserStore<User>> _userStoreMock;

        public ConfirmedUsersServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userStoreMock = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _emailSendingService = new Mock<IEmailSendingService>();
        }

        [Fact]
        public async Task CreateTest()
        {
            // Arrange
            _userManager
                .Setup(x => x.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync("userId");
            _repoWrapper
                .Setup(x => x.ConfirmedUser.CreateAsync(It.IsAny<ConfirmedUser>()));
            _repoWrapper
                .Setup(x => x.SaveAsync());
            _emailSendingService
                .Setup(s => s.SendEmailAsync(It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>()))
                .ReturnsAsync(true);
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { Email = "email" });
            var service = new ConfirmedUsersService(_repoWrapper.Object, _userManager.Object, _emailSendingService.Object);

            // Act
            await service.CreateAsync(new User(), "vaucheeId");

            // Assert
            _repoWrapper.Verify(r => r.ConfirmedUser.CreateAsync(It.IsAny<ConfirmedUser>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
        }

        [Fact]
        public async Task DeleteTest()
        {
            // Arrange
            _repoWrapper.Setup(x => x.ConfirmedUser
                                      .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ConfirmedUser, bool>>>(),
                                                              null))
                .ReturnsAsync(new ConfirmedUser() { UserID = "userId" });
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { Email = "email" });
            _repoWrapper
                .Setup(x => x.ConfirmedUser.Delete(It.IsAny<ConfirmedUser>()));
            _repoWrapper
                .Setup(x => x.SaveAsync());
            _emailSendingService
                .Setup(s => s.SendEmailAsync(It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>()))
                .ReturnsAsync(true);
            var service = new ConfirmedUsersService(_repoWrapper.Object,
                                                    _userManager.Object,
                                                    _emailSendingService.Object);

            // Act
            await service.DeleteAsync(new User(), 1);

            // Assert
            _repoWrapper.Verify(r => r.ConfirmedUser.Delete(It.IsAny<ConfirmedUser>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
        }
    }
}
