using MediatR;

namespace EPlast.BLL.Queries.Club
{
    public class GetCountUsersPerYearQuery : IRequest<int>
    {
        public int ClubId { get; set; }

        public GetCountUsersPerYearQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
