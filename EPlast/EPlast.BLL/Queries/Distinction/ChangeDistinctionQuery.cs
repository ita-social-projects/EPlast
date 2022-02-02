using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.Distinction
{
    public class ChangeDistinctionQuery: IRequest
    {
        public DistinctionDTO DistinctionDTO { get; set; }
        public User User { get; set; }

        public ChangeDistinctionQuery(DistinctionDTO distinctionDTO, User user)
        {
            DistinctionDTO = distinctionDTO;
            User = user;
        }
    }
}
