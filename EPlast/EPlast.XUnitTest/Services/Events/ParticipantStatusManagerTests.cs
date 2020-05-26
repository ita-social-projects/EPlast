using EPlast.BussinessLayer.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class ParticipantStatusManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;

        public ParticipantStatusManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
        }
        [Fact]
        public void GetStatusIdTest()
        {
            //Arrange
            string statusName = "Status Name";
            _repoWrapper.Setup(x => x.ParticipantStatus.FindByCondition(It.IsAny<Expression<Func<ParticipantStatus, bool>>>()))
                .Returns(GetParticipantStatuses());
            //Act
            var participantStatusManager = new ParticipantStatusManager(_repoWrapper.Object);
            var methodResult = participantStatusManager.GetStatusId(statusName);
            //Assert
            Assert.Equal(1, methodResult);
        }
        public IQueryable<ParticipantStatus> GetParticipantStatuses()
        {
            var participantStatuses = new List<ParticipantStatus>
            {
                new ParticipantStatus(){
                    ID = 1,
                    ParticipantStatusName = "Status 1"
                }
            }.AsQueryable();
            return participantStatuses;
        }
    }

}

