using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

namespace EPlast.Tests.Services.ActiveMembership
{
    [TestFixture]
    public class PlastDegreeServiceTests
    {
        private PlastDegreeService _activeMembershipService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IUserManagerService> _userManagerService;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userManagerService = new Mock<IUserManagerService>();
            _activeMembershipService = new PlastDegreeService(_mapper.Object, _repoWrapper.Object, _userManagerService.Object);
        }
        [Test]
        public async Task GetDergeesAsync_ReturnsAllDergees()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.PlastDegrees.GetAllAsync(It.IsAny<Expression<Func<PlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<PlastDegree>, IIncludableQueryable<PlastDegree, object>>>()))
                .ReturnsAsync(GetTestPlastDegrees());
            _mapper.Setup(m => m.Map<IEnumerable<PlastDegreeDto>>(It.IsAny<IEnumerable<PlastDegree>>()))
                .Returns(GetTestPlastDegreesDTO());

            // Act
            var result = await _activeMembershipService.GetDegreesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<PlastDegreeDto>>(result);
        }

        [TestCase(7)]
        public async Task CheckDegreeAsync_ReturnsAllowedDergees(int degreeId)
        {
            // Arrange
            List<string> degrees = new List<string>(){ "Старший пластун прихильник / Старша пластунка прихильниця",
                    "Пластун сеніор прихильник / Пластунка сеніорка прихильниця"};
            _repoWrapper
                .Setup(rw => rw.PlastDegrees.GetFirstAsync(It.IsAny<Expression<Func<PlastDegree, bool>>>(), null))
                .ReturnsAsync(new PlastDegree { Id = 1, Name = "Старший пластун прихильник / Старша пластунка прихильниця" });

            // Act
            var result = await _activeMembershipService.CheckDegreeAsync(degreeId, degrees);

            // Assert
            Assert.True(result);
            Assert.NotNull(result);
        }
        [Test]
        public async Task GetDateOfEntry_ReturnsUserDateOfEntry()
        {
            // Arrange
            _userManagerService
                .Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);

            // Act
            var result = await _activeMembershipService.GetDateOfEntryAsync(UserId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<DateTime>(result);
            Assert.AreEqual(UserDateOfEntry, result);

        }

        [Test]
        public async Task GetUserPlastDegreeAsync_ReturnsUserDegree()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.UserPlastDegree.GetAllAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(GetTestUserPlastDegree());
            _mapper.Setup(m => m.Map<UserPlastDegreeDto>(It.IsAny<UserPlastDegree>()))
                .Returns(GetTestUserPlastDegreeDTO());

            // Act
            var result = await _activeMembershipService.GetUserPlastDegreeAsync(UserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UserPlastDegreeDto>(result);
        }

        [Test]
        public async Task AddPlastDegreeForUserAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            _repoWrapper.Setup(x => x.UserPlastDegree.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDto());

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddPlastDegreeForUserAsync_UserHasDegreeWhichHeAlreadyHas()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _mapper.Setup(m => m.Map<UserPlastDegree>(It.IsAny<UserPlastDegreeDto>()))
                .Returns(new UserPlastDegree { PlastDegreeId = 1 });
            _repoWrapper.Setup(x => x.UserPlastDegree.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());

            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDto { PlastDegreeId = 1 });

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddPlastDegreeForUserAsync_UserHasDegreeWhichHeAlreadyHas_ReturnsFalse()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _mapper.Setup(m => m.Map<IEnumerable<UserPlastDegree>>(It.IsAny<IEnumerable<UserPlastDegreeDto>>()))
                .Returns(GetTestUserPlastDegree());
            _repoWrapper.Setup(x => x.UserPlastDegree.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());

            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDto { PlastDegreeId = 1 });

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddPlastDegreeForUserAsync_AddDegreeWhihDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _repoWrapper.Setup(x => x.UserPlastDegree.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());
            _mapper.Setup(m => m.Map<IEnumerable<UserPlastDegree>>(It.IsAny<IEnumerable<UserPlastDegreeDto>>()))
                .Returns(GetTestUserPlastDegree());
            _mapper.Setup(m => m.Map<UserPlastDegree>(It.IsAny<UserPlastDegreePostDto>()))
               .Returns(new UserPlastDegree());
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>()))
                .Returns(new User());
            _repoWrapper.Setup(rw => rw.PlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<PlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<PlastDegree>, IIncludableQueryable<PlastDegree, object>>>()))
                .ReturnsAsync(() => null);
            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDto
            {
                PlastDegreeId = DoesNotExistingId
            });

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddPlastDegreeForUserAsync_AddDegree_ReturnsTrue()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _mapper.Setup(m => m.Map<IEnumerable<UserPlastDegree>>(It.IsAny<IEnumerable<UserPlastDegreeDto>>()))
                .Returns(GetTestUserPlastDegree());
            _mapper.Setup(m => m.Map<UserPlastDegree>(It.IsAny<UserPlastDegreePostDto>()))
               .Returns(new UserPlastDegree());
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>()))
                .Returns(new User());
            _repoWrapper.Setup(rw => rw.PlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<PlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<PlastDegree>, IIncludableQueryable<PlastDegree, object>>>()))
                .ReturnsAsync(new PlastDegree());
            _repoWrapper.Setup(rw => rw.UserPlastDegree.Attach(It.IsAny<UserPlastDegree>()));
            _repoWrapper.Setup(rw => rw.UserPlastDegree.Create(It.IsAny<UserPlastDegree>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDto
            {
                PlastDegreeId = DoesNotExistingId
            });

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task DeletePlastDegreeForUserAsync_DegreeForUserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.UserPlastDegree.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _activeMembershipService.DeletePlastDegreeForUserAsync(UserId, DoesNotExistingId);

            // Assert
            Assert.IsFalse(result);

        }

        [Test]
        public async Task DeletePlastDegreeForUserAsync_RemovesDegreeForUser_ReturnsTrue()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.UserPlastDegree.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());
            _repoWrapper.Setup(rw => rw.UserPlastDegree.Delete(It.IsAny<UserPlastDegree>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _activeMembershipService.DeletePlastDegreeForUserAsync(UserId, DoesNotExistingId);

            // Assert
            Assert.IsTrue(result);
        }

        private string UserId => Guid.NewGuid().ToString();
        private DateTime UserDateOfEntry => DateTime.Today;
        private int DoesNotExistingId => 42;

        private UserDto UserDTO => new UserDto
        {
            Id = UserId,
            RegistredOn = UserDateOfEntry,
            UserPlastDegrees = new UserPlastDegreeDto()
        };

        private IEnumerable<UserPlastDegree> GetTestUserPlastDegree()
        {
            return new List<UserPlastDegree>
            {
               new  UserPlastDegree
               {
                   UserId = UserId,
                   PlastDegreeId = GetTestPlastDegrees().ToList()[0].Id,
                   PlastDegree = GetTestPlastDegrees().ToList()[0]
               },
               new  UserPlastDegree
               {
                   UserId = UserId,
                   PlastDegreeId = GetTestPlastDegrees().ToList()[1].Id,
                   PlastDegree = GetTestPlastDegrees().ToList()[1]
               },
               new  UserPlastDegree
               {
                   UserId = UserId,
                   PlastDegreeId = GetTestPlastDegrees().ToList()[2].Id,
                   PlastDegree = GetTestPlastDegrees().ToList()[2]
               }
            }.AsEnumerable();
        }

        private UserPlastDegreeDto GetTestUserPlastDegreeDTO()
        {

            return new UserPlastDegreeDto
            {
                PlastDegree = new PlastDegreeDto { Name = "Пластприят" }
            };

        }
       
        private IEnumerable<PlastDegree> GetTestPlastDegrees()
        {
            return new List<PlastDegree>
            {
                new PlastDegree{Id = 1, Name = "Старший пластун прихильник / старша пластунка прихильниця"},
                new PlastDegree{Id = 2, Name = "Старший пластун / старша пластунка"},
                new PlastDegree{Id = 3, Name = "Старший пластун скоб/ старша пластунка скоб"}
            }.AsEnumerable();
        }
        private IEnumerable<PlastDegreeDto> GetTestPlastDegreesDTO()
        {
            return new List<PlastDegreeDto>
            {
                new PlastDegreeDto{Id = 1, Name = "Старший пластун прихильник / старша пластунка прихильниця"},
                new PlastDegreeDto{Id = 2, Name = "Старший пластун / старша пластунка"},
                new PlastDegreeDto{Id = 3, Name = "Старший пластун скоб/ старша пластунка скоб"}
            }.AsEnumerable();
        }
    }
}
