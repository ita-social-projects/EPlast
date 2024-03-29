﻿using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityMembersQuery : IRequest<CityProfileDto>
    {
        public int CityId { get; set; }

        public GetCityMembersQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
