using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class CreateCityCommand : IRequest<DataAccess.Entities.City>
    {
        public CityDTO City { get; set; }

        public CreateCityCommand(CityDTO city)
        {
            City = city;
        }
    }
}
