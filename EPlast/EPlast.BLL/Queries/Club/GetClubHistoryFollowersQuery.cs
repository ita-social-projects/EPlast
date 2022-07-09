using System.Collections.Generic;
using EPlast.BLL.DTO.Club;
using MediatR;

namespace EPlast.BLL.Queries.Club
{
    public class GetClubHistoryFollowersQuery : IRequest<IEnumerable<ClubMemberHistoryDto>>
    {
        public int ClubId { get; set; }

        public GetClubHistoryFollowersQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
