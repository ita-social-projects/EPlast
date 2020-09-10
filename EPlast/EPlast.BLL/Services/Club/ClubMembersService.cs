using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Club
{
    public class ClubMembersService : IClubMembersService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IClubService _clubService;
        private readonly IUserManagerService _userManagerService;

        public ClubMembersService(IRepositoryWrapper repoWrapper, IMapper mapper, IClubService clubService,
            IUserManagerService userManagerService)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _clubService = clubService;
            _userManagerService = userManagerService;
        }

        /// <inheritdoc />
        public async Task<ClubMembersDTO> ToggleIsApprovedInClubMembersAsync(int memberId, int clubId)
        {
            var person = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(
                    u => u.ID == memberId && u.ClubId == clubId,
                    u => u.Include((x) => x.User));

            if (person == null)
            {
                throw new ArgumentNullException($"User with id={memberId} not found");
            }

            person.IsApproved = !person.IsApproved;
            _repoWrapper.ClubMembers.Update(person);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<ClubMembers, ClubMembersDTO>(person);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDTO> AddFollowerAsync(int clubId, string userId)
        {
            var club = await _clubService.GetClubInfoByIdAsync(clubId);
            var userDto = await _userManagerService.FindByIdAsync(userId) ??
                          throw new ArgumentNullException($"User with {userId} id not found");

            var oldMember = await _repoWrapper.ClubMembers.GetFirstOrDefaultAsync(i => i.UserId == userId);

            if (oldMember != null)
            {
                _repoWrapper.ClubMembers.Delete(oldMember);
                await _repoWrapper.SaveAsync();
            }

            ClubMembers newMember = new ClubMembers() { ClubId = club.ID, IsApproved = false, UserId = userDto.Id };

            await _repoWrapper.ClubMembers.CreateAsync(newMember);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<ClubMembers, ClubMembersDTO>(newMember);
        }

        /// <inheritdoc />
        public async Task RemoveMemberAsync(int memberId)
        {
            var clubMember = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.ID == memberId);

            _repoWrapper.ClubMembers.Delete(clubMember);
            await _repoWrapper.SaveAsync();
        }
    }
}