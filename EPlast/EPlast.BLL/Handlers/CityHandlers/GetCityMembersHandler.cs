using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Queries.City;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityMembersHandler : IRequestHandler<GetCityMembersQuery, CityProfileDto>
    {
        private readonly IMediator _mediator;

        public GetCityMembersHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CityProfileDto> Handle(GetCityMembersQuery request, CancellationToken cancellationToken)
        {
            var query = new GetCityByIdWthFullInfoQuery(request.CityId);
            var city = await _mediator.Send(query, cancellationToken);
            if (city == null)
            {
                return null;
            }

            var members = city.CityMembers
                .Where(m => m.IsApproved)
                .ToList();

            var cityProfileDto = new CityProfileDto
            {
                City = city,
                Members = members
            };

            return cityProfileDto;
        }
    }
}
