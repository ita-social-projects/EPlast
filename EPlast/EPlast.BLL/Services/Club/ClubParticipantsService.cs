using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using EPlast.Resources;

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
        public async Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationByIdAsync(int clubId)
        {
            var ClubAdministration = await _repositoryWrapper.ClubAdministration.GetAllAsync(
                predicate: x => x.ClubId == clubId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(ClubAdministration);
        }

        /// <inheritdoc />
        public async Task<ClubAdministrationDTO> AddAdministratorAsync(ClubAdministrationDTO adminDTO)
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

            var user = await _userManager.FindByIdAsync(adminDTO.UserId);
            string role;
            switch (adminType.AdminTypeName)
            {
                case Roles.KurinHead:
                    role = Roles.KurinHead;
                    break;
                case Roles.KurinHeadDeputy:
                    role = Roles.KurinHeadDeputy;
                    break;
                default:
                    role = Roles.KurinSecretary;
                    break;
            }
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
        public async Task<ClubAdministrationDTO> EditAdministratorAsync(ClubAdministrationDTO adminDTO)
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
            string role;
            switch (adminType.AdminTypeName)
            {
                case Roles.KurinHead:
                    role = Roles.KurinHead;
                    break;
                case Roles.KurinHeadDeputy:
                    role = Roles.KurinHeadDeputy;
                    break;
                default:
                    role = Roles.KurinSecretary;
                    break;
            }
            await _userManager.RemoveFromRoleAsync(user, role);

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

        public async Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationsOfUserAsync(string userId)
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == userId && a.Status,
                 include:
                 source => source.Include(c => c.User).Include(a => a.Club).Include(c => c.AdminType)
                 );

            foreach (var admin in admins)
            {
                if (admin.Club != null)
                {
                    admin.Club.ClubAdministration = null;
                }
            }

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(admins);
        }

        public async Task<IEnumerable<ClubAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string userId)
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == userId && !a.Status,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.Club)
                 );

            foreach (var admin in admins)
            {
                admin.Club.ClubAdministration = null;
            }

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(admins).Reverse();
        }

        public async Task<IEnumerable<ClubAdministrationStatusDTO>> GetAdministrationStatuses(string userId)
        {
            var clubAdmins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == userId && !a.Status,
                             include:
                             source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.Club)
                             );
            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationStatusDTO>>(clubAdmins);
        }

        public async Task CheckClubHasAdminAsync(int clubId, string adminTypeName, ClubAdministration newAdmin)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminTypeName);
            var admin = await _repositoryWrapper.ClubAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID && a.ClubId == clubId && a.Status);
            
            newAdmin.Status = false;
            if (admin != null)
            {
                if (newAdmin.EndDate == null || admin.EndDate < newAdmin.EndDate)
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

        public async Task<IEnumerable<ClubMembersDTO>> GetMembersByClubIdAsync(int clubId)
        {
            var сlubMembers = await _repositoryWrapper.ClubMembers.GetAllAsync(
                    predicate: c => c.ClubId == clubId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));

            return _mapper.Map<IEnumerable<ClubMembers>, IEnumerable<ClubMembersDTO>>(сlubMembers);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDTO> AddFollowerAsync(int clubId, string userId)
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

            return _mapper.Map<ClubMembers, ClubMembersDTO>(ClubMember);
        }

        /// <inheritdoc />
        public async Task<ClubMembersDTO> AddFollowerAsync(int clubId, User user)
        {
            var userId = await _userManager.GetUserIdAsync(user);

            return await AddFollowerAsync(clubId, userId);
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

        public async Task RemoveMemberAsync(ClubMembers member)
        {
            _repositoryWrapper.ClubMembers.Delete(member);
            await _repositoryWrapper.SaveAsync();
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
    }
}
