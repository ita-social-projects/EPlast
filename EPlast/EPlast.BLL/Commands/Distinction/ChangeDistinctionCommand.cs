using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Commands.Distinction
{
    public class ChangeDistinctionCommand: IRequest
    {
        public DistinctionDto DistinctionDTO { get; set; }
        public User User { get; set; }

        public ChangeDistinctionCommand(DistinctionDto distinctionDTO, User user)
        {
            DistinctionDTO = distinctionDTO;
            User = user;
        }
    }
}
