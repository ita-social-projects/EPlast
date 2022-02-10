using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.Distinction
{
    public class ChangeDistinctionCommand: IRequest
    {
        public DistinctionDTO DistinctionDTO { get; set; }
        public User User { get; set; }

        public ChangeDistinctionCommand(DistinctionDTO distinctionDTO, User user)
        {
            DistinctionDTO = distinctionDTO;
            User = user;
        }
    }
}
