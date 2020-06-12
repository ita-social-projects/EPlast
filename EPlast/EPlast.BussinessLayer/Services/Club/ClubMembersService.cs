using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Club
{
    public class ClubMembersService : IClubMembersService
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ClubMembersService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task ToggleIsApprovedInClubMembersAsync(int memberId, int clubId)
        {
            var person = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.ID == memberId && u.ClubId == clubId);

            if (person != null)
                person.IsApproved = !person.IsApproved;
            
            _repoWrapper.ClubMembers.Update(person);
            await _repoWrapper.SaveAsync();
        }

        public async Task AddFollowerAsync(int index, string userId)
        {
            var oldMember = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(i => i.UserId == userId);

            if (oldMember != null)
            {
                _repoWrapper.ClubMembers.Delete(oldMember);
                await _repoWrapper.SaveAsync();
            }

            ClubMembers newMember = new ClubMembers()
            {
                ClubId = index,
                IsApproved = false,
                UserId = userId
            };

            await _repoWrapper.ClubMembers.CreateAsync(newMember);
            await _repoWrapper.SaveAsync();
        }
    }
}
