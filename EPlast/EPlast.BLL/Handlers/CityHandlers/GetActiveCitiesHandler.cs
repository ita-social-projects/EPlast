using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetActiveCitiesHandler : IRequestHandler<GetActiveCitiesQuery, IEnumerable<CityForAdministrationDto>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetActiveCitiesHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityForAdministrationDto>> Handle(GetActiveCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await _repoWrapper.City.GetAllAsync(c => c.IsActive);

            var sortedCities = cities.OrderBy(c => c.Name);

            return _mapper.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityForAdministrationDto>>(sortedCities);
        }
    }
}
