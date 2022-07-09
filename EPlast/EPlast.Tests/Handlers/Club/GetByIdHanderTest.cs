using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Handlers;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Handlers.Club
{
    public class GetByIdHanderTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GetByIdHander _handler;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _handler = new GetByIdHander(_repoWrapper.Object, _mapper.Object);
        }

        [Test]
        public async Task GetByIdHanderTest_ReturnsClubDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new ClubDto());

            // Act 
            var responce = await _handler.Handle(It.IsAny<GetByIdQuery>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(responce);
            Assert.IsInstanceOf<ClubDto>(responce);
        }

    }
}
