using EPlast.BLL.DTO.Terms;
using EPlast.BLL.Handlers.TermsOfUse;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.TermsOfUse;
using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.Tests.Handlers.Terms
{
    public class ChangeTermsHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMediator> _mockMediator;
        private ChangeTermsHandler _handler;
        private ChangeTermsCommand _query;

        private User _user;
        private TermsDTO _termsDto;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMediator = new Mock<IMediator>();
            _handler = new ChangeTermsHandler(_mockRepoWrapper.Object, _mockMediator.Object);
            _user = new User();
            _termsDto = new TermsDTO();
            _query = new ChangeTermsCommand(_termsDto, _user);
        }

        [Test]
        public async Task ChangeTermsHandler_Valid()
        {
            //Arrange
            var terms = new DataAccess.Entities.Terms() {TermsText = "lol"};
            _mockMediator
                .Setup(x => x.Send(It.IsAny<CheckIfAdminForTermsQuery>(), It.IsAny<CancellationToken>()));
            _mockRepoWrapper
                .Setup(x => x.TermsOfUse.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Terms, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.Terms>, IIncludableQueryable<DataAccess.Entities.Terms, object>>>()))
                .ReturnsAsync(terms);
            _mockRepoWrapper.Setup(x => x.TermsOfUse.Update(It.IsAny<DataAccess.Entities.Terms>()));
            _mockRepoWrapper.Setup(x => x.SaveAsync());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
            Assert.DoesNotThrowAsync(async () => { await _handler.Handle(_query, It.IsAny<CancellationToken>()); });
        }
    }
}