using EPlast.BLL.DTO.Club;
using MediatR;
using System.Collections.Generic;

namespace EPlast.BLL.Queries.Club
{
    public class GetClubHistoryFollowersQuery : IRequest<IEnumerable<ClubMemberHistoryDTO>>
    {
        public int ClubId { get; set; }

        public GetClubHistoryFollowersQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
