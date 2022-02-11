using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Commands.Precaution
{
    public class ChangePrecautionCommand: IRequest
    {
        public PrecautionDTO PrecautionDTO { get; set; }
        public User User { get; set; }
        public ChangePrecautionCommand(PrecautionDTO precautionDTO, User user)
        {
            PrecautionDTO = precautionDTO;
            User = user;
        }
    }
}
