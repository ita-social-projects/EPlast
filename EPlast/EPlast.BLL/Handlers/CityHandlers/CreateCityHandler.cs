using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.City;
using EPlast.BLL.DTO.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class CreateCityHandler : IRequestHandler<CreateCityCommand, City>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public CreateCityHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<City> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var city = _mapper.Map<CityDto, City>(request.City);
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(r => r.RegionName == city.Region.RegionName);

            city.RegionId = region.ID;
            city.Region = region;

            return city;
        }
    }
}
