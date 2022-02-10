using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Handlers.DistinctionHandlers;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Distinction
{
    public class AddDistinctionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private Mock<IMapper> _mapper;
        private AddDistinctionHandler _handler;
        private AddDistinctionCommand _query;
        private Mock<UserManager<User>> _userManager;

        private User _user;
        private DistinctionDTO _distinctionDTO;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
            _handler = new AddDistinctionHandler(_mockRepoWrapper.Object, _mapper.Object, _mockMediator.Object);
            _distinctionDTO = new DistinctionDTO();
            _user = new User();
            _query = new AddDistinctionCommand(_distinctionDTO, _user);
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Test]
        public async Task AddDistinctionHandler_Valid()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => x.Distinction.CreateAsync(It.IsAny<DataAccess.Entities.UserEntities.Distinction>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
            Assert.DoesNotThrowAsync(async () => { await _handler.Handle(_query, It.IsAny<CancellationToken>()); });
        }
    }
}
