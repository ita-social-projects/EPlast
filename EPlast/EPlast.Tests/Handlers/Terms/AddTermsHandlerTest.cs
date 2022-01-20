using AutoMapper;
using EPlast.BLL.Commands.TermsOfUse;
using EPlast.BLL.DTO.Terms;
using EPlast.BLL.Handlers.TermsOfUse;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Terms
{
    public class AddTermsHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private Mock<IMediator> _mockMediator;
        private AddTermsHandler _handler;
        private AddTermsCommand _query;

        private User _user;
        private TermsDTO _termsDto;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
            _handler = new AddTermsHandler(_mockRepoWrapper.Object, _mockMapper.Object, _mockMediator.Object);
            _user = new User();
            _termsDto = new TermsDTO();
            _query = new AddTermsCommand(_termsDto, _user);
        }

        [Test]
        public async Task AddTermsHandler_Valid()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => x.TermsOfUse.CreateAsync(It.IsAny<DataAccess.Entities.Terms>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
            Assert.DoesNotThrowAsync(async () => { await _handler.Handle(_query, It.IsAny<CancellationToken>()); });
        }
    }
}