using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Commands.Distinction
{
    public class AddDistinctionCommand: IRequest
    {
        public DistinctionDTO DistinctionDTO { get; set; }
        public User User { get; set; }

        public AddDistinctionCommand(DistinctionDTO distinctionDTO, User user)
        {
            DistinctionDTO = distinctionDTO;
            User = user;
        }
    }
}
