using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class PlastMemberCheckQuery : IRequest<bool>
    {
        public string UserId { get; set; }

        public PlastMemberCheckQuery(string userId)
        {
            UserId = userId;
        }
    }
}
