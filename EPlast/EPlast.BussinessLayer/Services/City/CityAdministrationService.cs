using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.City
{
    public class CityAdministrationService : ICItyAdministrationService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        public CityAdministrationService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityAdministrationDTO>> GetByCityIdAsync(int cityId)
        {
            var cityAdministration = await _repoWrapper.CityAdministration
                    .FindByCondition(ca => ca.CityId == cityId)
                        .Include(ca => ca.User)
                        .Include(ca => ca.AdminType).
                    ToListAsync();
            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(cityAdministration);
        }
    }
}
