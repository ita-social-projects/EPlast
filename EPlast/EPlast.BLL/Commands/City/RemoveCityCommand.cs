using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class RemoveCityCommand : IRequest
    {
        public int CityId { get; set; }

        public RemoveCityCommand(int cityId)
        {
            CityId = cityId;
        }
    }
}
