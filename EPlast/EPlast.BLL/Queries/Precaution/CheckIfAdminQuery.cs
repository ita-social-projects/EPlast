using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.Precaution
{
    public class CheckIfAdminQuery: IRequest
    {
        public User User { get; set; }
        public CheckIfAdminQuery(User user)
        {
            User = user;
        }
    }
}
