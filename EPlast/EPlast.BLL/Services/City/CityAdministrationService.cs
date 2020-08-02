using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.City
{
    public class CityAdministrationService : ICityAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IAdminTypeService _adminTypeService;

        public CityAdministrationService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IAdminTypeService adminTypeService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _adminTypeService = adminTypeService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityAdministrationDTO>> GetAdministrationByIdAsync(int cityId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetAllAsync(
                predicate: x => x.CityId == cityId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(cityAdministration);
        }

        /// <inheritdoc />
        public async Task<CityAdministrationDTO> AddAdministratorAsync(CityAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            adminDTO.AdminTypeId = adminType.ID;
            adminDTO.AdminType = adminType;

            adminDTO.StartDate ??= DateTime.Now;
            
            var admin = _mapper.Map<CityAdministrationDTO, CityAdministration>(adminDTO);

            _repositoryWrapper.CityAdministration.Attach(admin);
            await _repositoryWrapper.CityAdministration.CreateAsync(admin);
            await _repositoryWrapper.SaveAsync();

            return adminDTO;
        }

        /// <inheritdoc />
        public async Task<CityAdministrationDTO> EditAdministratorAsync(CityAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            adminDTO.AdminTypeId = adminType.ID;
            adminDTO.AdminType = adminType;

            var admin = _mapper.Map<CityAdministrationDTO, CityAdministration>(adminDTO);
            
            _repositoryWrapper.CityAdministration.Attach(admin);
            _repositoryWrapper.CityAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();

            return adminDTO;
        }

        /// <inheritdoc />
        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(u => u.ID == adminId);
            
            _repositoryWrapper.CityAdministration.Delete(admin);
            await _repositoryWrapper.SaveAsync();
        }
    }
}