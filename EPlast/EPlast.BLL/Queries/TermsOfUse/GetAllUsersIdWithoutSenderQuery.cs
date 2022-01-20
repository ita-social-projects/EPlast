using EPlast.DataAccess.Entities;
using MediatR;
using System.Collections.Generic;

namespace EPlast.BLL.Queries.TermsOfUse
{
    public class GetAllUsersIdWithoutSenderQuery : IRequest<IEnumerable<string>>
    {
        public User User { get; set; }

        public GetAllUsersIdWithoutSenderQuery(User user)
        {
            User = user;
        }
    }
}