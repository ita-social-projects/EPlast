﻿using AutoMapper;
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
using System.Linq;

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

        public async Task<RegionAdministrationDTO> AddRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(regionAdministrationDTO.AdminType.AdminTypeName);
            var headType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.OkrugaHead);
            var headDeputyType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.OkrugaHeadDeputy);
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

            string role;
            switch (adminType.AdminTypeName)
            {
                case Roles.OkrugaHead:
                    role = Roles.OkrugaHead;
                    break;
                case Roles.OkrugaHeadDeputy:
                    role = Roles.OkrugaHeadDeputy;
                    break;
                default:
                    role = Roles.OkrugaSecretary;
                    break;
            }
            await _userManager.AddToRoleAsync(newUser, role);

            if (adminType.AdminTypeName == headType.AdminTypeName)
            {
                var headDeputy = await _repoWrapper.RegionAdministration
                    .GetFirstOrDefaultAsync(a => a.AdminTypeId == headDeputyType.ID && a.RegionId == regionAdministrationDTO.RegionId && a.Status);
                if (headDeputy != null && headDeputy.UserId == regionAdministrationDTO.UserId)
                {
                    await DeleteAdminByIdAsync(headDeputy.ID);
                }
            }

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
                regionAdministrationDTO.ID = newRegionAdmin.ID;
                return regionAdministrationDTO;
            }
            else
            {
                newRegionAdmin.Status = DateTime.Today < newRegionAdmin.EndDate || newRegionAdmin.EndDate == null;
                await _repoWrapper.SaveAsync();
                await _repoWrapper.RegionAdministration.CreateAsync(newRegionAdmin);
                await _repoWrapper.SaveAsync();
                regionAdministrationDTO.ID = newRegionAdmin.ID;
                return regionAdministrationDTO;
            }
        }

        public async Task<RegionAdministrationDTO> EditRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO)
        {
            var admin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.ID == regionAdministrationDTO.ID);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(regionAdministrationDTO.AdminType.AdminTypeName);           
            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = regionAdministrationDTO.StartDate ?? DateTime.Now;
                admin.EndDate = regionAdministrationDTO.EndDate;
                admin.Status = true;

                _repoWrapper.RegionAdministration.Update(admin);
                await _repoWrapper.SaveAsync();
                return regionAdministrationDTO;
            }

            await DeleteAdminByIdAsync(regionAdministrationDTO.ID);
            await AddRegionAdministrator(regionAdministrationDTO);
            return regionAdministrationDTO;

        }

        public async Task DeleteAdminByIdAsync(int Id)
        {
            var Admin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.ID == Id);
            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(Admin.AdminTypeId);

            var user = await _userManager.FindByIdAsync(Admin.UserId);
            string role;
            switch (adminType.AdminTypeName)
            {
                case Roles.OkrugaHead:
                    role = Roles.OkrugaHead;
                    break;
                case Roles.OkrugaHeadDeputy:
                    role = Roles.OkrugaHeadDeputy;
                    break;
                default:
                    role = Roles.OkrugaSecretary;
                    break;
            }
            await _userManager.RemoveFromRoleAsync(user, role);

            _repoWrapper.RegionAdministration.Delete(Admin);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<RegionAdministrationDTO>> GetUsersAdministrations(string userId)
        {

            var secretaries = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && a.Status,
                include: source => source
                 .Include(r => r.User)
                 .Include(r => r.Region)
                 .Include(r => r.AdminType));

            return _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(secretaries);
        }


        public async Task<IEnumerable<RegionAdministrationDTO>> GetUsersPreviousAdministrations(string userId)
        {
            var secretaries = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && !a.Status,
                include: source => source
                 .Include(r => r.User)
                 .Include(r => r.Region)
                 .Include(r => r.AdminType));
            return _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(secretaries).Reverse();
        }

        public async Task<RegionAdministrationDTO> GetHead(int regionId)
        {
            var head = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(d => d.RegionId == regionId && d.AdminType.AdminTypeName == Roles.OkrugaHead && (d.EndDate > DateTime.Now || d.EndDate == null),
                include: source => source
                .Include(
                d => d.User));

            return _mapper.Map<RegionAdministration, RegionAdministrationDTO>(head);
        }

        public async Task<RegionAdministrationDTO> GetHeadDeputy(int regionId)
        {
            var headDeputy = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(d => d.RegionId == regionId && d.AdminType.AdminTypeName == Roles.OkrugaHeadDeputy && (d.EndDate > DateTime.Now || d.EndDate == null),
                include: source => source
                .Include(
                d => d.User));

            return _mapper.Map<RegionAdministration, RegionAdministrationDTO>(headDeputy);
        }
      
        public async Task<int> GetAdminType(string name)
        {
            var type = await _repoWrapper.AdminType.GetFirstOrDefaultAsync(a => a.AdminTypeName == name);
            if (type == null)
            {
                var newAdminType = new AdminType();
                newAdminType.AdminTypeName = name;
                _repoWrapper.AdminType.Create(newAdminType);
                await _repoWrapper.SaveAsync();
                type = await _repoWrapper.AdminType.GetFirstOrDefaultAsync(a => a.AdminTypeName == name);
            }
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

        public async Task RemoveAdminRolesByUserIdAsync(string userId)
        {
            var adminRoles = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && a.Status);
            foreach(var role in adminRoles)
            {
                await DeleteAdminByIdAsync(role.ID);
            }
        }
    }
}
