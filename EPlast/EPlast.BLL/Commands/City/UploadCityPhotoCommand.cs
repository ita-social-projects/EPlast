using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class UploadCityPhotoCommand : IRequest
    {
        public CityDTO City { get; set; }

        public UploadCityPhotoCommand(CityDTO city)
        {
            City = city;
        }
    }
}
