using EPlast.BLL.DTO.City;
using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityProfileQuery : IRequest<CityProfileDTO>
    {
        public int CityId{ get; set; }
        public User User { get; set; }

        public GetCityProfileQuery(int cityId, User user)
        {
            CityId = cityId;
            User = user;
        }
    }
}
