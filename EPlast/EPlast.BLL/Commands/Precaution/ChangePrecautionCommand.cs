using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Commands.Precaution
{
    public class ChangePrecautionCommand: IRequest
    {
        public PrecautionDto PrecautionDTO { get; set; }
        public User User { get; set; }
        public ChangePrecautionCommand(PrecautionDto precautionDTO, User user)
        {
            PrecautionDTO = precautionDTO;
            User = user;
        }
    }
}
