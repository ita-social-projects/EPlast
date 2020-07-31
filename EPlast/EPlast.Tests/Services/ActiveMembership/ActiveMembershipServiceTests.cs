using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Services.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.ActiveMembership
{
    [TestFixture]
    public class ActiveMembershipServiceTests
    {
        private ActiveMembershipService _activeMembershipService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IUserManagerService> _userManagerService;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userManagerService = new Mock<IUserManagerService>();
            _activeMembershipService = new ActiveMembershipService(_mapper.Object, _repoWrapper.Object, _userManagerService.Object);
        }
        [Test]
        public async Task GetDergeesAsync_ReturnsAllDergees()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.PlastDegrees.GetAllAsync(It.IsAny<Expression<Func<PlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<PlastDegree>, IIncludableQueryable<PlastDegree, object>>>()))
                .ReturnsAsync(GetTestPlastDegrees());
            _mapper.Setup(m => m.Map<IEnumerable<PlastDegreeDTO>>(It.IsAny<IEnumerable<PlastDegree>>()))
                .Returns(GetTestPlastDegreesDTO());

            // Act
            var result = await _activeMembershipService.GetDergeesAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<PlastDegreeDTO>>(result);
        }
        [Test]
        public async Task GetDateOfEntry_ReturnsUserDateOfEntry()
        {
            // Arrange
            _userManagerService
                .Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userDTO);

            // Act
            var result = await _activeMembershipService.GetDateOfEntryAsync(userId);

            // Assert
            Assert.IsInstanceOf<DateTime>(result);
            Assert.AreEqual(userDateOfEntry, result);

        }
        private string userId => Guid.NewGuid().ToString();
        private DateTime userDateOfEntry => DateTime.Today;
        private UserDTO userDTO => new UserDTO
        {
            Id = userId,
            RegistredOn = userDateOfEntry
        };
        private IEnumerable<PlastDegree> GetTestPlastDegrees()
        {
            return new List<PlastDegree>
            {
                new PlastDegree{Id = 1, Name = "Старший пластун прихильник / старша пластунка прихильниця"},
                new PlastDegree{Id = 2, Name = "Старший пластун / старша пластунка"},
                new PlastDegree{Id = 3, Name = "Старший пластун скоб/ старша пластунка скоб"}
            }.AsEnumerable();
        }
        private IEnumerable<PlastDegreeDTO> GetTestPlastDegreesDTO()
        {
            return new List<PlastDegreeDTO>
            {
                new PlastDegreeDTO{Id = 1, Name = "Старший пластун прихильник / старша пластунка прихильниця"},
                new PlastDegreeDTO{Id = 2, Name = "Старший пластун / старша пластунка"},
                new PlastDegreeDTO{Id = 3, Name = "Старший пластун скоб/ старша пластунка скоб"}
            }.AsEnumerable();
        }
    }
}
