using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.Resources;

namespace EPlast.BLL.Services.Region
{
    public class RegionAdministrationService : IRegionAdministrationService
    {

        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IAdminTypeService _adminTypeService;
        private readonly UserManager<User> _userManager;

        public RegionAdministrationService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IAdminTypeService adminTypeService,
            UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _adminTypeService = adminTypeService;
            _userManager = userManager;

        }

        public async Task AddRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(regionAdministrationDTO.AdminTypeId);
            var newRegionAdmin = new RegionAdministration()
            {
                StartDate = regionAdministrationDTO.StartDate ?? DateTime.Now,
                EndDate = regionAdministrationDTO.EndDate,
                AdminTypeId = adminType.ID,
                RegionId = regionAdministrationDTO.RegionId,
                UserId = regionAdministrationDTO.UserId
            };

            var oldAdmin = await _repoWrapper.RegionAdministration.
                GetFirstOrDefaultAsync(d => d.AdminTypeId == newRegionAdmin.AdminTypeId
                && d.RegionId == newRegionAdmin.RegionId && d.Status);

            var newUser = await _userManager.FindByIdAsync(newRegionAdmin.UserId);

            var role = adminType.AdminTypeName == Roles.okrugaHead ? Roles.okrugaHead : Roles.okrugaSecretary;
            await _userManager.AddToRoleAsync(newUser, role);

            if (oldAdmin != null)
            {
                if (DateTime.Now < newRegionAdmin.EndDate || newRegionAdmin.EndDate == null)
                {
                    newRegionAdmin.Status = true;
                    oldAdmin.Status = false;
                    oldAdmin.EndDate = DateTime.Now;
                }
                else
                {
                    newRegionAdmin.Status = false;
                }
                var oldUser = await _userManager.FindByIdAsync(oldAdmin.UserId);
                await _userManager.RemoveFromRoleAsync(oldUser, role);
                _repoWrapper.RegionAdministration.Update(oldAdmin);
                await _repoWrapper.SaveAsync();
                await _repoWrapper.RegionAdministration.CreateAsync(newRegionAdmin);
                await _repoWrapper.SaveAsync();
            }
            else
            {
                newRegionAdmin.Status = true;
                await _repoWrapper.SaveAsync();
                await _repoWrapper.RegionAdministration.CreateAsync(newRegionAdmin);
                await _repoWrapper.SaveAsync();
            }
        }

        public async Task EditRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO)
        {
            var admin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.ID == regionAdministrationDTO.ID);
            if (admin != null)
            {
                var adminType = await _adminTypeService.GetAdminTypeByIdAsync(regionAdministrationDTO.AdminTypeId);

                if (adminType.ID == admin.AdminTypeId)
                {
                    admin.StartDate = regionAdministrationDTO.StartDate ?? DateTime.Now;
                    admin.EndDate = regionAdministrationDTO.EndDate;

                    _repoWrapper.RegionAdministration.Update(admin);
                    await _repoWrapper.SaveAsync();
                }

                else
                {
                    await DeleteAdminByIdAsync(regionAdministrationDTO.ID);
                    await AddRegionAdministrator(regionAdministrationDTO);
                }
            }
        }

        public async Task DeleteAdminByIdAsync(int Id)
        {
            var Admin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.ID == Id);
            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(Admin.AdminTypeId);

            var user = await _userManager.FindByIdAsync(Admin.UserId);
            var role = adminType.AdminTypeName == Roles.okrugaHead ? Roles.okrugaHead : Roles.okrugaSecretary;
            await _userManager.RemoveFromRoleAsync(user, role);

            _repoWrapper.RegionAdministration.Delete(Admin);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<RegionAdministrationDTO>> GetUsersAdministrations(string userId)
        {
            var secretaries = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && (a.EndDate > DateTime.Now || a.EndDate == null),
                include: source => source
                 .Include(r => r.User)
                 .Include(r => r.Region)
                 .Include(r => r.AdminType));
            return _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(secretaries);
        }


        public async Task<IEnumerable<RegionAdministrationDTO>> GetUsersPreviousAdministrations(string userId)
        {
            var secretaries = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && a.EndDate < DateTime.Now,
                include: source => source
                 .Include(r => r.User)
                 .Include(r => r.Region)
                 .Include(r => r.AdminType));
            return _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(secretaries);
        }

        public async Task<RegionAdministrationDTO> GetHead(int regionId)
        {
            var head = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(d => d.RegionId == regionId && d.AdminType.AdminTypeName == Roles.okrugaHead && (d.EndDate > DateTime.Now || d.EndDate == null),
                include: source => source
                .Include(
                d => d.User));

            return _mapper.Map<RegionAdministration, RegionAdministrationDTO>(head);
        }

        public async Task<int> GetAdminType(string name)
        {
            var type = await _repoWrapper.AdminType.GetFirstAsync(a => a.AdminTypeName == name);
            return type.ID;
        }

        public async Task<IEnumerable<RegionAdministrationDTO>> GetAdministrationAsync(int regionId)
        {
            var admins = await _repoWrapper.RegionAdministration.GetAllAsync(d => d.RegionId == regionId && d.Status,
                include: source => source
                    .Include(t => t.User)
                        .Include(t => t.Region)
                        .Include(t => t.AdminType));
            return _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(admins);
        }

        public async Task<IEnumerable<AdminTypeDTO>> GetAllAdminTypes()
        {
            return _mapper.Map<IEnumerable<AdminType>, IEnumerable<AdminTypeDTO>>(await _repoWrapper.AdminType.GetAllAsync());
        }
    }
}
