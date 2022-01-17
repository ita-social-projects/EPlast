using MediatR;

namespace EPlast.BLL.Queries.Club
{
    public class GetCountDeletedUsersPerYearQuery : IRequest<int>
    {
        public int ClubId { get; set; }

        public GetCountDeletedUsersPerYearQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
