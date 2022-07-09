using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class UploadCityPhotoCommand : IRequest
    {
        public CityDto City { get; set; }

        public UploadCityPhotoCommand(CityDto city)
        {
            City = city;
        }
    }
}
