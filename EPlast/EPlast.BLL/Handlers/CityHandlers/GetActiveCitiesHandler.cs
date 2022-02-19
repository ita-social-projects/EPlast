using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetActiveCitiesHandler : IRequestHandler<GetActiveCitiesQuery, IEnumerable<CityForAdministrationDTO>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetActiveCitiesHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityForAdministrationDTO>> Handle(GetActiveCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await _repoWrapper.City.GetAllAsync(c => c.IsActive);

            return _mapper.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityForAdministrationDTO>>(cities);
        }
    }
}
