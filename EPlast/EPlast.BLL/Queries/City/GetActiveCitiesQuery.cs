using System.Collections.Generic;
using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetActiveCitiesQuery : IRequest<IEnumerable<CityForAdministrationDto>>
    {
    }
}
