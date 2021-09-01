using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
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
    public class PlastDegreeServiceTests
    {
        private PlastDegreeService _activeMembershipService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IUserManagerService> _userManagerService;
        private IUniqueIdService _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userManagerService = new Mock<IUserManagerService>();
            _uniqueId = new UniqueIdService();
            _activeMembershipService = new PlastDegreeService(_mapper.Object, _repoWrapper.Object, _userManagerService.Object);
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
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<PlastDegreeDTO>>(result);
        }

        [TestCase(7)]
        public async Task GetDergeeAsync_ReturnsAllowedDergees(int degreeId)
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
        public async Task GetUserPlastDegreesAsync_ReturnsAllUserDegrees()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.GetAllAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(GetTestUserPlastDegrees());
            _mapper.Setup(m => m.Map<IEnumerable<UserPlastDegreeDTO>>(It.IsAny<IEnumerable<UserPlastDegree>>()))
                .Returns(GetTestUserPlastDegreesDTO());

            // Act
            var result = await _activeMembershipService.GetUserPlastDegreesAsync(UserId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<UserPlastDegreeDTO>>(result);

        }


        [Test]
        public async Task AddPlastDegreeForUserAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            _repoWrapper.Setup(x => x.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDTO());

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddPlastDegreeForUserAsync_UserHasDegreeWhichHeAlreadyHas_ReturnsFalse()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _mapper.Setup(m => m.Map<IEnumerable<UserPlastDegree>>(It.IsAny<IEnumerable<UserPlastDegreeDTO>>()))
                .Returns(GetTestUserPlastDegrees());
            _repoWrapper.Setup(x => x.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());

            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDTO { PlastDegreeId = 1 });

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddPlastDegreeForUserAsync_AddDegreeWhihDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _userManagerService.Setup(ums => ums.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UserDTO);
            _repoWrapper.Setup(x => x.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());
            _mapper.Setup(m => m.Map<IEnumerable<UserPlastDegree>>(It.IsAny<IEnumerable<UserPlastDegreeDTO>>()))
                .Returns(GetTestUserPlastDegrees());
            _mapper.Setup(m => m.Map<UserPlastDegree>(It.IsAny<UserPlastDegreePostDTO>()))
               .Returns(new UserPlastDegree());
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDTO>()))
                .Returns(new User());
            _repoWrapper.Setup(rw => rw.PlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<PlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<PlastDegree>, IIncludableQueryable<PlastDegree, object>>>()))
                .ReturnsAsync(() => null);
            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDTO
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
            _mapper.Setup(m => m.Map<IEnumerable<UserPlastDegree>>(It.IsAny<IEnumerable<UserPlastDegreeDTO>>()))
                .Returns(GetTestUserPlastDegrees());
            _mapper.Setup(m => m.Map<UserPlastDegree>(It.IsAny<UserPlastDegreePostDTO>()))
               .Returns(new UserPlastDegree());
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDTO>()))
                .Returns(new User());
            _repoWrapper.Setup(rw => rw.PlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<PlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<PlastDegree>, IIncludableQueryable<PlastDegree, object>>>()))
                .ReturnsAsync(new PlastDegree());
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.Attach(It.IsAny<UserPlastDegree>()));
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.Create(It.IsAny<UserPlastDegree>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _activeMembershipService.AddPlastDegreeForUserAsync(new UserPlastDegreePostDTO
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
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
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
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.Delete(It.IsAny<UserPlastDegree>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _activeMembershipService.DeletePlastDegreeForUserAsync(UserId, DoesNotExistingId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddEndDateForUserPlastDegreeAsync_DegreeForUserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _activeMembershipService.AddEndDateForUserPlastDegreeAsync(It.IsAny<UserPlastDegreePutDTO>());

            // Assert
            Assert.IsFalse(result);

        }

        [Test]
        public async Task AddEndDateForUserPlastDegreeAsync_AddsEndDateDegreeForUser_ReturnsTrue()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.Update(It.IsAny<UserPlastDegree>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _activeMembershipService.AddEndDateForUserPlastDegreeAsync(new UserPlastDegreePutDTO());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SetPlastDegreeForUserAsCurrentAsync_DegreeForUserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _activeMembershipService.SetPlastDegreeForUserAsCurrentAsync(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.IsFalse(result);

        }

        [Test]
        public async Task SetPlastDegreeForUserAsCurrentAsync_SetsDegreeAsCurrent_ReturnsTrue()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new UserPlastDegree());
            _repoWrapper.Setup(rw => rw.UserPlastDegrees.Update(It.IsAny<UserPlastDegree>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _activeMembershipService.SetPlastDegreeForUserAsCurrentAsync(UserId, DoesNotExistingId);

            // Assert
            Assert.IsTrue(result);
        }
        private string UserId => _uniqueId.GetUniqueId().ToString();
        private DateTime UserDateOfEntry => DateTime.Today;
        private int DoesNotExistingId => 42;

        private UserDTO UserDTO => new UserDTO
        {
            Id = UserId,
            RegistredOn = UserDateOfEntry,
            UserPlastDegrees = new List<UserPlastDegreeDTO>
            {
                new UserPlastDegreeDTO()
            }
        };

        private IEnumerable<UserPlastDegree> GetTestUserPlastDegrees()
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
        private IEnumerable<UserPlastDegreeDTO> GetTestUserPlastDegreesDTO()
        {
            return new List<UserPlastDegreeDTO>
            {
               new  UserPlastDegreeDTO
               {
                   PlastDegree = GetTestPlastDegreesDTO().ToList()[0]
               },
               new  UserPlastDegreeDTO
               {
                   PlastDegree = GetTestPlastDegreesDTO().ToList()[1]
               },
               new  UserPlastDegreeDTO
               {
                   PlastDegree = GetTestPlastDegreesDTO().ToList()[2]
               }
            }.AsEnumerable();
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
