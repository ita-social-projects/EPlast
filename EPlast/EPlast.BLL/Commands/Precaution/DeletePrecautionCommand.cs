using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.Precaution
{
    public class DeletePrecautionCommand: IRequest
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DeletePrecautionCommand(int id, User user)
        {
            Id = id;
            User = user;
        }
    }
}
