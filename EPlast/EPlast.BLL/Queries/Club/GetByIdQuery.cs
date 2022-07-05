using EPlast.BLL.DTO.Club;
using MediatR;

namespace EPlast.BLL.Queries.Club
{
    public class GetByIdQuery : IRequest<ClubDto>
    {
        public int ClubId { get; set; }

        public GetByIdQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
