using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.GoverningBody;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.DTO.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EPlast.BLL.Services.GoverningBodies
{
    public class GoverningBodyAdministrationService : IGoverningBodyAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IAdminTypeService _adminTypeService;
        private readonly IMapper _mapper;

        public GoverningBodyAdministrationService(IRepositoryWrapper repositoryWrapper,
            UserManager<User> userManager,
            IAdminTypeService adminTypeService,
            IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _adminTypeService = adminTypeService;
            _mapper = mapper;
        }

        public async Task<Tuple<IEnumerable<GoverningBodyAdministrationDTO>, int>>
            GetGoverningBodyAdministratorsByPageAsync(int pageNumber, int pageSize)
        {
            var governingBodyAdminType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.GoverningBodyAdmin);

            var tuple = await _repositoryWrapper.GoverningBodyAdministration.GetRangeAsync(
                predicate: admin => admin.AdminTypeId == governingBodyAdminType.ID && admin.Status,
                null,
                null,
                include: GetInclude(),
                pageNumber, pageSize
            );

            var governingBodyAdmins =
                _mapper.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDTO>>(
                    tuple.Item1);

            var rows = tuple.Item2;

            return new Tuple<IEnumerable<GoverningBodyAdministrationDTO>, int>
                (governingBodyAdmins, rows);
        }

        public async Task<IEnumerable<GoverningBodyAdministrationDTO>> GetGoverningBodyAdministratorsAsync()
        {
            var governingBodyAdminType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.GoverningBodyAdmin);

            var governingBodyAdmins =
                await _repositoryWrapper.GoverningBodyAdministration.GetAllAsync(
                    predicate: a => a.Status && a.AdminTypeId == governingBodyAdminType.ID,
                    include: GetInclude());

            return _mapper.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDTO>>(
                governingBodyAdmins);
        }

        public async Task<GoverningBodyAdministrationDTO> AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDTO governingBodyAdministrationDto)
        {
            var user = await _userManager.FindByIdAsync(governingBodyAdministrationDto.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (await _userManager.IsInRoleAsync(user, Roles.GoverningBodyAdmin))
            {
                throw new ArgumentException("User already has GoverningBodyAdmin role");
            }

            if (!userRoles.Contains(Roles.PlastMember))
            {
                throw new ArgumentException($"Can't add user without {Roles.PlastMember} roles");
            }

            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.GoverningBodyAdmin);
            governingBodyAdministrationDto.Status = DateTime.Now < governingBodyAdministrationDto.EndDate || governingBodyAdministrationDto.EndDate == null;

            var governingBodyAdministration = new GoverningBodyAdministration
            {
                StartDate = governingBodyAdministrationDto.StartDate ?? DateTime.Now,
                EndDate = governingBodyAdministrationDto.EndDate,
                AdminTypeId = adminType.ID,
                GoverningBodyId = null,
                UserId = governingBodyAdministrationDto.UserId,
                Status = governingBodyAdministrationDto.Status,
                WorkEmail = user.Email,
                GoverningBodyAdminRole = governingBodyAdministrationDto.GoverningBodyAdminRole
            };

            await _repositoryWrapper.GoverningBodyAdministration.CreateAsync(governingBodyAdministration);
            await _repositoryWrapper.SaveAsync();

            await _userManager.AddToRoleAsync(user, adminType.AdminTypeName);

            return governingBodyAdministrationDto;
        }

        /// <inheritdoc />
        public async Task<GoverningBodyAdministrationDTO> AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDTO governingBodyAdministrationDto)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(governingBodyAdministrationDto.AdminType.AdminTypeName);
            var IsMainStatus = (await _repositoryWrapper.GoverningBody.GetFirstOrDefaultAsync(x => x.ID == governingBodyAdministrationDto.GoverningBodyId)).IsMainStatus;

            governingBodyAdministrationDto.Status = DateTime.Now < governingBodyAdministrationDto.EndDate || governingBodyAdministrationDto.EndDate == null;
            var governingBodyAdministration = new GoverningBodyAdministration
            {
                StartDate = governingBodyAdministrationDto.StartDate ?? DateTime.Now,
                EndDate = governingBodyAdministrationDto.EndDate,
                AdminTypeId = adminType.ID,
                GoverningBodyId = governingBodyAdministrationDto.GoverningBodyId,
                UserId = governingBodyAdministrationDto.UserId,
                Status = governingBodyAdministrationDto.Status,
                WorkEmail = governingBodyAdministrationDto.WorkEmail
            };

            var user = await _userManager.FindByIdAsync(governingBodyAdministrationDto.UserId);

            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Contains(Roles.PlastMember))
            {
                throw new ArgumentException("Can't add user with the roles");
            }

            var adminRole = adminType.AdminTypeName == Roles.GoverningBodyHead ?
                Roles.GoverningBodyHead :
                Roles.GoverningBodySecretary;

            await CheckGoverningBodyHasAdmin(governingBodyAdministrationDto.GoverningBodyId, adminRole);

            await _userManager.AddToRoleAsync(user, adminRole);
            if (IsMainStatus
               && adminType.AdminTypeName == Roles.GoverningBodyHead)
            {
                await _userManager.AddToRoleAsync(user, Roles.GoverningBodyAdmin);
            }

            await _repositoryWrapper.GoverningBodyAdministration.CreateAsync(governingBodyAdministration);
            await _repositoryWrapper.SaveAsync();
            governingBodyAdministrationDto.ID = governingBodyAdministration.Id;

            return governingBodyAdministrationDto;
        }

        /// <inheritdoc />
        public async Task<GoverningBodyAdministrationDTO> EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDTO governingBodyAdministrationDto)
        {
            var admin = await _repositoryWrapper.GoverningBodyAdministration.GetFirstOrDefaultAsync(a => a.Id == governingBodyAdministrationDto.ID);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(governingBodyAdministrationDto.AdminType.AdminTypeName);

            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = governingBodyAdministrationDto.StartDate ?? DateTime.Now;
                admin.EndDate = governingBodyAdministrationDto.EndDate;
                admin.WorkEmail = governingBodyAdministrationDto.WorkEmail;
                admin.GoverningBodyAdminRole = governingBodyAdministrationDto.GoverningBodyAdminRole;

                _repositoryWrapper.GoverningBodyAdministration.Update(admin);
                await _repositoryWrapper.SaveAsync();
            }
            else
            {
                await RemoveAdministratorAsync(governingBodyAdministrationDto.ID);
                governingBodyAdministrationDto = await AddGoverningBodyAdministratorAsync(governingBodyAdministrationDto);
            }

            return governingBodyAdministrationDto;
        }

        /// <inheritdoc />
        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.GoverningBodyAdministration.GetFirstOrDefaultAsync(u => u.Id == adminId);
            admin.EndDate = DateTime.Now;
            admin.Status = false;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var role = adminType.AdminTypeName;

            await _userManager.RemoveFromRoleAsync(user, role);
            if (role == Roles.GoverningBodyHead && userRoles.Contains(Roles.GoverningBodyAdmin))
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.GoverningBodyAdmin);
            }

            _repositoryWrapper.GoverningBodyAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }


        /// <inheritdoc />
        public async Task RemoveMainAdministratorAsync(string userId)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.GoverningBodyAdmin);
            var admin = await _repositoryWrapper.GoverningBodyAdministration.GetFirstOrDefaultAsync(u =>
                u.UserId == userId && u.AdminTypeId == adminType.ID && u.Status);
            
            if (admin != null)
            {
                admin.EndDate = DateTime.Now;

                if (admin.Status)
                {
                    admin.Status = false;
                }

                _repositoryWrapper.GoverningBodyAdministration.Update(admin);
                await _repositoryWrapper.SaveAsync();
            }

            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains(Roles.GoverningBodyAdmin))
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.GoverningBodyAdmin);
            }

            
        }

        public async Task RemoveAdminRolesByUserIdAsync(string userId)
        {
            var roles = await _repositoryWrapper.GoverningBodyAdministration.GetAllAsync(a => a.UserId == userId && a.Status);
            foreach (var role in roles)
            {
                await RemoveAdministratorAsync(role.Id);
            }
        }

        private async Task CheckGoverningBodyHasAdmin(int governingBodyId, string adminTypeName)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminTypeName);
            var admin = await _repositoryWrapper.GoverningBodyAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID
                                            && a.Status && a.GoverningBodyId == governingBodyId);

            if (admin != null)
            {
                await RemoveAdministratorAsync(admin.Id);
            }
        }

        private Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>> GetInclude()
        {
            Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>> expr = x => x.Include(a => a.User).Include(a =>a.AdminType );
            return expr;
        }

        public async Task<IEnumerable<ShortUserInformationDTO>> GetUsersForGoverningBodyAdminFormAsync()
        {
            var users = await _repositoryWrapper.User.GetAllAsync();
            var usersDtos = new List<ShortUserInformationDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(Roles.PlastMember))
                {
                    var isInDeputyRole = await _userManager.IsInRoleAsync(user, Roles.GoverningBodyAdmin);
                    var isInLowerRole = roles.Intersect(Roles.LowerRoles).Any();
                    var shortUser = _mapper.Map<User, ShortUserInformationDTO>(user);
                    shortUser.IsInDeputyRole = isInDeputyRole;
                    shortUser.IsInLowerRole = isInLowerRole;
                    usersDtos.Add(shortUser);
                }
            }

            usersDtos = usersDtos.OrderBy(u => u.IsInDeputyRole||u.IsInLowerRole).ToList();

            return usersDtos;
    }
    }
}
