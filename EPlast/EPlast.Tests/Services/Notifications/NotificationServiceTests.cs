using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.Notifications;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Notifications
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private Mock<IMapper> _mapper;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IUserManagerService> _userManagerService;
        private NotificationService _notificationService;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userManagerService = new Mock<IUserManagerService>();
            _notificationService = new NotificationService(_mapper.Object, _repoWrapper.Object, _userManagerService.Object);
        }

        [Test]
        public async Task GetAllNotificationTypesAsync_Valid_ReturnsTypes()
        {
            _repoWrapper.Setup(m => m.NotificationTypes.GetAllAsync(It.IsAny<Expression<Func<NotificationType, bool>>>(),
                    It.IsAny<Func<IQueryable<NotificationType>, IIncludableQueryable<NotificationType, object>>>())).ReturnsAsync(new List<NotificationType>());

            _mapper.Setup(m => m.Map<IEnumerable<NotificationTypeDto>>(It.IsAny<IEnumerable<NotificationType>>()))
                .Returns(new List<NotificationTypeDto>());

            //Act
            var result = await _notificationService.GetAllNotificationTypesAsync();
            //Assert
            Assert.IsInstanceOf<IEnumerable<NotificationTypeDto>>(result);

            _repoWrapper.Verify(f => f.NotificationTypes.GetAllAsync(It.IsAny<Expression<Func<NotificationType, bool>>>(),
                    It.IsAny<Func<IQueryable<NotificationType>, IIncludableQueryable<NotificationType, object>>>()));

            _mapper.Verify(f => f.Map<IEnumerable<NotificationTypeDto>>(It.IsAny<IEnumerable<NotificationType>>()));
        }

        [Test]
        public async Task GetAllUserNotificationsAsync_Valid_ReturnsListNotify()
        {
            _repoWrapper.Setup(m => m.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                    It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>())).ReturnsAsync(new List<UserNotification>());

            _mapper.Setup(m => m.Map<IEnumerable<UserNotificationDto>>(It.IsAny<IEnumerable<UserNotification>>()))
                .Returns(new List<UserNotificationDto>());

            _repoWrapper.Setup(m => m.SaveAsync());

            //Act
            var result = await _notificationService.GetAllUserNotificationsAsync("");
            //Assert
            Assert.IsInstanceOf<IEnumerable<UserNotificationDto>>(result);
           
            _repoWrapper.Verify(f => f.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                    It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>()));

            _mapper.Verify(f => f.Map<IEnumerable<UserNotificationDto>>(It.IsAny<IEnumerable<UserNotification>>()));
        }

        [Test]
        public async Task AddListUserNotificationAsync_Valid_ReturnsListNotify()
        {
            _userManagerService
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserDto());

            _mapper.Setup(m => m.Map<IEnumerable<UserNotificationDto>, IEnumerable<UserNotification>>(It.IsAny<IEnumerable<UserNotificationDto>>()))
                .Returns(GetTestUserNotification());
            _mapper.Setup(m => m.Map<IEnumerable<UserNotificationDto>>(It.IsAny<IEnumerable<UserNotificationDto>>()))
                .Returns(GetTestUserNotificationDTO());

            _repoWrapper
                .Setup(m => m.NotificationTypes.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<NotificationType, bool>>>(),
                    It.IsAny<Func<IQueryable<NotificationType>,
                    IIncludableQueryable<NotificationType, object>>>()))
                .ReturnsAsync(new NotificationType());

            _repoWrapper.Setup(m => m.UserNotifications.CreateAsync(It.IsAny<UserNotification>()));

            _repoWrapper.Setup(m => m.SaveAsync());

            //Act
            var result = await _notificationService.AddListUserNotificationAsync(GetTestUserNotificationDTO());

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserNotificationDto>>(result);

            _mapper.Verify(f => f.Map<IEnumerable<UserNotificationDto>, IEnumerable<UserNotification>>(It.IsAny<IEnumerable<UserNotificationDto>>()), Times.Once);
            _repoWrapper.Verify(f => f.UserNotifications.CreateAsync(It.IsAny<UserNotification>()), Times.Exactly(3));
            _repoWrapper.Verify(f => f.SaveAsync(), Times.Once);
            _mapper.Verify(f => f.Map<IEnumerable<UserNotificationDto>>(It.IsAny<IEnumerable<UserNotification>>()));
        }

        [Test]
        public void AddListUserNotificationAsync_InValidType_ThrowException()
        {
            _userManagerService
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserDto());

            _mapper.Setup(m => m.Map<IEnumerable<UserNotificationDto>, IEnumerable<UserNotification>>(It.IsAny<IEnumerable<UserNotificationDto>>()))
                .Returns(GetTestUserNotification());
            _mapper.Setup(m => m.Map<IEnumerable<UserNotificationDto>>(It.IsAny<IEnumerable<UserNotificationDto>>()))
                .Returns(GetTestUserNotificationDTO());

            _repoWrapper
                .Setup(m => m.NotificationTypes.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<NotificationType, bool>>>(),
                    It.IsAny<Func<IQueryable<NotificationType>, 
                    IIncludableQueryable<NotificationType, object>>>()))
                .ReturnsAsync((NotificationType)null);

            //Assert & Act 
            Assert.ThrowsAsync<InvalidOperationException>(() => _notificationService.AddListUserNotificationAsync(GetTestUserNotificationDTO()));

            _mapper.Verify(f => f.Map<IEnumerable<UserNotificationDto>, IEnumerable<UserNotification>>(It.IsAny<IEnumerable<UserNotificationDto>>()), Times.Once);
            _repoWrapper.Verify(f => f.UserNotifications.CreateAsync(It.IsAny<UserNotification>()), Times.Never);
            _repoWrapper.Verify(f => f.SaveAsync(), Times.Never);
            _mapper.Verify(f => f.Map<IEnumerable<UserNotificationDto>>(It.IsAny<IEnumerable<UserNotification>>()), Times.Never);
        }

        [Test]
        public void AddListUserNotificationAsync_InValidUserId_ThrowException()
        {
            _userManagerService
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDto)null);

            _mapper
                .Setup(m => m.Map<IEnumerable<UserNotificationDto>, IEnumerable<UserNotification>>(It.IsAny<IEnumerable<UserNotificationDto>>()))
                .Returns(GetTestUserNotification());

            //Assert & Act 
            Assert.ThrowsAsync<InvalidOperationException>(() => _notificationService.AddListUserNotificationAsync(GetTestUserNotificationDTO()));

            _userManagerService.Verify(f => f.FindByIdAsync(It.IsAny<string>()), Times.Exactly(3));

            _mapper.Verify(f => f.Map<IEnumerable<UserNotificationDto>, IEnumerable<UserNotification>>(It.IsAny<IEnumerable<UserNotificationDto>>()), Times.Once);
            _repoWrapper.Verify(f => f.UserNotifications.CreateAsync(It.IsAny<UserNotification>()), Times.Never);
            _repoWrapper.Verify(f => f.SaveAsync(), Times.Never);
            _mapper.Verify(f => f.Map<IEnumerable<UserNotificationDto>>(It.IsAny<IEnumerable<UserNotification>>()), Times.Never);
        }

        [Test]
        public async Task SetCheckForListNotificationAsync_Valid_ReturnsTrue()
        {
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDto());

            _repoWrapper.Setup(m => m.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
        It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>())).ReturnsAsync(GetTestUserNotification().Where(c => c.OwnerUserId == "1"));

            _repoWrapper.Setup(m => m.UserNotifications.Update(It.IsAny<UserNotification>()));

            _repoWrapper.Setup(m => m.SaveAsync());

            //Act 
            var result = await _notificationService.SetCheckForListNotificationAsync("1");

            //Assert
            Assert.IsTrue(result);

            _repoWrapper.Verify(f => f.UserNotifications.Update(It.IsAny<UserNotification>()), Times.Exactly(1));

            _repoWrapper.Verify(f => f.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
        It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>()), Times.Exactly(1));

            _repoWrapper.Verify(f => f.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task SetCheckForListNotificationAsync_InValid_ReturnsFalse()
        {
            _userManagerService.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((UserDto)null);

            _repoWrapper.Setup(m => m.SaveAsync());

            //Act 
            var result = await _notificationService.SetCheckForListNotificationAsync("1");

            //Assert
            Assert.IsFalse(result);

            _repoWrapper.Verify(f => f.UserNotifications.Update(It.IsAny<UserNotification>()), Times.Never);

            _repoWrapper.Verify(f => f.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>()), Times.Never);

            _repoWrapper.Verify(f => f.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task RemoveUserNotificationAsync_Valid_ReturnsTrue()
        {
            _repoWrapper.Setup(m => m.UserNotifications.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>())).ReturnsAsync(new UserNotification());

            _repoWrapper.Setup(m => m.UserNotifications.Delete(It.IsAny<UserNotification>()));

            _repoWrapper.Setup(m => m.SaveAsync());

            //Act 
            var result = await _notificationService.RemoveUserNotificationAsync(1);

            //Assert
            Assert.IsTrue(result);

            _repoWrapper.Verify(f => f.UserNotifications.Delete(It.IsAny<UserNotification>()), Times.Once);

            _repoWrapper.Verify(f => f.UserNotifications.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>()), Times.Once);

            _repoWrapper.Verify(f => f.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveUserNotificationAsync_InValid_ReturnsFalse()
        {
            _repoWrapper.Setup(m => m.UserNotifications.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>())).ReturnsAsync((UserNotification)null);

            //Act 
            var result = await _notificationService.RemoveUserNotificationAsync(1);

            //Assert
            Assert.IsFalse(result);

            _repoWrapper.Verify(f => f.UserNotifications.Delete(It.IsAny<UserNotification>()), Times.Never);

            _repoWrapper.Verify(f => f.UserNotifications.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>()), Times.Once);

            _repoWrapper.Verify(f => f.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task RemoveAllUserNotificationAsync_Valid_ReturnsTrue()
        {
            _repoWrapper.Setup(m => m.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>())).ReturnsAsync(GetTestUserNotification());

            _repoWrapper.Setup(m => m.UserNotifications.Delete(It.IsAny<UserNotification>()));

            _repoWrapper.Setup(m => m.SaveAsync());

            //Act 
            var result = await _notificationService.RemoveAllUserNotificationAsync("1");

            //Assert
            Assert.IsTrue(result);

            _repoWrapper.Verify(f => f.UserNotifications.Delete(It.IsAny<UserNotification>()), Times.Exactly(3));

            _repoWrapper.Verify(f => f.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>()), Times.Once);

            _repoWrapper.Verify(f => f.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveAllUserNotificationAsync_InValid_ReturnsFalse()
        {
            _repoWrapper.Setup(m => m.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>())).ReturnsAsync(new List<UserNotification>());

            //Act 
            var result = await _notificationService.RemoveAllUserNotificationAsync("1");

            //Assert
            Assert.IsFalse(result);

            _repoWrapper.Verify(f => f.UserNotifications.Delete(It.IsAny<UserNotification>()), Times.Never);

            _repoWrapper.Verify(f => f.UserNotifications.GetAllAsync(It.IsAny<Expression<Func<UserNotification, bool>>>(),
                 It.IsAny<Func<IQueryable<UserNotification>, IIncludableQueryable<UserNotification, object>>>()), Times.Once);

            _repoWrapper.Verify(f => f.SaveAsync(), Times.Never);
        }


        private IEnumerable<UserNotificationDto> GetTestUserNotificationDTO()
        {
            return new List<UserNotificationDto>
            {
               new  UserNotificationDto
               {
                   OwnerUserId="1",
                   Id=1,
                   NotificationTypeId = 1,
                   Message="New message #1"
               },
               new  UserNotificationDto
               {
                   OwnerUserId="2",
                   Id=2,
                   NotificationTypeId = 1,
                   Message="New message #2"
               },
               new  UserNotificationDto
               {
                   OwnerUserId="3",
                   Id=3,
                   NotificationTypeId = 2,
                   Message="New message #3"
               }
            }.AsEnumerable();
        }

        private IEnumerable<UserNotification> GetTestUserNotification()
        {
            return new List<UserNotification>
            {
               new  UserNotification
               {
                   OwnerUserId="1",
                   Id=1,
                   NotificationTypeId = 1,
                   Message="New message #1",
                   Checked = false
               },
               new  UserNotification
               {
                   OwnerUserId="2",
                   Id=2,
                   NotificationTypeId = 1,
                   Message="New message #2"
               },
               new  UserNotification
               {
                   OwnerUserId="3",
                   Id=3,
                   NotificationTypeId = 2,
                   Message="New message #3"
               }
            }.AsEnumerable();
        }
    }
}
