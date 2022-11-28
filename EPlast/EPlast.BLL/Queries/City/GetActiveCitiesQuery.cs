using System.Collections.Generic;
using EPlast.BLL.DTO.City;
using EPlast.Resources;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetActiveCitiesQuery : IRequest<IEnumerable<CityForAdministrationDto>>
    {
        public UkraineOblasts Oblast { get; set; }
        public bool IsOnlyActive { get; set; }

        public GetActiveCitiesQuery(bool isOnlyActive, UkraineOblasts oblast)
        {
            IsOnlyActive = isOnlyActive;
            Oblast = oblast;
        }
    }
}
