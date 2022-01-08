using System.Collections.Generic;
using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetAdministrationQuery : IRequest<IEnumerable<CityAdministrationGetDTO>>
    {
        public int CityId { get; set; }

        public GetAdministrationQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
