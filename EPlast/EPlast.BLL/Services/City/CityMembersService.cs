using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.City
{
    public class CityMembersService : ICityMembersService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ICityService _cityService;
        private readonly ICityAdministrationService _cityAdministrationService;
        private readonly IMapper _mapper;
        private readonly IUserManagerService _userManagerService;

        public CityMembersService(IRepositoryWrapper repositoryWrapper,
            ICityService cityService,
            ICityAdministrationService cityAdministrationService,
            IMapper mapper,
            IUserManagerService userManagerService)
        {
            _repositoryWrapper = repositoryWrapper;
            _cityService = cityService;
            _cityAdministrationService = cityAdministrationService;
            _mapper = mapper;
            _userManagerService = userManagerService;
        }

        public async Task<IEnumerable<CityMembersDTO>> GetCurrentByCityIdAsync(int cityId)
        {
            var cityMembers = await _repositoryWrapper.CityMembers.GetAllAsync(
                    predicate: c => c.CityId == cityId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));
            return _mapper.Map<IEnumerable<CityMembers>, IEnumerable<CityMembersDTO>>(cityMembers);
        }

        public async Task<CityMembersDTO> AddCityFollower(int cityId, string userId)
        {
            var city = await _cityService.GetByIdAsync(cityId);
            var user = await _userManagerService.FindByIdAsync(userId); 

            var oldCityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(i => i.UserId == userId);

            if (oldCityMember == null)
            {
                _repositoryWrapper.CityMembers.Delete(oldCityMember);
                await _repositoryWrapper.SaveAsync();
            }

            var cityMember = new CityMembers()
            {
                CityId = cityId,
                IsApproved = false,
                UserId = userId
            };

            await _repositoryWrapper.CityMembers.CreateAsync(cityMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
        }
    }
}