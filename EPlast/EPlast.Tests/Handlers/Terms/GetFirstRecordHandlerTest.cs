using AutoMapper;
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

namespace EPlast.Tests.Handlers.Terms
{
    public class GetFirstRecordHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetFirstRecordTermsHandler _handler;
        private GetFirstRecordQuery _query;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetFirstRecordTermsHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetFirstRecordQuery();
        }

        [Test]
        public async Task GetFirstRecordTermsHandler_Valid()
        {
            //Arrange
            _mockRepoWrapper.
                Setup(x => x.TermsOfUse.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Terms, bool>>>(),
                                It.IsAny<Func<IQueryable<DataAccess.Entities.Terms>, IIncludableQueryable<DataAccess.Entities.Terms, object>>>()))
                            .ReturnsAsync(new DataAccess.Entities.Terms());
            _mockMapper.Setup(m => m.Map<TermsDTO>(It.IsAny<DataAccess.Entities.Terms>()))
                .Returns(new TermsDTO());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<TermsDTO>(result);
        }
    }
}