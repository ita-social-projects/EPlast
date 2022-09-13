using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class CreateCityWthIdCommand : IRequest<int>
    {
        public CityDto City { get; set; }

        public CreateCityWthIdCommand(CityDto city)
        {
            City = city;
        }
    }
}
