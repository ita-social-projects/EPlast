using System;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;

namespace EPlast.BussinessLayer.Services.Club
{
    public class ClubMembersService : IClubMembersService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IClubService _clubService;
        private readonly IUserManagerService _userManagerService;

        public ClubMembersService(IRepositoryWrapper repoWrapper, IMapper mapper, IClubService clubService, IUserManagerService userManagerService)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _clubService = clubService;
            _userManagerService = userManagerService;
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
            var club = await _clubService.GetClubInfoByIdAsync(clubId);
            var userDto = await _userManagerService.FindByIdAsync(userId)??throw new ArgumentNullException($"User with {userId} id not found");
            
            var oldMember = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(i => i.UserId == userId);

            if (oldMember != null)
            {
                _repoWrapper.ClubMembers.Delete(oldMember);
                await _repoWrapper.SaveAsync();
            }

            ClubMembers newMember = new ClubMembers()
            {
                ClubId = club.ID,
                IsApproved = false,
                UserId = userDto.Id
            };

            await _repoWrapper.ClubMembers.CreateAsync(newMember);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<ClubMembers, ClubMembersDTO>(newMember);
        }
    }
}