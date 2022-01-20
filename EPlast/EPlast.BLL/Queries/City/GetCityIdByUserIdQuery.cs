using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityIdByUserIdQuery : IRequest<int>
    {
        public string UserId { get; set; }

        public GetCityIdByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
