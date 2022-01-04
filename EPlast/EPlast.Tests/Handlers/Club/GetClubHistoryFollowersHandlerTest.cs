using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Handlers.ClubHandlers;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Club
{
    public class GetClubHistoryFollowersHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GetClubHistoryFollowersHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _handler = new GetClubHistoryFollowersHandler(_repoWrapper.Object, _mapper.Object);
        }

        [Test]
        public async Task GetClubHistoryFollowersHandlerTest_ReturnsClubMemberHistoryDTOList()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<ClubMemberHistory, bool>>>(), null))
                .ReturnsAsync(new List<ClubMemberHistory>());
            _mapper
                .Setup(m => m.Map<IEnumerable<ClubMemberHistory>,
                                  IEnumerable<ClubMemberHistoryDTO>>
                      (It.IsAny<IEnumerable<ClubMemberHistory>>()))
                .Returns(new List<ClubMemberHistoryDTO>());

            // Act
            var responce = await _handler.Handle(It.IsAny<GetClubHistoryFollowersQuery>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(responce);
            Assert.IsInstanceOf<List<ClubMemberHistoryDTO>>(responce);
        }
    }
}
