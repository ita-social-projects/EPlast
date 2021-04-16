using EPlast.BLL.Services.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        public async Task GetStatusIdTest()
        {
            //Arrange
            string statusName = "Status Name";
            _repoWrapper.Setup(x =>
                    x.ParticipantStatus.GetFirstAsync(It.IsAny<Expression<Func<ParticipantStatus, bool>>>(), null))
                .ReturnsAsync(GetParticipantStatus());
            //Act
            var participantStatusManager = new ParticipantStatusManager(_repoWrapper.Object);
            var methodResult = await participantStatusManager.GetStatusIdAsync(statusName);
            //Assert
            Assert.Equal(1, methodResult);
        }
        public ParticipantStatus GetParticipantStatus()
        {
            return new ParticipantStatus()
            {
                ID = 1,
                ParticipantStatusName = "Status 1"
            };
        }
    }

}

