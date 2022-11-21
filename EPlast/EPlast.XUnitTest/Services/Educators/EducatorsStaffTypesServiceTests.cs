using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.Educators
{
    public class EducatorsStaffTypesServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapper;
        private readonly Mock<IMapper> _mapper;

        public EducatorsStaffTypesServiceTests()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task CreateKVTypeTest()
        {
            //Arrange

            _mapper.Setup(a => a.Map<EducatorsStaffTypesDto, EducatorsStaffTypes> (It.IsAny<EducatorsStaffTypesDto>()))
               .Returns(new EducatorsStaffTypes() { 
                   ID = It.IsAny<int>(),
                   Name = It.IsAny<string>(),
                   UsersKadras = new List<EducatorsStaff>()
               });
            _mapper.Setup(a => a.Map<EducatorsStaffTypes, EducatorsStaffTypesDto>(It.IsAny<EducatorsStaffTypes>()))
               .Returns(new EducatorsStaffTypesDto()
               {
                   ID = It.IsAny<int>(),
                   Name = It.IsAny<string>(),
                   UsersKadras = new List<EducatorsStaff>()
               });
            _repositoryWrapper.Setup(x => x.KVTypes.CreateAsync(It.IsAny<EducatorsStaffTypes>()));
            _repositoryWrapper.Setup(x => x.SaveAsync());

            //Act

            var actionManager = new EducatorsStaffTypesService(_repositoryWrapper.Object, _mapper.Object);
            var methodResult = await actionManager.CreateKVType(It.IsAny<EducatorsStaffTypesDto>());
            
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<EducatorsStaffTypesDto>(methodResult);
        }

        [Fact]
        public async Task GetAllKVTypesAsyncTest()
        {
            //Arrange

            _mapper.Setup(a => a.Map<IEnumerable<EducatorsStaffTypes>, IEnumerable<EducatorsStaffTypesDto>>(It.IsAny<IEnumerable<EducatorsStaffTypes>>()))
               .Returns(new List<EducatorsStaffTypesDto>() {
                new EducatorsStaffTypesDto()
               {
                   ID = It.IsAny<int>(),
                   Name = It.IsAny<string>(),
                   UsersKadras = new List<EducatorsStaff>()
               }
               });

            _repositoryWrapper.Setup(x => x.KVTypes.GetAllAsync(null, null)).ReturnsAsync(GetFakeEducatorsTypes());

            //Act

            var actionManager = new EducatorsStaffTypesService(_repositoryWrapper.Object, _mapper.Object);
            var methodResult = await actionManager.GetAllKVTypesAsync();
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<List<EducatorsStaffTypesDto>>(methodResult);
        }

        [Fact]
        public async Task GetKadrasWithSuchTypeTest()
        {
            //Arrange

            _mapper.Setup(a => a.Map<IEnumerable<EducatorsStaff>, IEnumerable<EducatorsStaffDto>>(It.IsAny<IEnumerable<EducatorsStaff>>()))
               .Returns(new List<EducatorsStaffDto>() { new EducatorsStaffDto() { ID = 1 } });

            _repositoryWrapper.Setup(x => x.KVs.GetAllAsync(null, null)).ReturnsAsync(GetFakeEducators());

            //Act

            var actionManager = new EducatorsStaffTypesService(_repositoryWrapper.Object, _mapper.Object);
            var methodResult = await actionManager.GetKadrasWithSuchType(It.IsAny<EducatorsStaffTypesDto>());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<List<EducatorsStaffDto>>(methodResult);
        }

        [Fact]
        public async Task GetKVsTypeByIdAsyncTest()
        {
            //Arrange

            _mapper.Setup(a => a.Map<EducatorsStaff, EducatorsStaffDto>(It.IsAny<EducatorsStaff>()))
               .Returns(new EducatorsStaffDto());
            _mapper.Setup(a => a.Map<EducatorsStaffTypes, EducatorsStaffTypesDto>(It.IsAny<EducatorsStaffTypes>()))
               .Returns(new EducatorsStaffTypesDto());

            _repositoryWrapper.Setup(x => x.KVs.GetFirstOrDefaultAsync(null, null)).ReturnsAsync(new EducatorsStaff());
            _repositoryWrapper.Setup(x => x.KVTypes.GetFirstOrDefaultAsync(null, null)).ReturnsAsync(new EducatorsStaffTypes());

            //Act

            var actionManager = new EducatorsStaffTypesService(_repositoryWrapper.Object, _mapper.Object);
            var methodResult = await actionManager.GetKVsTypeByIdAsync(It.IsAny<int>());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsAssignableFrom<EducatorsStaffTypesDto>(methodResult);
        }



        private IEnumerable<EducatorsStaffTypes> GetFakeEducatorsTypes()
        {
            return new List<EducatorsStaffTypes>()
            {
                new EducatorsStaffTypes
                {
                    Name = "Course"
                }
            }.AsEnumerable();
        }
        private IEnumerable<EducatorsStaff> GetFakeEducators()
        {
            return new List<EducatorsStaff>()
            {
                new EducatorsStaff
                {
                    ID = 1
                }
            }.AsEnumerable();
        }



    }
}
