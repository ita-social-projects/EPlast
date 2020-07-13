using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<CityAdministrationDTO>> GetByCityIdAsync(int cityId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetAllAsync(
                predicate: x => x.CityId == cityId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));
            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(cityAdministration);
        }

        public async Task<CityAdministrationDTO> AddAdministratorAsync(CityAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);

            var admin = new CityAdministration()
            {
                AdminTypeId = adminType.ID,
                CityId = adminDTO.CityId,
                StartDate = adminDTO.StartDate,
                EndDate = adminDTO.EndDate,
                UserId = adminDTO.UserId
            };

            await _repositoryWrapper.CityAdministration.CreateAsync(admin);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<CityAdministration, CityAdministrationDTO>(admin);
        }

        public async Task RemoveAdministratorAsync(string userId)
        {
            var admin = await _repositoryWrapper.CityAdministration
                .GetFirstOrDefaultAsync(u => u.UserId == userId);

            _repositoryWrapper.CityAdministration.Delete(admin);
            await _repositoryWrapper.SaveAsync();
        }
    }
}