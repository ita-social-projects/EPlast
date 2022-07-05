using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Commands.Distinction
{
    public class AddDistinctionCommand: IRequest
    {
        public DistinctionDto DistinctionDTO { get; set; }
        public User User { get; set; }

        public AddDistinctionCommand(DistinctionDto distinctionDTO, User user)
        {
            DistinctionDTO = distinctionDTO;
            User = user;
        }
    }
}
