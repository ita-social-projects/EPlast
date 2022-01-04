using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Handlers.ClubHandlers;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Club
{
    public class GetClubHistoryMembersHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GetClubHistoryMembersHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _handler = new GetClubHistoryMembersHandler(_repoWrapper.Object, _mapper.Object);
        }

        [Test]
        public async Task GetClubHistoryMembersHandlerTest_ReturnsClubMemberHistoryDTOList()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<ClubMemberHistory, bool>>>(),
                      It.IsAny<Func<IQueryable<ClubMemberHistory>, IIncludableQueryable<ClubMemberHistory, IQueryable<User>>>>()))
                .ReturnsAsync(new List<ClubMemberHistory>());

            _mapper.Setup(m => m.Map<IEnumerable<ClubMemberHistory>, IEnumerable<ClubMemberHistoryDTO>>
                      (It.IsAny<IEnumerable<ClubMemberHistory>>()))
                .Returns(new List<ClubMemberHistoryDTO>());

            // Act
            var responce = await _handler.Handle(It.IsAny<GetClubHistoryMembersQuery>(), It.IsAny<CancellationToken>());

            // Arrange
            Assert.NotNull(responce);
            Assert.IsInstanceOf<List<ClubMemberHistoryDTO>>(responce);
        }
    }
}
