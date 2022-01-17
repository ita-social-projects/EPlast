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
    public class GetAllCitiesOrByNameHandler : IRequestHandler<GetAllCitiesOrByNameQuery, IEnumerable<CityDTO>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetAllCitiesOrByNameHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityDTO>> Handle(GetAllCitiesOrByNameQuery request,
            CancellationToken cancellationToken)
        {
            var cities = await _repoWrapper.City.GetAllAsync();

            if (!string.IsNullOrEmpty(request.CityName))
                cities = cities.Where(c => c.Name.ToLower().Contains(request.CityName.ToLower()));

            return _mapper.Map<IEnumerable<City>, IEnumerable<CityDTO>>(cities);
        }
    }
}
