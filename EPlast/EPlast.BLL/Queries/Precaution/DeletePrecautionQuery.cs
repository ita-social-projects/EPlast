using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.Precaution
{
    public class DeletePrecautionQuery: IRequest
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DeletePrecautionQuery(int id, User user)
        {
            Id = id;
            User = user;
        }
    }
}
