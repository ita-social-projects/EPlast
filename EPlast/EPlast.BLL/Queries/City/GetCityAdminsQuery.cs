﻿using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityAdminsQuery : IRequest<CityAdministrationViewModelDto>
    {
        public int CityId { get; set; }

        public GetCityAdminsQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
