using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class CreateCityWthIdCommand : IRequest<int>
    {
        public CityDTO City { get; set; }

        public CreateCityWthIdCommand(CityDTO city)
        {
            City = city;
        }
    }
}
