using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Commands.Club
{
    public class UnArchiveCommand : IRequest
    {
        public int CLubId { get; set; }
        public UnArchiveCommand(int cLubId)
        {
            CLubId = cLubId;
        }
    }
}
