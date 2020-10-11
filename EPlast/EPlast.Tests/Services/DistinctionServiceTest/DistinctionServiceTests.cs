using AutoMapper;
using EPlast.BLL;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.DistinctionServiceTest
{
    [TestFixture]
    class DistinctionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private DistinctionService distinctionService;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            distinctionService = new DistinctionService(mockMapper.Object, mockRepoWrapper.Object);
        }

        [Test]
        public async Task GetDistinctionAsync_ById_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
                .ReturnsAsync(new Distinction());
            mockMapper.Setup(m => m.Map<DistinctionDTO>(It.IsAny<Distinction>()))
                .Returns(new DistinctionDTO());

            //Act
            var result = await distinctionService.GetDistinctionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetDistinctionAsync_ById_IsWrigthType()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
                .ReturnsAsync(new Distinction());
            mockMapper.Setup(m => m.Map<DistinctionDTO>(It.IsAny<Distinction>()))
                .Returns(new DistinctionDTO());

            //Act
            var result = await distinctionService.GetDistinctionAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<DistinctionDTO>(result);
        }

        [Test]
        public async Task GetDistinctionAsync_ById_Null()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
                .ReturnsAsync(nullDistinction);
            mockMapper.Setup(m => m.Map<DistinctionDTO>(It.IsAny<Distinction>()))
                .Returns(nullDistinctionDTO);

            //Act
            var result = await distinctionService.GetDistinctionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllDistinctionAsync_NotNull()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(GetTestPlastDistinction());
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(GetTestPlastDistinctionDTO());
            
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllDistinctionAsync_IsWrigthType()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(GetTestPlastDistinction());
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(GetTestPlastDistinctionDTO());
            
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            
            //Assert
            Assert.IsInstanceOf<IEnumerable<DistinctionDTO>>(result);
        }

        [Test]
        public async Task GetAllDistinctionAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(nullListDistinction);
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(nullListDistinctionDTO);
            
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            
            //Assert
            Assert.IsEmpty(result);
        }
                
        [Test]
        public async Task DeleteDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(new Distinction());

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Htos`"));
            notAdmin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.DeleteDistinctionAsync(It.IsAny<int>(), notAdmin);
            }
            catch (UnauthorizedAccessException) { exceptions++; }

            //Assert
            Assert.AreEqual(1, exceptions);
            exceptions = 0;
        }

        [Test]
        public async Task DeleteDistinctionAsync_IfAdmin_ThrowsArgumentNullException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(() => null);

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            Admin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.DeleteDistinctionAsync(It.IsAny<int>(), Admin);
            }
            catch (ArgumentNullException) { exceptions++; }

            //Assert
            Assert.AreEqual(1, exceptions);
            exceptions = 0;
        }

        [Test]
        public async Task DeleteDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(new Distinction());

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            Admin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.DeleteDistinctionAsync(It.IsAny<int>(), Admin);
            }
            catch (Exception) { exceptions++; }

            //Assert
            Assert.AreEqual(0, exceptions);
        }

        [Test]
        public async Task ChangeDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(new Distinction());

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Htos`"));
            notAdmin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.ChangeDistinctionAsync(It.IsAny<DistinctionDTO>(), notAdmin);
            }
            catch (UnauthorizedAccessException) { exceptions++; }

            //Assert
            Assert.AreEqual(1, exceptions);
            exceptions = 0;
        }

        [Test]
        public async Task ChangeDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(new Distinction());
            mockMapper.Setup(m => m.Map<DistinctionDTO>(It.IsAny<Distinction>()))
                .Returns(new DistinctionDTO());

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            Admin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.ChangeDistinctionAsync(DistinctionDTO, Admin);
            }
            catch (Exception) { exceptions++; }

            //Assert
            Assert.AreEqual(0, exceptions);
        }

        [Test]
        public async Task AddDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.CreateAsync(It.IsAny<Distinction>()));

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Htos`"));
            notAdmin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.AddDistinctionAsync(It.IsAny<DistinctionDTO>(), notAdmin);
            }
            catch (UnauthorizedAccessException) { exceptions++; }

            //Assert
            Assert.AreEqual(1, exceptions);
            exceptions = 0;
        }

        [Test]
        public async Task AddDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.CreateAsync(It.IsAny<Distinction>()));

            //Act
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            Admin.AddIdentity(claimsIdentity);
            try
            {
                await distinctionService.AddDistinctionAsync(It.IsAny<DistinctionDTO>(), Admin);
            }
            catch (Exception) { exceptions++; }

            //Assert
            Assert.AreEqual(0, exceptions);
        }

        int exceptions = 0;
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        ClaimsPrincipal notAdmin = new ClaimsPrincipal();
        ClaimsPrincipal Admin = new ClaimsPrincipal();
        Distinction nullDistinction = null;
        DistinctionDTO nullDistinctionDTO = null;
        List<DistinctionDTO> nullListDistinctionDTO = null;
        List<Distinction> nullListDistinction = null;
        DistinctionDTO DistinctionDTO = new DistinctionDTO { Id = 1, Name = "За силу" };

        private IEnumerable<Distinction> GetTestPlastDistinction()
        {
            return new List<Distinction>
            {
                new Distinction{Id = 1, Name = "За силу"},
                new Distinction{Id = 2, Name = "За волю"},
                new Distinction{Id = 3, Name = "За народ"}
            }.AsEnumerable();
        }

        private IEnumerable<DistinctionDTO> GetTestPlastDistinctionDTO()
        {
            return new List<DistinctionDTO>
            {
                new DistinctionDTO{Id = 1, Name = "За силу"},
                new DistinctionDTO{Id = 2, Name = "За волю"},
                new DistinctionDTO{Id = 3, Name = "За народ"}
            }.AsEnumerable();
        }
    }
}
