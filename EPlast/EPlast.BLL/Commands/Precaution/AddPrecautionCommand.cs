using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Commands.Precaution
{
    public class AddPrecautionCommand: IRequest
    {
        public PrecautionDto PrecautionDTO { get; set; }
        public User User { get; set; }
        public AddPrecautionCommand(PrecautionDto precautionDTO, User user)
        {
            PrecautionDTO = precautionDTO;
            User = user;
        }
    }
}
