using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
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

        public async Task<Tuple<IEnumerable<GoverningBodyAdministrationDto>, int>> GetGoverningBodyAdministratorsByPageAsync(int pageNumber, int pageSize)
        {
            var governingBodyAdminType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.GoverningBodyAdmin);

            var tuple = await _repositoryWrapper.GoverningBodyAdministration.GetRangeAsync(
                predicate: admin => admin.AdminTypeId == governingBodyAdminType.ID && admin.Status,
                null,
                null,
                include: Include,
                pageNumber, pageSize
            );

            var governingBodyAdmins =
                _mapper.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDto>>(
                    tuple.Item1);

            var rows = tuple.Item2;

            return new Tuple<IEnumerable<GoverningBodyAdministrationDto>, int>
                (governingBodyAdmins, rows);
        }

        public async Task<IEnumerable<GoverningBodyAdministrationDto>> GetGoverningBodyAdministratorsAsync()
        {
            var governingBodyAdminType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.GoverningBodyAdmin);

            var governingBodyAdmins =
                await _repositoryWrapper.GoverningBodyAdministration.GetAllAsync(
                    predicate: a => a.Status && a.AdminTypeId == governingBodyAdminType.ID,
                    include: Include);

            return _mapper.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDto>>(
                governingBodyAdmins);
        }

        public async Task<GoverningBodyAdministrationDto> AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDto governingBodyAdministrationDto)
        {
            var user = await _userManager.FindByIdAsync(governingBodyAdministrationDto.UserId);

            if (await CheckRoleNameExistsAsync(governingBodyAdministrationDto.GoverningBodyAdminRole))
            {
                throw new ArgumentException("This role name of GoverningBodyAdmin already exists");
            }

            if (await _userManager.IsInRoleAsync(user, Roles.GoverningBodyAdmin))
            {
                throw new ArgumentException("User already has GoverningBodyAdmin role");
            }

            if (!await _userManager.IsInRoleAsync(user, Roles.PlastMember))
            {
                throw new ArgumentException("User is not a plast member");
            }

            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.GoverningBodyAdmin);
            governingBodyAdministrationDto.Status = governingBodyAdministrationDto.EndDate == null || DateTime.Now < governingBodyAdministrationDto.EndDate;

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

            if (governingBodyAdministration.Status)
            {
                await RemoveExistingGbAdminsAsync(governingBodyAdministrationDto.GoverningBodyId, Roles.GoverningBodyAdmin);
                await _userManager.AddToRoleAsync(user, Roles.GoverningBodyAdmin);
            }

            await _repositoryWrapper.GoverningBodyAdministration.CreateAsync(governingBodyAdministration);
            await _repositoryWrapper.SaveAsync();

            return governingBodyAdministrationDto;
        }

        public async Task<GoverningBodyAdministrationDto> AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDto governingBodyAdministrationDto)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(governingBodyAdministrationDto.AdminType.AdminTypeName);
            var IsMainStatus = (await _repositoryWrapper.GoverningBody.GetFirstOrDefaultAsync(x => x.ID == governingBodyAdministrationDto.GoverningBodyId)).IsMainStatus;

            governingBodyAdministrationDto.Status = governingBodyAdministrationDto.EndDate == null || DateTime.Now < governingBodyAdministrationDto.EndDate;
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

            if (!await _userManager.IsInRoleAsync(user, Roles.PlastMember))
            {
                throw new ArgumentException("User is not a plast member");
            }

            if (governingBodyAdministration.Status)
            {
                if (IsMainStatus && adminType.AdminTypeName == Roles.GoverningBodyHead)
                {
                    await RemoveExistingGbAdminsAsync(governingBodyAdministrationDto.GoverningBodyId, Roles.GoverningBodyAdmin);
                    await _userManager.AddToRoleAsync(user, Roles.GoverningBodyAdmin);
                }
                else
                {
                    await RemoveExistingGbAdminsAsync(governingBodyAdministrationDto.GoverningBodyId, adminType.AdminTypeName);
                    await _userManager.AddToRoleAsync(user, Roles.GoverningBodyAdmin);
                }
            }

            await _repositoryWrapper.GoverningBodyAdministration.CreateAsync(governingBodyAdministration);
            await _repositoryWrapper.SaveAsync();

            governingBodyAdministrationDto.ID = governingBodyAdministration.Id;
            return governingBodyAdministrationDto;
        }


        public async Task<GoverningBodyAdministrationDto> EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDto governingBodyAdministrationDto)
        {
            if (await CheckRoleNameExistsAsync(governingBodyAdministrationDto.GoverningBodyAdminRole))
            {
                throw new ArgumentException("This role name of GoverningBodyAdmin already exists");
            }

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

        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.GoverningBodyAdministration.GetFirstOrDefaultAsync(u => u.Id == adminId);
            if (admin == null) return;

            admin.EndDate = DateTime.Now;
            admin.Status = false;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);

            var user = await _userManager.FindByIdAsync(admin.UserId);

            string role = adminType.AdminTypeName switch
            {
                Roles.GoverningBodyAdmin => Roles.GoverningBodyAdmin,
                _ => Roles.GoverningBodySecretary,
            };

            if (role != Roles.GoverningBodySecretary || (await CheckUserHasOneSecretaryTypeForGoverningBodyAsync(admin)))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            _repositoryWrapper.GoverningBodyAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();

            var countOfSectorAdministrations = await _repositoryWrapper.GoverningBodySectorAdministration.GetAllAsync(
                u => u.UserId == admin.UserId && u.Status);
            var countOfGbAdministrations = await _repositoryWrapper.GoverningBodyAdministration.GetAllAsync(
                u => u.UserId == admin.UserId && u.Status);
            if (countOfGbAdministrations.Any() && countOfSectorAdministrations.Any())
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.GoverningBodyAdmin);
            }
        }

        public async Task<IEnumerable<GoverningBodyAdministrationDto>> GetUserAdministrations(string userId)
        {

            var secretaries = await _repositoryWrapper.GoverningBodyAdministration.GetAllAsync(a => a.UserId == userId && a.Status,
                include: Include);

            return _mapper.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDto>>(secretaries);
        }

        public async Task RemoveMainAdministratorAsync(string userId)
        {
            var adminTypesToRemove = await _repositoryWrapper.AdminType.GetAllAsync(
                at => at.AdminTypeName == Roles.GoverningBodyAdmin ||
                      at.AdminTypeName == Roles.GoverningBodyHead    
            );

            foreach (var adminTypeToRemove in adminTypesToRemove)
            {
                var admins = await _repositoryWrapper.GoverningBodyAdministration
                .GetAllAsync(u =>
                    u.UserId == userId
                    && u.AdminTypeId == adminTypeToRemove.ID
                    && u.Status
                );
                foreach (var admin in admins)
                {
                    admin.EndDate = DateTime.Now;
                    admin.Status = false;

                    _repositoryWrapper.GoverningBodyAdministration.Update(admin);
                }
            }
            await _repositoryWrapper.SaveAsync();

            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRoleAsync(user, Roles.GoverningBodyAdmin);
        }

        public async Task RemoveGbAdminRoleAsync(string userId)
        {
            var admins = await _repositoryWrapper.GoverningBodyAdministration.GetAllAsync(a => a.UserId == userId && a.Status);

            foreach (var admin in admins)
            {
                await RemoveAdministratorAsync(admin.Id);
            }
        }

        public async Task<IEnumerable<ShortUserInformationDto>> GetUsersForGoverningBodyAdminFormAsync()
        {
            var users = await _repositoryWrapper.User.GetAllAsync();
            var usersDtos = new List<ShortUserInformationDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(Roles.PlastMember))
                {
                    var isInDeputyRole = roles.Contains(Roles.GoverningBodyAdmin);
                    var isInLowerRole = roles.Intersect(Roles.LowerRoles).Any();

                    var shortUser = _mapper.Map<User, ShortUserInformationDto>(user);
                    shortUser.IsInDeputyRole = isInDeputyRole;
                    shortUser.IsInLowerRole = isInLowerRole;

                    usersDtos.Add(shortUser);
                }
            }

            usersDtos = usersDtos.OrderBy(u => u.IsInDeputyRole || u.IsInLowerRole).ToList();

            return usersDtos;
        }

        public async Task<bool> CheckRoleNameExistsAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return false;

            var result = await _repositoryWrapper.GoverningBodyAdministration
                .GetFirstOrDefaultAsync(a => a.GoverningBodyAdminRole == roleName);

            return result != null;
        }

        private async Task<bool> CheckUserHasOneSecretaryTypeForGoverningBodyAsync(GoverningBodyAdministration admin)
        {
            int secretaryAdminTypesCount = 0;
            var userAdminTypes = await GetUserAdministrations(admin.UserId);
            foreach (GoverningBodyAdministrationDto userAdminType in userAdminTypes)
            {
                var secretaryCheck = userAdminType.AdminType.AdminTypeName switch
                {
                    Roles.GoverningBodyAdmin => Roles.GoverningBodyAdmin,
                    _ => Roles.GoverningBodySecretary,
                };
                if (secretaryCheck == Roles.GoverningBodySecretary) secretaryAdminTypesCount++;
            }
            if (secretaryAdminTypesCount > 1) return false;
            return true;
        }

        private async Task RemoveExistingGbAdminsAsync(int governingBodyId, string adminTypeName)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminTypeName);

            var admins = await _repositoryWrapper.GoverningBodyAdministration
                .GetAllAsync(a =>
                    a.AdminTypeId == adminType.ID
                    && a.Status
                    && a.GoverningBodyId == governingBodyId
                );

            foreach (var admin in admins)
            {
                await RemoveAdministratorAsync(admin.Id);
            }
        }

        private Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>> Include =>
            x => x.Include(a => a.User).Include(a => a.AdminType);
    }
}
