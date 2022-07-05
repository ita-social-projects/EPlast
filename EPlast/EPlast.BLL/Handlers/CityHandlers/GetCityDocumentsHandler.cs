using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Queries.City;
using EPlast.BLL.Services;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityDocumentsHandler : IRequestHandler<GetCityDocumentsQuery, CityProfileDto>
    {
        private readonly IMediator _mediator;

        public GetCityDocumentsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CityProfileDto> Handle(GetCityDocumentsQuery request, CancellationToken cancellationToken)
        {
            var query = new GetCityByIdWthFullInfoQuery(request.CityId);
            var city = await _mediator.Send(query, cancellationToken);
            if (city == null)
            {
                return null;
            }

            var cityDoc = DocumentsSorter<CityDocumentsDto>.SortDocumentsBySubmitDate(city.CityDocuments);

            var cityProfileDto = new CityProfileDto
            {
                City = city,
                Documents = cityDoc.ToList()
            };

            return cityProfileDto;
        }
    }
}
