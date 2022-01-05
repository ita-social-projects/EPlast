using EPlast.BLL.DTO.Club;
using MediatR;
using System.Collections.Generic;

namespace EPlast.BLL.Queries.Club
{
    public class GetClubHistoryMembersQuery : IRequest<IEnumerable<ClubMemberHistoryDTO>>
    {
        public int ClubId { get; set; }

        public GetClubHistoryMembersQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
