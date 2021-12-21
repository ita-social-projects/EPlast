using EPlast.DataAccess.Entities;
using MediatR;
using System.Collections.Generic;

namespace EPlast.BLL.Queries.Club
{
    public class GetClubAdministrationsQuery : IRequest<IEnumerable<ClubAdministration>>
    {
        public int ClubId { get; set; }

        public GetClubAdministrationsQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
