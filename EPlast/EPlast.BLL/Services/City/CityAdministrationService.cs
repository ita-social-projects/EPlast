using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.City
{
    public class CityAdministrationService : ICityAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IAdminTypeService _adminTypeService;
        private readonly UserManager<User> _userManager;


        public CityAdministrationService(IRepositoryWrapper repositoryWrapper,
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
        public async Task<IEnumerable<CityAdministrationDTO>> GetAdministrationByIdAsync(int cityId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetAllAsync(
                predicate: x => x.CityId == cityId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));

            return  _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(cityAdministration);
        }

        /// <inheritdoc />
        public async Task<CityAdministrationDTO> AddAdministratorAsync(CityAdministrationDTO adminDTO)
        {
            
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            var admin = new CityAdministration()
            {
                StartDate = adminDTO.StartDate ?? DateTime.Now,
                EndDate = adminDTO.EndDate,
                AdminTypeId = adminType.ID,
                CityId = adminDTO.CityId,
                UserId = adminDTO.UserId
            };

            var user = await _userManager.FindByIdAsync(adminDTO.UserId);
            var role = adminType.AdminTypeName == "Голова Станиці" ? "Голова Станиці" : "Діловод Станиці";
            await _userManager.AddToRoleAsync(user, role);

            if(role == "Голова Станиці")
            {
                await CheckCityHasHead(adminDTO.CityId);
            }

            await _repositoryWrapper.CityAdministration.CreateAsync(admin);
            await _repositoryWrapper.SaveAsync();
            adminDTO.ID = admin.ID;

            return adminDTO;
        }
        

        /// <inheritdoc />
        public async Task<CityAdministrationDTO> EditAdministratorAsync(CityAdministrationDTO adminDTO)
        {
            var admin = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(a => a.ID == adminDTO.ID);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);

            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = adminDTO.StartDate ?? DateTime.Now;
                admin.EndDate = adminDTO.EndDate;

                _repositoryWrapper.CityAdministration.Update(admin);
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
            var admin = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(u => u.ID == adminId);
            admin.EndDate = DateTime.Now;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            var role = adminType.AdminTypeName == "Голова Станиці" ? "Голова Станиці" : "Діловод Станиці";
            await _userManager.RemoveFromRoleAsync(user, role);

            _repositoryWrapper.CityAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task CheckPreviousAdministratorsToDelete()
        {
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.EndDate <= DateTime.Now);
            var cityHeadType = await _adminTypeService.GetAdminTypeByNameAsync("Голова Станиці");

            foreach (var admin in admins)
            {
                var role = admin.AdminTypeId == cityHeadType.ID ? "Голова Станиці" : "Діловод Станиці";

                var currentAdministration = await _repositoryWrapper.CityAdministration
                    .GetAllAsync(a => (a.EndDate > DateTime.Now || a.EndDate == null) && a.UserId == admin.UserId);

                if (currentAdministration.All(a => (a.AdminTypeId == cityHeadType.ID ? "Голова Станиці" : "Діловод Станиці") != role)
                    || currentAdministration.Count() == 0)
                {
                    var user = await _userManager.FindByIdAsync(admin.UserId);

                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }
        }

        public async Task<IEnumerable<CityAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(a => a.City)
                 );

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(admins);
        }

        private async Task CheckCityHasHead(int cityId)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync("Голова Станиці");
            var admin = await _repositoryWrapper.CityAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID 
                    && (DateTime.Now < a.EndDate || a.EndDate == null));

            if (admin != null)
            {
                await RemoveAdministratorAsync(admin.ID);
            }
        }
    }
}