using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers.Helpers;
using EPlast.BLL.Queries.City;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityAdminsHandler : IRequestHandler<GetCityAdminsQuery, CityProfileDTO>
    {
        private readonly IMediator _mediator;

        public GetCityAdminsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CityProfileDTO> Handle(GetCityAdminsQuery request, CancellationToken cancellationToken)
        {
            var query = new GetCityByIdWthFullInfoQuery(request.CityId);
            var city = await _mediator.Send(query, cancellationToken);
            if (city == null)
            {
                return null;
            }

            var cityHead = CityHelpers.GetCityHead(city);
            var cityHeadDeputy = CityHelpers.GetCityHeadDeputy(city);
            var cityAdmins = CityHelpers.GetCityAdmins(city);

            var cityProfileDto = new CityProfileDTO
            {
                City = city,
                Admins = cityAdmins,
                Head = cityHead,
                HeadDeputy = cityHeadDeputy
            };

            return cityProfileDto;
        }
    }
}
