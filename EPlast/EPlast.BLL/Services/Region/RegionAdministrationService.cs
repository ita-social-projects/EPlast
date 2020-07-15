using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Region
{
    class RegionAdministrationService : IRegionAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IAdminTypeService _adminTypeService;
        
        public RegionAdministrationService(IRepositoryWrapper repositoryWrapper,
            IAdminTypeService adminTypeService,
            IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _adminTypeService = adminTypeService;
        }

        public async Task<IEnumerable<RegionAdministrationDTO>> GetAdministrationByIdAsync(int regionId)
        {
            var admins = await _repositoryWrapper.RegionAdministration.GetAllAsync(r => r.ID == regionId);

            return _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(admins);
        }

        public async Task<RegionAdministrationDTO> AddAdministratorAsync(RegionAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            adminDTO.AdminTypeId = adminType.ID;

            var admin = _mapper.Map<RegionAdministrationDTO, RegionAdministration>(adminDTO);
            
            await _repositoryWrapper.RegionAdministration.CreateAsync(admin);
            await _repositoryWrapper.SaveAsync();

            return adminDTO;
        }

        public async Task<RegionAdministrationDTO> EditAdministratorAsync(RegionAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            adminDTO.AdminTypeId = adminType.ID;

            var admin = _mapper.Map<RegionAdministrationDTO, RegionAdministration>(adminDTO);

            _repositoryWrapper.RegionAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();

            return adminDTO;
        }

        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(r => r.ID == adminId);
            admin.EndDate = DateTime.Now;

            _repositoryWrapper.RegionAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }
    }
}
