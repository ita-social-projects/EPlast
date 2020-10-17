using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Club
{
    public class ClubMembersService : IClubMembersService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IClubAdministrationService _ClubAdministrationService;

        public ClubMembersService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            UserManager<User> userManager,
            IClubAdministrationService ClubAdministrationService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _ClubAdministrationService = ClubAdministrationService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubMembersDTO>> GetMembersByClubIdAsync(int ClubId)
        {
            var ClubMembers = await _repositoryWrapper.ClubMembers.GetAllAsync(
                    predicate: c => c.ClubId == ClubId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));

            return _mapper.Map<IEnumerable<ClubMembers>, IEnumerable<ClubMembersDTO>>(ClubMembers);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDTO> AddFollowerAsync(int ClubId, string userId)
        {
            var oldClubMember = await _repositoryWrapper.ClubMembers
                .GetFirstOrDefaultAsync(i => i.UserId == userId);
            if (oldClubMember != null)
            {
                _repositoryWrapper.ClubMembers.Delete(oldClubMember);
                await _repositoryWrapper.SaveAsync();
            }

            var oldClubAdmins = await _repositoryWrapper.ClubAdministration
                .GetAllAsync(i => i.UserId == userId && (DateTime.Now < i.EndDate || i.EndDate == null));
            foreach (var admin in oldClubAdmins)
            {
                await _ClubAdministrationService.RemoveAdministratorAsync(admin.ID);
            }

            var ClubMember = new ClubMembers()
            {
                ClubId = ClubId,
                IsApproved = false,
                UserId = userId,
                User = await _userManager.FindByIdAsync(userId)
            };

            await _repositoryWrapper.ClubMembers.CreateAsync(ClubMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<ClubMembers, ClubMembersDTO>(ClubMember);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDTO> AddFollowerAsync(int ClubId, ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);

            return await AddFollowerAsync(ClubId, userId);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDTO> ToggleApproveStatusAsync(int memberId)
        {
            var ClubMember = await _repositoryWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.ID == memberId, m => m.Include(u => u.User));

            ClubMember.IsApproved = !ClubMember.IsApproved;

            _repositoryWrapper.ClubMembers.Update(ClubMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<ClubMembers, ClubMembersDTO>(ClubMember);
        }

        /// <inheritdoc />
        public async Task RemoveFollowerAsync(int followerId)
        {
            var ClubMember = await _repositoryWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.ID == followerId);

            _repositoryWrapper.ClubMembers.Delete(ClubMember);
            await _repositoryWrapper.SaveAsync();
        }
    }
}