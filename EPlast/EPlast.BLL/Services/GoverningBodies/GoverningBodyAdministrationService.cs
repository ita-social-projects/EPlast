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

namespace EPlast.BLL.Services.GoverningBodies
{
    public class GoverningBodyAdministrationService : IGoverningBodyAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IAdminTypeService _adminTypeService;

        public GoverningBodyAdministrationService(IRepositoryWrapper repositoryWrapper,
            UserManager<User> userManager,
            IAdminTypeService adminTypeService)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _adminTypeService = adminTypeService;
        }

        /// <inheritdoc />
        public async Task<GoverningBodyAdministrationDTO> AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDTO governingBodyAdministrationDto)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(governingBodyAdministrationDto.AdminType.AdminTypeName);
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

            var allowedRoles = new List<List<string>>
            {
                { new List<string>(){ Roles.PlastMember } },
                { new List<string>(){ Roles.PlastMember, Roles.KurinHead } },
                { new List<string>(){ Roles.PlastMember, Roles.KurinHeadDeputy } }
            };

            if (!allowedRoles.Any(x => x.OrderByDescending(x => x).SequenceEqual(userRoles.OrderByDescending(x => x))))
                throw new ArgumentException("Can't add user with the roles");

            var adminRole = adminType.AdminTypeName == Roles.GoverningBodyHead ? 
                Roles.GoverningBodyHead : 
                Roles.GoverningBodySecretary;
            await _userManager.AddToRoleAsync(user, adminRole);

            await CheckGoverningBodyHasAdmin(governingBodyAdministrationDto.GoverningBodyId, adminType.AdminTypeName);

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
            var role = adminType.AdminTypeName == Roles.GoverningBodyHead ? Roles.GoverningBodyHead : Roles.GoverningBodySecretary;
            await _userManager.RemoveFromRoleAsync(user, role);

            _repositoryWrapper.GoverningBodyAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
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
    }
}
