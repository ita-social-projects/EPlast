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
        public async Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationByIdAsync(int ClubId)
        {
            var ClubAdministration = await _repositoryWrapper.ClubAdministration.GetAllAsync(
                predicate: x => x.ClubId == ClubId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(ClubAdministration);
        }

        /// <inheritdoc />
        public async Task<ClubAdministrationDTO> AddAdministratorAsync(ClubAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);

            var admin = new ClubAdministration()
            {
                StartDate = adminDTO.StartDate ?? DateTime.Now,
                EndDate = adminDTO.EndDate,
                AdminTypeId = adminType.ID,
                ClubId = adminDTO.ClubId,
                UserId = adminDTO.UserId
            };

            var user = await _userManager.FindByIdAsync(adminDTO.UserId);
            var role = adminType.AdminTypeName == Roles.kurinHead ? Roles.kurinHead : Roles.kurinSecretary;
            try
            {

            await _userManager.AddToRoleAsync(user, role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionDispatchInfo.Capture(e).Throw();
            }

            await CheckClubHasAdmin(adminDTO.ClubId, adminType.AdminTypeName);

            await _repositoryWrapper.ClubAdministration.CreateAsync(admin);
            await _repositoryWrapper.SaveAsync();
            adminDTO.ID = admin.ID;

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

                _repositoryWrapper.ClubAdministration.Update(admin);
                await _repositoryWrapper.SaveAsync();
            }
            else
            {
                await RemoveAdministratorAsync(adminDTO.ID);
                adminDTO = await AddAdministratorAsync(adminDTO);
            }

            return adminDTO;
        }

        /// <inheritdoc />
        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(u => u.ID == adminId);
            admin.EndDate = DateTime.Now;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            var role = adminType.AdminTypeName == Roles.kurinHead ? Roles.kurinHead : Roles.kurinSecretary;
            await _userManager.RemoveFromRoleAsync(user, role);

            _repositoryWrapper.ClubAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task CheckPreviousAdministratorsToDelete()
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.EndDate <= DateTime.Now);
            var ClubHeadType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.kurinHead);

            foreach (var admin in admins)
            {
                var role = admin.AdminTypeId == ClubHeadType.ID ? Roles.kurinHead : Roles.kurinSecretary;

                var currentAdministration = await _repositoryWrapper.ClubAdministration
                    .GetAllAsync(a => (a.EndDate > DateTime.Now || a.EndDate == null) && a.UserId == admin.UserId);

                if (currentAdministration.All(a => (a.AdminTypeId == ClubHeadType.ID ? Roles.kurinHead : Roles.kurinSecretary) != role)
                    || !currentAdministration.Any())
                {
                    var user = await _userManager.FindByIdAsync(admin.UserId);

                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }
        }

        public async Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == UserId && (a.EndDate > DateTime.Now || a.EndDate == null),
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


        public async Task<IEnumerable<ClubAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == UserId && a.EndDate < DateTime.Now,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.Club)
                 );

            foreach (var admin in admins)
            {
                admin.Club.ClubAdministration = null;
            }

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(admins);
        }

        public async Task<IEnumerable<ClubAdministrationStatusDTO>> GetAdministrationStatuses(string UserId)
        {
            var clubAdmins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == UserId && !a.Status,
                             include:
                             source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.Club)
                             );
            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationStatusDTO>>(clubAdmins);
        }

        public async Task CheckClubHasAdmin(int clubId, string adminTypeName)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminTypeName);
            var admin = await _repositoryWrapper.ClubAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID
                    && (DateTime.Now < a.EndDate || a.EndDate == null) && a.ClubId == clubId);

            if (admin != null)
            {
                await RemoveAdministratorAsync(admin.ID);
            }

        }

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
                await RemoveAdministratorAsync(admin.ID);
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
        public async Task<ClubMembersDTO> AddFollowerAsync(int ClubId, User user)
        {
            var userId = await _userManager.GetUserIdAsync(user);

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

        public async Task RemoveMemberAsync(ClubMembers member)
        {
            _repositoryWrapper.ClubMembers.Delete(member);
            await _repositoryWrapper.SaveAsync();
        }
    }
}
