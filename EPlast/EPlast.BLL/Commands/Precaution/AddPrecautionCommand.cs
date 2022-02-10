using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Queries.Precaution
{
    public class AddPrecautionCommand: IRequest
    {
        public PrecautionDTO PrecautionDTO { get; set; }
        public User User { get; set; }
        public AddPrecautionCommand(PrecautionDTO precautionDTO, User user)
        {
            PrecautionDTO = precautionDTO;
            User = user;
        }
    }
}
