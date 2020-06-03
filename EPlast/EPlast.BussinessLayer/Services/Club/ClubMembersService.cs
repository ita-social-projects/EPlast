using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Linq;

namespace EPlast.BussinessLayer.Services.Club
{
    public class ClubMembersService : IClubMembersService
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ClubMembersService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public void ToggleIsApprovedInClubMembers(int memberId, int clubId)
        {
            var person = _repoWrapper.ClubMembers
                .FindByCondition(u => u.ID == memberId && u.ClubId == clubId)
                .FirstOrDefault();

            if (person != null)
            {
                person.IsApproved = !person.IsApproved;
            }

            _repoWrapper.ClubMembers.Update(person);
            _repoWrapper.Save();
        }

        public void AddFollower(int index, string userId)
        {
            ClubMembers oldMember =
                _repoWrapper.ClubMembers
                    .FindByCondition(i => i.UserId == userId)
                    .FirstOrDefault();

            if (oldMember != null)
            {
                _repoWrapper.ClubMembers.Delete(oldMember);
                _repoWrapper.Save();
            }

            ClubMembers newMember = new ClubMembers()
            {
                ClubId = index,
                IsApproved = false,
                UserId = userId
            };

            _repoWrapper.ClubMembers.Create(newMember);
            _repoWrapper.Save();
        }
    }
}
