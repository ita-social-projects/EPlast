using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Queries.City;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityFollowersHandler : IRequestHandler<GetCityFollowersQuery, CityProfileDTO>
    {
        private readonly IMediator _mediator;

        public GetCityFollowersHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CityProfileDTO> Handle(GetCityFollowersQuery request, CancellationToken cancellationToken)
        {
            var query = new GetCityByIdWthFullInfoQuery(request.CityId);
            var city = await _mediator.Send(query, cancellationToken);
            if (city == null)
            {
                return null;
            }

            var followers = city.CityMembers
                .Where(m => !m.IsApproved)
                .ToList();

            var cityProfileDto = new CityProfileDTO
            {
                City = city,
                Followers = followers
            };

            return cityProfileDto;
        }
    }
}
