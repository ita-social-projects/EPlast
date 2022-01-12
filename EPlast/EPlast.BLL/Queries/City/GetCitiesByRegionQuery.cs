using System.Collections.Generic;
using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCitiesByRegionQuery : IRequest<IEnumerable<CityDTO>>
    {
        public int RegionId { get; set; }

        public GetCitiesByRegionQuery(int regionId)
        {
            RegionId = regionId;
        }
    }
}
