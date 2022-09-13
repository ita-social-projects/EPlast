using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class EditCityCommand : IRequest
    {
        public CityDto City { get; set; }

        public EditCityCommand(CityDto city)
        {
            City = city;
        }
    }
}
