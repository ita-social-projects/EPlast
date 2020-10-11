using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Services.Distinctions;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.DistinctionServiceTest
{
    [TestFixture]
    class UserDistinctionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private UserDistinctionService distinctionService;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            distinctionService = new UserDistinctionService(mockMapper.Object, mockRepoWrapper.Object);
        }

        [Test]
        public async Task DeleteUserDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Htos`"));
            notAdmin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.DeleteUserDistinctionAsync(It.IsAny<int>(), notAdmin);
            }
            catch (UnauthorizedAccessException) { exceptions++; }

            //Assert
            Assert.AreEqual(1, exceptions);
            exceptions = 0;
        }
                
        [Test]
        public async Task DeleteUserDistinctionAsync_IfAdmin_ThrowsNotImplementedException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(() => null);

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            Admin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.DeleteUserDistinctionAsync(It.IsAny<int>(), Admin);
            }
            catch (NotImplementedException) { exceptions++; }

            //Assert
            Assert.AreEqual(1, exceptions);
            exceptions = 0;
        }

        [Test]
        public async Task DeleteUserDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(userDistinction);

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            Admin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.DeleteUserDistinctionAsync(It.IsAny<int>(), Admin);
            }
            catch (Exception) { exceptions++; }

            //Assert
            Assert.AreEqual(0, exceptions);
        }

        [Test]
        public async Task ChangeUserDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Htos`"));
            notAdmin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.ChangeUserDistinctionAsync(It.IsAny<UserDistinctionDTO>(), notAdmin);
            }
            catch (UnauthorizedAccessException) { exceptions++; }

            //Assert
            Assert.AreEqual(1, exceptions);
            exceptions = 0;
        }

        [Test]
        public async Task ChangeUserDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            Admin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.ChangeUserDistinctionAsync(userDistinctionDTO, Admin);
            }
            catch (Exception) { exceptions++; }

            //Assert
            Assert.AreEqual(0, exceptions);
        }

        [Test]
        public async Task AddUserDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.CreateAsync(It.IsAny<UserDistinction>()));

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Htos`"));
            notAdmin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.AddUserDistinctionAsync(It.IsAny<UserDistinctionDTO>(), notAdmin);
            }
            catch (UnauthorizedAccessException) { exceptions++; }

            //Assert
            Assert.AreEqual(1, exceptions);
            exceptions = 0;
        }

        [Test]
        public async Task AddUserDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.CreateAsync(new UserDistinction()));

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            Admin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.AddUserDistinctionAsync(userDistinctionDTO, Admin);
            }
            catch (Exception) { exceptions++; }

            //Assert
            Assert.AreEqual(0, exceptions);
        }
                
        [Test]
        public async Task IsNumberExistAsync_True()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(new UserDistinctionDTO());

            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task IsNumberExistAsync_False()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nullDistinction);
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(nullDistinctionDTO);

            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsNumberExistAsync_IsWrigthType()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(new UserDistinctionDTO());

            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsWrigthType()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());

            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserDistinctionDTO>>(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());

            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nulluserDistinctions);
            mockMapper
                .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
                .Returns(nulluserDistinctionsDTO);

            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsWrigthType()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());

            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny <string>());

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserDistinctionDTO>>(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());

            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nulluserDistinctions);
            mockMapper
                .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
                .Returns(nulluserDistinctionsDTO);

            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserDistinctionAsync_IsNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nullDistinction);
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(nullDistinctionDTO);

            //Act
            var result = await distinctionService.GetUserDistinctionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        int exceptions = 0;
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        ClaimsPrincipal notAdmin = new ClaimsPrincipal();
        ClaimsPrincipal Admin = new ClaimsPrincipal();
        UserDistinction nullDistinction = null;
        UserDistinctionDTO nullDistinctionDTO = null;
        List<UserDistinction> nulluserDistinctions = null;
        List<UserDistinctionDTO> nulluserDistinctionsDTO = null;
        DistinctionDTO distinctionDTO = new DistinctionDTO { Id = 1, Name = "За силу" };
        UserDTO userDTO = new UserDTO
        {
            FirstName = "Василь",
            LastName = "Кук",
            FatherName = "Петрович",
            PhoneNumber = "0631248596",
            Id = "235",
            ImagePath = "",
            Email = "someemail@gmail.com",
            UserName = "",            
            UserDistinctions = new List<UserDistinctionDTO>(),
            UserProfile = new UserProfileDTO(),
            
        };
        
        private string UserId => Guid.NewGuid().ToString();

        private UserDistinction userDistinction => new UserDistinction
        {
            Id = 1,
            Distinction = new Distinction { Id = 1, Name = "За силу",
                                            UserDistinctions = new List<UserDistinction>() { new UserDistinction() } },
            UserId = UserId,
            Date = DateTime.Now,
            User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName = "",
                                                  PhoneNumber = "", Id = UserId, ImagePath = "", Email = "", UserName = "",
                                                  UserDistinctions = new List<UserDistinction>(),
                                                  UserProfile = new UserProfile() },
            DistinctionId = 1,
            Number = 1,
            Reason = "",
            Reporter = ""            
        };

        private UserDistinctionDTO userDistinctionDTO => new UserDistinctionDTO
        {
            Id = 1,
            Distinction = new DistinctionDTO { Id = 1, Name = "За силу" },
            UserId = UserId,
            Date = DateTime.Now,
            User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName = "",
                                                  Email = "", ID = UserId, ImagePath = "", PhoneNumber = "" },
            DistinctionId = 1,
            Number = 1,
            Reason = "",
            Reporter = ""
        };

        private IEnumerable<UserDistinction> GetTestUserDistinction()
        {
            return new List<UserDistinction>
            {
               new  UserDistinction
               {
                   Distinction = new Distinction{Id = 1, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserDistinction
               {
                   Distinction = new Distinction{Id = 2, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserDistinction
               {
                   Distinction = new Distinction{Id = 3, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               }
            }.AsEnumerable();
        }

        private IEnumerable<UserDistinctionDTO> GetTestUserDistinctionDTO()
        {
            return new List<UserDistinctionDTO>
            {
               new  UserDistinctionDTO
               {
                   Distinction = new DistinctionDTO{Id = 1, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserDistinctionDTO
               {
                   Distinction = new DistinctionDTO{Id = 2, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserDistinctionDTO
               {
                   Distinction = new DistinctionDTO{Id = 3, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName =""}
               }
            }.AsEnumerable();
        }
    }
}
