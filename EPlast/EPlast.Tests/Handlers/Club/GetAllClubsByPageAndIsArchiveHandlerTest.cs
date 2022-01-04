using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Handlers.ClubHandlers;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Handlers.Club
{
    public class GetAllClubsByPageAndIsArchiveHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GetAllClubsByPageAndIsArchiveHandler _handler;
        private static int number => 1;
        private static string name => "name";
        private static bool isArchived => true;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _handler = new GetAllClubsByPageAndIsArchiveHandler(_repoWrapper.Object, _mapper.Object);
        }

        [Test]
        public async Task GetNotActiveClubHandlerTest_TupleWithIEnumerableClubObjectDTOAndInt()
        {
            //Arrange
            _repoWrapper
              .Setup(x => x.Club.GetRangeAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(),
              It.IsAny<Expression<Func<DataAccessClub.Club, DataAccessClub.Club>>>(), It.IsAny<Expression<Func<DataAccessClub.Club, Object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act 
            var responce = await _handler.Handle(query, It.IsAny<CancellationToken>());
            
            //Assert
            Assert.IsNotNull(responce);
            Assert.IsInstanceOf<Tuple<IEnumerable<ClubObjectDTO>, int>>(responce);
        }

        private GetAllClubsByPageAndIsArchiveQuery query = new GetAllClubsByPageAndIsArchiveQuery(number, number, name, isArchived);
        private List<DataAccessClub.Club> GetClubsByPage()
        {
            return new List<DataAccessClub.Club>()
            {
                new DataAccessClub.Club()
                {
                    Name = "Курінь",
                }
            };
        }

        private Tuple<IEnumerable<DataAccessClub.Club>, int> CreateTuple => new Tuple<IEnumerable<DataAccessClub.Club>, int>(GetClubsByPage(), 100);
    }
}
