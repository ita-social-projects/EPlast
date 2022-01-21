using MediatR;

namespace EPlast.BLL.Commands.City
{
    public class ArchiveCityCommand : IRequest
    {
        public int CityId { get; set; }

        public ArchiveCityCommand(int cityId)
        {
            CityId = cityId;
        }
    }
}
