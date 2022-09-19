using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.GoverningBodies.Sector
{
    public class SectorAdministrationService : ISectorAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IAdminTypeService _adminTypeService;
        private readonly IMapper _mapper;

        public SectorAdministrationService(IRepositoryWrapper repositoryWrapper,
                                            UserManager<User> userManager,
                                            IAdminTypeService adminTypeService,
                                            IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _adminTypeService = adminTypeService;
            _mapper = mapper;
        }

        public async Task<SectorAdministrationDto> AddSectorAdministratorAsync(SectorAdministrationDto sectorAdministrationDto)
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
                Status = sectorAdministrationDto.Status,
                WorkEmail = sectorAdministrationDto.WorkEmail
            };

            var user = await _userManager.FindByIdAsync(sectorAdministrationDto.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!sectorAdministration.Status)
            {
                await _repositoryWrapper.GoverningBodySectorAdministration.CreateAsync(sectorAdministration);
                await _repositoryWrapper.SaveAsync();
                sectorAdministrationDto.Id = sectorAdministration.Id;
                return sectorAdministrationDto;
            }

            if (!userRoles.Contains(Roles.PlastMember))
            {
                throw new ArgumentException("Can't add user with the roles");
            }

            var adminRole = adminType.AdminTypeName == Roles.GoverningBodySectorHead ?
                Roles.GoverningBodySectorHead :
                Roles.GoverningBodySectorSecretary;
            await _userManager.AddToRoleAsync(user, adminRole);

            await RemoveSectorAdminIfPresent(sectorAdministrationDto.SectorId, adminType.AdminTypeName);

            await _repositoryWrapper.GoverningBodySectorAdministration.CreateAsync(sectorAdministration);
            await _repositoryWrapper.SaveAsync();
            sectorAdministrationDto.Id = sectorAdministration.Id;

            return sectorAdministrationDto;
        }

        public async Task<SectorAdministrationDto> EditSectorAdministratorAsync(SectorAdministrationDto sectorAdministrationDto)
        {
            var admin = await _repositoryWrapper.GoverningBodySectorAdministration.GetFirstOrDefaultAsync(a => a.Id == sectorAdministrationDto.Id);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(sectorAdministrationDto.AdminType.AdminTypeName);

            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = sectorAdministrationDto.StartDate ?? DateTime.Now;
                admin.EndDate = sectorAdministrationDto.EndDate;
                admin.WorkEmail = sectorAdministrationDto.WorkEmail;

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
            var role = adminType.AdminTypeName switch
            {
                Roles.GoverningBodySectorHead => Roles.GoverningBodySectorHead,
                _ => Roles.GoverningBodySectorSecretary
            }; 
            
            if (role != Roles.GoverningBodySectorSecretary || (await CheckUserHasOneSecretaryTypeForSectorAsync(admin)))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            _repositoryWrapper.GoverningBodySectorAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task RemoveAdminRolesByUserIdAsync(string userId)
        {
            var roles = await _repositoryWrapper.GoverningBodySectorAdministration.GetAllAsync(a => a.UserId == userId && a.Status);
            foreach(var role in roles)
            {
                await RemoveAdministratorAsync(role.Id);
            }
        }

        public async Task<IEnumerable<SectorAdministrationDto>> GetUserAdministrations(string userId)
        {
            var admins = await _repositoryWrapper.GoverningBodySectorAdministration.GetAllAsync(a => a.UserId == userId && a.Status,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType)
                 );

            return _mapper.Map<IEnumerable<SectorAdministration>, IEnumerable<SectorAdministrationDto>>(admins);
        }

        private async Task<bool> CheckUserHasOneSecretaryTypeForSectorAsync(SectorAdministration admin)
        {
            int secretaryAdminTypesCount = 0;
            var userAdminTypes = await GetUserAdministrations(admin.UserId);
            foreach (SectorAdministrationDto userAdminType in userAdminTypes)
            {
                var secretaryCheck = userAdminType.AdminType.AdminTypeName switch
                {
                    Roles.GoverningBodySectorHead => Roles.GoverningBodySectorHead,
                    _ => Roles.GoverningBodySectorSecretary,
                };
                if (secretaryCheck == Roles.GoverningBodySectorSecretary) secretaryAdminTypesCount++;
            }
            if (secretaryAdminTypesCount > 1) return false;
            return true;
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
