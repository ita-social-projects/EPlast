using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class EditCityCommand : IRequest
    {
        public CityDTO City { get; set; }

        public EditCityCommand(CityDTO city)
        {
            City = city;
        }
    }
}
