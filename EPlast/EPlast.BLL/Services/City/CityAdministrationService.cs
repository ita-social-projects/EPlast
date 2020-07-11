using AutoMapper;
using EPlast.BLL.DTO.City;
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
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public CityAdministrationService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityAdministrationDTO>> GetByCityIdAsync(int cityId)
        {
            var cityAdministration = await _repoWrapper.CityAdministration.GetAllAsync(
                predicate: x => x.CityId == cityId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));
            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(cityAdministration);
        }
    }
}