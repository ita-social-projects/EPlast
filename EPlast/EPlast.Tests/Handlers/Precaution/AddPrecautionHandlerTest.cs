using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Handlers.PrecautionHandlers;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Precaution
{
    public class AddPrecautionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private Mock<IMapper> _mapper;
        private AddPrecautionHandler _handler;
        private AddPrecautionQuery _query;
        private Mock<UserManager<User>> _userManager;

        private User _user;
        private PrecautionDTO _precautionDTO;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
            _handler = new AddPrecautionHandler(_mockRepoWrapper.Object, _mapper.Object, _mockMediator.Object);
            _precautionDTO = new PrecautionDTO();
            _user = new User();
            _query = new AddPrecautionQuery(_precautionDTO, _user);
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Test]
        public async Task AddPrecautionHandler_Valid()
        {
            //Arrange            
            _mockRepoWrapper
                .Setup(x => x.Precaution.CreateAsync(It.IsAny<DataAccess.Entities.UserEntities.Precaution>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
            Assert.DoesNotThrowAsync(async () => { await _handler.Handle(_query, It.IsAny<CancellationToken>()); });
        }
    }
}
