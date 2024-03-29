﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.Club
{
    public class ClubParticipantsService : IClubParticipantsService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IAdminTypeService _adminTypeService;
        private readonly UserManager<User> _userManager;


        public ClubParticipantsService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IAdminTypeService adminTypeService,
            UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _adminTypeService = adminTypeService;
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubAdministrationDto>> GetAdministrationByIdAsync(int clubId)
        {
            var ClubAdministration = await _repositoryWrapper.ClubAdministration.GetAllAsync(
                predicate: x => x.ClubId == clubId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDto>>(ClubAdministration);
        }

        /// <inheritdoc />
        public async Task<ClubAdministrationDto> AddAdministratorAsync(ClubAdministrationDto adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            var headType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.KurinHead);
            var headDeputyType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.KurinHeadDeputy);
            adminDTO.Status = DateTime.Today < adminDTO.EndDate || adminDTO.EndDate == null;

            var newAdmin = new ClubAdministration()
            {
                StartDate = adminDTO.StartDate ?? DateTime.Now,
                EndDate = adminDTO.EndDate,
                AdminTypeId = adminType.ID,
                ClubId = adminDTO.ClubId,
                UserId = adminDTO.UserId,
                Status = adminDTO.Status
            };


            if (CheckCityWasAdmin(newAdmin))
            {
                newAdmin.Status = false;
                await _repositoryWrapper.ClubAdministration.CreateAsync(newAdmin);
                await _repositoryWrapper.SaveAsync();
                adminDTO.ID = newAdmin.ID;
                return adminDTO;
            }

            var user = await _userManager.FindByIdAsync(adminDTO.UserId);
            string role = adminType.AdminTypeName switch
            {
                Roles.KurinHead => Roles.KurinHead,
                Roles.KurinHeadDeputy => Roles.KurinHeadDeputy,
                _ => Roles.KurinSecretary,
            };
            try
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionDispatchInfo.Capture(e).Throw();
            }
            if (adminType.AdminTypeName == headType.AdminTypeName)
            {
                var headDeputy = await _repositoryWrapper.ClubAdministration
                    .GetFirstOrDefaultAsync(a => a.AdminTypeId == headDeputyType.ID && a.ClubId == adminDTO.ClubId && a.Status);
                if (headDeputy != null && headDeputy.UserId == adminDTO.UserId)
                {
                    await RemoveAdministratorAsync(headDeputy.ID);
                }
            }

            await CheckClubHasAdminAsync(adminDTO.ClubId, adminType.AdminTypeName, newAdmin);

            await _repositoryWrapper.ClubAdministration.CreateAsync(newAdmin);
            await _repositoryWrapper.SaveAsync();
            adminDTO.ID = newAdmin.ID;

            return adminDTO;
        }

        /// <inheritdoc />
        public async Task<bool?> CheckIsUserApproved(int userId)
        {
            var clubMember = await _repositoryWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.ID == userId);
            return clubMember?.IsApproved;
        }

        /// <inheritdoc />
        public async Task<ClubAdministrationDto> EditAdministratorAsync(ClubAdministrationDto adminDTO)
        {
            var admin = await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(a => a.ID == adminDTO.ID);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);

            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = adminDTO.StartDate ?? DateTime.Now;
                admin.EndDate = adminDTO.EndDate;
                admin.Status = true;

                _repositoryWrapper.ClubAdministration.Update(admin);
                await _repositoryWrapper.SaveAsync();
                return adminDTO;
            }

            await RemoveAdministratorAsync(adminDTO.ID);
            adminDTO = await AddAdministratorAsync(adminDTO);
            return adminDTO;
        }

        /// <inheritdoc />
        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(u => u.ID == adminId);
            admin.EndDate = DateTime.Now;
            admin.Status = false;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            string role = adminType.AdminTypeName switch
            {
                Roles.KurinHead => Roles.KurinHead,
                Roles.KurinHeadDeputy => Roles.KurinHeadDeputy,
                _ => Roles.KurinSecretary,
            };

            if (role != Roles.KurinSecretary || (await CheckUserHasOneSecretaryTypeForClubAsync(admin)))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            _repositoryWrapper.ClubAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task ContinueAdminsDueToDate()
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(x => x.Status);

            foreach (var admin in admins)
            {
                if (admin.EndDate != null && DateTime.Compare((DateTime)admin.EndDate, DateTime.Now) < 0)
                {
                    admin.EndDate = admin.EndDate.Value.AddYears(1);
                    _repositoryWrapper.ClubAdministration.Update(admin);
                }
            }
            await _repositoryWrapper.SaveAsync();
        }

        public async Task<IEnumerable<ClubAdministrationDto>> GetAdministrationsOfUserAsync(string userId)
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == userId && a.Status,
                 include:
                 source => source.Include(c => c.User).Include(a => a.Club).Include(c => c.AdminType)
                 );

            foreach (var admin in admins.Where(x => x.Club != null))
            {
                admin.Club.ClubAdministration = null;
            }

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDto>>(admins);
        }

        public async Task<IEnumerable<ClubAdministrationDto>> GetPreviousAdministrationsOfUserAsync(string userId)
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == userId && !a.Status,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.Club)
                 );

            foreach (var admin in admins)
            {
                admin.Club.ClubAdministration = null;
            }

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDto>>(admins).Reverse();
        }

        public async Task<IEnumerable<ClubAdministrationStatusDto>> GetAdministrationStatuses(string userId)
        {
            var clubAdmins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == userId && !a.Status,
                             include:
                             source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.Club)
                             );
            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationStatusDto>>(clubAdmins);
        }

        public async Task CheckClubHasAdminAsync(int clubId, string adminTypeName, ClubAdministration newAdmin)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminTypeName);
            var admin = await _repositoryWrapper.ClubAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID && a.ClubId == clubId && a.Status);

            newAdmin.Status = false;
            if (admin != null)
            {
                if (newAdmin.EndDate == null || admin.EndDate == null || admin.EndDate < newAdmin.EndDate)
                {
                    await RemoveAdministratorAsync(admin.ID);
                    newAdmin.Status = true;
                }
            }
            else
            {
                newAdmin.Status = true;
            }
        }

        public async Task<IEnumerable<ClubMembersDto>> GetMembersByClubIdAsync(int clubId)
        {
            var сlubMembers = await _repositoryWrapper.ClubMembers.GetAllAsync(
                    predicate: c => c.ClubId == clubId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));

            return _mapper.Map<IEnumerable<ClubMembers>, IEnumerable<ClubMembersDto>>(сlubMembers);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDto> AddFollowerAsync(int clubId, string userId)
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
                await RemoveAdministratorAsync(admin.ID);
            }
            var ClubMember = new ClubMembers()
            {
                ClubId = clubId,
                IsApproved = false,
                UserId = userId,
                User = await _userManager.FindByIdAsync(userId)
            };

            await _repositoryWrapper.ClubMembers.CreateAsync(ClubMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<ClubMembers, ClubMembersDto>(ClubMember);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDto> AddFollowerAsync(int clubId, User user)
        {
            var userId = await _userManager.GetUserIdAsync(user);

            return await AddFollowerAsync(clubId, userId);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDto> ToggleApproveStatusAsync(int memberId)
        {
            var ClubMember = await _repositoryWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.ID == memberId, m => m.Include(u => u.User));

            ClubMember.IsApproved = !ClubMember.IsApproved;

            _repositoryWrapper.ClubMembers.Update(ClubMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<ClubMembers, ClubMembersDto>(ClubMember);
        }

        public async Task<string> ClubOfApprovedMember(string memberId)
        {
            var clubMember = await _repositoryWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.UserId == memberId,
                    m => m.Include(u => u.Club));

            if (clubMember == null)
                return null;
            if (clubMember.IsApproved)
            {
                return clubMember.Club.Name;
            }
            return clubMember.Club.Name = null;
        }

        /// <inheritdoc />
        public async Task RemoveFollowerAsync(int followerId)
        {
            var сlubMember = await _repositoryWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.ID == followerId);
            await UpdateStatusFollowerInHistoryAsync(сlubMember.UserId, true, true);
            _repositoryWrapper.ClubMembers.Delete(сlubMember);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task RemoveMemberAsync(string userId)
        {
            var clubMember = await _repositoryWrapper.ClubMembers.GetFirstOrDefaultAsync(m => m.UserId == userId);
            if (clubMember != null)
            {
                _repositoryWrapper.ClubMembers.Delete(clubMember);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task AddFollowerInHistoryAsync(int clubId, string userId)
        {
            var oldClubMember = await _repositoryWrapper.ClubMemberHistory
               .GetFirstOrDefaultAsync(i => i.UserId == userId && !i.IsDeleted);

            if (oldClubMember != null)
            {
                await UpdateStatusFollowerInHistoryAsync(userId, oldClubMember.IsFollower, true);
            }

            var clubHistoryUser = new ClubMemberHistory()
            {
                Date = DateTime.Now,
                UserId = userId,
                ClubId = clubId,
                IsFollower = true,
                IsDeleted = false
            };

            await _repositoryWrapper.ClubMemberHistory.CreateAsync(clubHistoryUser);
            await _repositoryWrapper.SaveAsync();
        }


        public async Task AddMemberInHistoryAsync(int clubId, string userId)
        {
            var oldClubMember = await _repositoryWrapper.ClubMemberHistory
               .GetFirstOrDefaultAsync(i => i.UserId == userId && !i.IsDeleted);

            if (oldClubMember != null)
            {
                await UpdateStatusFollowerInHistoryAsync(userId, oldClubMember.IsFollower, true);
            }

            var clubHistoryUser = new ClubMemberHistory()
            {
                Date = DateTime.Now,
                UserId = userId,
                ClubId = clubId,
                IsFollower = false,
                IsDeleted = false
            };

            await _repositoryWrapper.ClubMemberHistory.CreateAsync(clubHistoryUser);
            await _repositoryWrapper.SaveAsync();
        }
        public async Task UpdateStatusFollowerInHistoryAsync(string userId, bool isFollower, bool isDeleted)
        {
            var clubHistoryMembers = await _repositoryWrapper.ClubMemberHistory.GetFirstOrDefaultAsync(
                   predicate: c => c.UserId == userId && !c.IsDeleted);

            clubHistoryMembers.IsFollower = isFollower;
            clubHistoryMembers.IsDeleted = isDeleted;
            clubHistoryMembers.Date = DateTime.Now;

            _repositoryWrapper.ClubMemberHistory.Update(clubHistoryMembers);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task RemoveAdminRolesByUserIdAsync(string userId)
        {
            var roles = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == userId && a.Status);
            foreach (var role in roles)
            {
                await RemoveAdministratorAsync(role.ID);
            }
        }

        private async Task<bool> CheckUserHasOneSecretaryTypeForClubAsync(ClubAdministration admin)
        {
            int secretaryAdminTypesCount = 0;
            var userAdminTypes = await GetAdministrationsOfUserAsync(admin.UserId);
            foreach (ClubAdministrationDto userAdminType in userAdminTypes)
            {
                var secretaryCheck = userAdminType.AdminType.AdminTypeName switch
                {
                    Roles.KurinHead => Roles.KurinHead,
                    Roles.KurinHeadDeputy => Roles.KurinHeadDeputy,
                    _ => Roles.KurinSecretary
                };
                if (secretaryCheck == Roles.KurinSecretary) secretaryAdminTypesCount++;
            }
            if (secretaryAdminTypesCount > 1) return false;
            return true;
        }

        private bool CheckCityWasAdmin(ClubAdministration newAdmin)
        {
            return !(newAdmin.EndDate == null || DateTime.Today < newAdmin.EndDate);
        }
    }
}
