﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.RegionAdministrations;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.RegionAdministrations
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

        public async Task<RegionAdministrationDto> AddRegionAdministrator(RegionAdministrationDto regionAdministrationDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(regionAdministrationDTO.AdminType.AdminTypeName);
            var headType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.OkrugaHead);
            var headDeputyType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.OkrugaHeadDeputy);


            var newRegionAdmin = new DataAccess.Entities.RegionAdministration()
            {
                StartDate = regionAdministrationDTO.StartDate ?? DateTime.Now,
                EndDate = regionAdministrationDTO.EndDate,
                AdminTypeId = adminType.ID,
                RegionId = regionAdministrationDTO.RegionId,
                UserId = regionAdministrationDTO.UserId
            };

            if (CheckCityWasAdmin(newRegionAdmin))
            {
                newRegionAdmin.Status = false;
                await _repoWrapper.RegionAdministration.CreateAsync(newRegionAdmin);
                await _repoWrapper.SaveAsync();
                newRegionAdmin.ID = regionAdministrationDTO.ID;
                return regionAdministrationDTO;
            }

            var oldAdmin = await _repoWrapper.RegionAdministration.
                GetFirstOrDefaultAsync(d => d.AdminTypeId == newRegionAdmin.AdminTypeId
                && d.RegionId == newRegionAdmin.RegionId && d.Status);

            var newUser = await _userManager.FindByIdAsync(newRegionAdmin.UserId);
            string role = adminType.AdminTypeName switch
            {
                Roles.OkrugaHead => Roles.OkrugaHead,
                Roles.OkrugaHeadDeputy => Roles.OkrugaHeadDeputy,
                Roles.OkrugaReferentUPS => Roles.OkrugaReferentUPS,
                Roles.OkrugaReferentUSP => Roles.OkrugaReferentUSP,
                Roles.OkrugaReferentOfActiveMembership => Roles.OkrugaReferentOfActiveMembership,
                _ => Roles.OkrugaSecretary,
            };
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

        public async Task<RegionAdministrationDto> EditRegionAdministrator(RegionAdministrationDto regionAdministrationDTO)
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

        public async Task EditStatusAdministration(int adminId, bool status = false)
        {
            var admin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.ID == adminId);
            if (admin != null)
            {
                admin.Status = status;
                admin.EndDate = DateTime.Now;
                _repoWrapper.RegionAdministration.Update(admin);
                await _repoWrapper.SaveAsync();
            }
        }

        public async Task DeleteAdminByIdAsync(int Id)
        {
            var admin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.ID == Id);
            // don't use EditStatusAdministration because of CheckUserHasOneSecretaryTypeForRegion
            admin.EndDate = DateTime.Now;
            admin.Status = false;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            string role = adminType.AdminTypeName switch
            {
                Roles.OkrugaHead => Roles.OkrugaHead,
                Roles.OkrugaHeadDeputy => Roles.OkrugaHeadDeputy,
                Roles.OkrugaReferentUPS => Roles.OkrugaReferentUPS,
                Roles.OkrugaReferentUSP => Roles.OkrugaReferentUSP,
                Roles.OkrugaReferentOfActiveMembership => Roles.OkrugaReferentOfActiveMembership,
                _ => Roles.OkrugaSecretary,
            };

            if (role != Roles.OkrugaSecretary || (await CheckUserHasOneSecretaryTypeForRegionAsync(admin)))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            _repoWrapper.RegionAdministration.Update(admin);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<RegionAdministrationDto>> GetUserAdministrations(string userId)
        {

            var secretaries = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && a.Status,
                include: source => source
                 .Include(r => r.User)
                 .Include(r => r.Region)
                 .Include(r => r.AdminType));

            return _mapper.Map<IEnumerable<DataAccess.Entities.RegionAdministration>, IEnumerable<RegionAdministrationDto>>(secretaries);
        }

        public async Task<RegionAdministrationDto> GetRegionAdministrationByIdAsync(int regionAdministrationId)
        {
            var regionAdministration = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(predicate: a=>a.ID==regionAdministrationId,
                include: source =>source.Include(a=>a.User)
                    .Include(a=>a.Region)
                    .Include(a=>a.AdminType));
            return _mapper.Map<DataAccess.Entities.RegionAdministration, RegionAdministrationDto>(regionAdministration);
        }

        public async Task<IEnumerable<RegionAdministrationDto>> GetUserPreviousAdministrations(string userId)
        {
            var secretaries = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && !a.Status,
                include: source => source
                 .Include(r => r.User)
                 .Include(r => r.Region)
                 .Include(r => r.AdminType));
            return _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDto>>(secretaries).Reverse();
        }

        public async Task<RegionAdministrationDto> GetHead(int regionId)
        {
            var head = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(d => d.RegionId == regionId && d.AdminType.AdminTypeName == Roles.OkrugaHead 
                                                                                        && (d.EndDate > DateTime.Now || d.EndDate == null) && d.Status,
                include: source => source
                .Include(
                d => d.User));

            return _mapper.Map<RegionAdministration, RegionAdministrationDto>(head);
        }

        public async Task<RegionAdministrationDto> GetHeadDeputy(int regionId)
        {
            var headDeputy = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(d => d.RegionId == regionId && d.AdminType.AdminTypeName == Roles.OkrugaHeadDeputy 
                                                                                             && (d.EndDate > DateTime.Now || d.EndDate == null) && d.Status,
                include: source => source
                .Include(
                d => d.User));

            return _mapper.Map<RegionAdministration, RegionAdministrationDto>(headDeputy);
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

        public async Task<IEnumerable<RegionAdministrationDto>> GetAdministrationAsync(int regionId)
        {
            var admins = await _repoWrapper.RegionAdministration.GetAllAsync(d => d.RegionId == regionId && d.Status,
                include: source => source
                    .Include(t => t.User)
                        .Include(t => t.Region)
                        .Include(t => t.AdminType));

            return _mapper.Map<IEnumerable<DataAccess.Entities.RegionAdministration>, IEnumerable<RegionAdministrationDto>>(admins);
        }

        public async Task<IEnumerable<AdminTypeDto>> GetAllAdminTypes()
        {
            return _mapper.Map<IEnumerable<AdminType>, IEnumerable<AdminTypeDto>>(await _repoWrapper.AdminType.GetAllAsync());
        }

        public async Task RemoveAdminRolesByUserIdAsync(string userId)
        {
            var adminRoles = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && a.Status);
            foreach(var role in adminRoles)
            {
                await DeleteAdminByIdAsync(role.ID);
            }
        }

        private async Task<bool> CheckUserHasOneSecretaryTypeForRegionAsync(RegionAdministration admin)
        {
            int secretaryAdminTypesCount = 0;
            var userAdminTypes = await GetUserAdministrations(admin.UserId);
            foreach (RegionAdministrationDto userAdminType in userAdminTypes)
            {
                var secretaryCheck = userAdminType.AdminType.AdminTypeName switch
                {
                    Roles.OkrugaHead => Roles.OkrugaHead,
                    Roles.OkrugaHeadDeputy => Roles.OkrugaHeadDeputy,
                    Roles.OkrugaReferentUPS => Roles.OkrugaReferentUPS,
                    Roles.OkrugaReferentUSP => Roles.OkrugaReferentUSP,
                    Roles.OkrugaReferentOfActiveMembership => Roles.OkrugaReferentOfActiveMembership,
                    _ => Roles.OkrugaSecretary,
                };
                if (secretaryCheck == Roles.OkrugaSecretary) secretaryAdminTypesCount++;
            }
            if (secretaryAdminTypesCount > 1) return false;
            return true;
        }

        private bool CheckCityWasAdmin(RegionAdministration newAdmin)
        {
            return !(DateTime.Today < newAdmin.EndDate || newAdmin.EndDate == null);
        }
    }
}
