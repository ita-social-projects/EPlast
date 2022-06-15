using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Handlers.CityHandlers.Helpers;
using EPlast.BLL.Queries.City;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityAdminsIdsHandler : IRequestHandler<GetCityAdminsIdsQuery, string>
    {
        private readonly IMediator _mediator;

        public GetCityAdminsIdsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> Handle(GetCityAdminsIdsQuery request, CancellationToken cancellationToken)
        {
            var query = new GetCityByIdQuery(request.CityId);
            var city = await _mediator.Send(query, cancellationToken);
            if (city == null)
            {
                return null;
            }

            var cityHead = CityHelpers.GetCityHead(city.CityAdministration);
            var cityHeadDeputy = CityHelpers.GetCityHeadDeputy(city.CityAdministration);

            var cityHeadId = cityHead != null ? cityHead.UserId : "No Id";
            var cityHeadDeputyId = cityHeadDeputy != null ? cityHeadDeputy.UserId : "No Id";

            return $"{cityHeadId},{cityHeadDeputyId}";
        }
    }
}
