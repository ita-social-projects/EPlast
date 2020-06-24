using System;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;

namespace EPlast.BussinessLayer.Services.Club
{
    public class ClubMembersService : IClubMembersService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;


        public ClubMembersService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<ClubMembersDTO> ToggleIsApprovedInClubMembersAsync(int memberId, int clubId)
        {
            var person = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.ID == memberId && u.ClubId == clubId);

            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            person.IsApproved = !person.IsApproved;
            _repoWrapper.ClubMembers.Update(person);
            await _repoWrapper.SaveAsync();
            return _mapper.Map<ClubMembers, ClubMembersDTO>(person);
        }

        public async Task<ClubMembersDTO> AddFollowerAsync(int clubId, string userId)
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
                ClubId = clubId,
                IsApproved = false,
                UserId = userId
            };

            await _repoWrapper.ClubMembers.CreateAsync(newMember);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<ClubMembers, ClubMembersDTO>(newMember);
        }
    }
}