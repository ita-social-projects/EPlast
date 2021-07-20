using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.GoverningBodies.Sector
{
    public class SectorAdministrationService : ISectorAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IAdminTypeService _adminTypeService;

        public SectorAdministrationService(IRepositoryWrapper repositoryWrapper,
                                            UserManager<User> userManager,
                                            IAdminTypeService adminTypeService)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _adminTypeService = adminTypeService;
        }

        public async Task<SectorAdministrationDTO> AddSectorAdministratorAsync(SectorAdministrationDTO sectorAdministrationDto)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(sectorAdministrationDto.AdminType.AdminTypeName);

            sectorAdministrationDto.Status = DateTime.Now < sectorAdministrationDto.EndDate || sectorAdministrationDto.EndDate == null;

            var sectorAdministration = new SectorAdministration
            {
                StartDate = sectorAdministrationDto.StartDate ?? DateTime.Now,
                EndDate = sectorAdministrationDto.EndDate,
                AdminTypeId = adminType.ID,
                SectorId = sectorAdministrationDto.SectorId,
                UserId = sectorAdministrationDto.UserId,
                Status = sectorAdministrationDto.Status
            };

            var user = await _userManager.FindByIdAsync(sectorAdministrationDto.UserId);
            var role = adminType.AdminTypeName == Roles.GoverningBodySectorHead ?
                Roles.GoverningBodySectorHead : Roles.GoverningBodySectorSecretary;
            await _userManager.AddToRoleAsync(user, role);

            await RemoveSectorAdminIfPresent(sectorAdministrationDto.SectorId, adminType.AdminTypeName);

            await _repositoryWrapper.GoverningBodySectorAdministration.CreateAsync(sectorAdministration);
            await _repositoryWrapper.SaveAsync();
            sectorAdministrationDto.Id = sectorAdministration.Id;

            return sectorAdministrationDto;
        }

        public async Task<SectorAdministrationDTO> EditSectorAdministratorAsync(SectorAdministrationDTO sectorAdministrationDto)
        {
            var admin = await _repositoryWrapper.GoverningBodySectorAdministration.GetFirstOrDefaultAsync(a => a.Id == sectorAdministrationDto.Id);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(sectorAdministrationDto.AdminType.AdminTypeName);

            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = sectorAdministrationDto.StartDate ?? DateTime.Now;
                admin.EndDate = sectorAdministrationDto.EndDate;

                _repositoryWrapper.GoverningBodySectorAdministration.Update(admin);
                await _repositoryWrapper.SaveAsync();
            }
            else
            {
                await RemoveAdministratorAsync(sectorAdministrationDto.Id);
                sectorAdministrationDto = await AddSectorAdministratorAsync(sectorAdministrationDto);
            }

            return sectorAdministrationDto;
        }

        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.GoverningBodySectorAdministration.GetFirstOrDefaultAsync(
                u => u.Id == adminId);
            admin.EndDate = DateTime.Now;
            admin.Status = false;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            var role = adminType.AdminTypeName == Roles.GoverningBodySectorHead ? 
                Roles.GoverningBodySectorHead : Roles.GoverningBodySectorSecretary;
            await _userManager.RemoveFromRoleAsync(user, role);

            _repositoryWrapper.GoverningBodySectorAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        private async Task RemoveSectorAdminIfPresent(int sectorId, string adminTypeName)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminTypeName);
            var admin = await _repositoryWrapper.GoverningBodySectorAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID
                                            && a.Status && a.SectorId == sectorId);

           if(admin != null)
            {
                await RemoveAdministratorAsync(admin.Id);
            }
        }
    }
}
