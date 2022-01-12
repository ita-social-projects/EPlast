using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCitiesByRegionHandler : IRequestHandler<GetCitiesByRegionQuery, IEnumerable<CityDTO>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetCitiesByRegionHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityDTO>> Handle(GetCitiesByRegionQuery request, CancellationToken cancellationToken)
        {
            var cities = await _repoWrapper.City.GetAllAsync(c => c.RegionId == request.RegionId && c.IsActive);
            foreach (var city in cities)
            {
                var cityMembers = await _repoWrapper.CityMembers.GetAllAsync(
                    predicate: c => c.CityId == city.ID);
                city.CityMembers = cityMembers.ToList();
            }

            return _mapper.Map<IEnumerable<City>, IEnumerable<CityDTO>>(cities);
        }
    }
}
