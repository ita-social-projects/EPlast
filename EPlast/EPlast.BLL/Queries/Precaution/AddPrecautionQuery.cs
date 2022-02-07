using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.Precaution
{
    public class AddPrecautionQuery: IRequest
    {
        public PrecautionDTO PrecautionDTO { get; set; }
        public User User { get; set; }
        public AddPrecautionQuery(PrecautionDTO precautionDTO, User user)
        {
            PrecautionDTO = precautionDTO;
            User = user;
        }
    }
}
