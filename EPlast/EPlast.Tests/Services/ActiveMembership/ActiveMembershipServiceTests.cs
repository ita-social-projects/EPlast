
using AutoMapper;
using EPlast.BLL.Services.ActiveMembership;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.ActiveMembership
{
    [TestFixture]
    public class ActiveMembershipServiceTests
    {
        private ActiveMembershipService _activeMembershipService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _activeMembershipService = new ActiveMembershipService(_mapper.Object, _repoWrapper.Object);
        }
    }
}
