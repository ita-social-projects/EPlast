using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.TermsOfUse
{
    public class CheckIfAdminForTermsQuery : IRequest
    {
        public User User { get; set; }

        public CheckIfAdminForTermsQuery(User user)
        {
            User = user;
        }
    }
}