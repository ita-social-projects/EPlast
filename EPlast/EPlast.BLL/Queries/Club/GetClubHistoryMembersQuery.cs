using System.Collections.Generic;
using EPlast.BLL.DTO.Club;
using MediatR;

namespace EPlast.BLL.Queries.Club
{
    public class GetClubHistoryMembersQuery : IRequest<IEnumerable<ClubMemberHistoryDto>>
    {
        public int ClubId { get; set; }

        public GetClubHistoryMembersQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
