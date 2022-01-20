using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class UnArchiveCityCommand : IRequest
    {
        public int CityId { get; set; }

        public UnArchiveCityCommand(int cityId)
        {
            CityId = cityId;
        }
    }
}
