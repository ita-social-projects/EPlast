using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.GoverningBodies
{
    public class GoverningBodyAdministrationService : IGoverningBodyAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAdminTypeService _adminTypeService;

        public GoverningBodyAdministrationService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            UserManager<User> userManager,
            IAdminTypeService adminTypeService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _adminTypeService = adminTypeService;
        }

        public async Task<GoverningBodyAdministrationDTO> AddGoverningBodyAdministrator(GoverningBodyAdministrationDTO governingBodyAdministrationDto)
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
                Status = governingBodyAdministrationDto.Status
            };

            var user = await _userManager.FindByIdAsync(governingBodyAdministrationDto.UserId);
            var role = adminType.AdminTypeName == Roles.GoverningBodyHead ? Roles.GoverningBodyHead : Roles.GoverningBodySecretary;
            await _userManager.AddToRoleAsync(user, role);

            await CheckGoverningBodyHasAdmin(governingBodyAdministrationDto.GoverningBodyId, adminType.AdminTypeName);

            await _repositoryWrapper.GoverningBodyAdministration.CreateAsync(governingBodyAdministration);
            await _repositoryWrapper.SaveAsync();
            governingBodyAdministrationDto.ID = governingBodyAdministration.Id;

            return governingBodyAdministrationDto;
        }

        /// <inheritdoc />
        public async Task<GoverningBodyAdministrationDTO> EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDTO adminDto)
        {
            var admin = await _repositoryWrapper.GoverningBodyAdministration.GetFirstOrDefaultAsync(a => a.Id == adminDto.ID);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDto.AdminType.AdminTypeName);

            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = adminDto.StartDate ?? DateTime.Now;
                admin.EndDate = adminDto.EndDate;

                _repositoryWrapper.GoverningBodyAdministration.Update(admin);
                await _repositoryWrapper.SaveAsync();
            }
            else
            {
                await RemoveAdministratorAsync(adminDto.ID);
                adminDto = await AddGoverningBodyAdministrator(adminDto);
            }

            return adminDto;
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
