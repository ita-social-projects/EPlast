using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class CreateCityCommand : IRequest<DataAccess.Entities.City>
    {
        public CityDto City { get; set; }

        public CreateCityCommand(CityDto city)
        {
            City = city;
        }
    }
}
