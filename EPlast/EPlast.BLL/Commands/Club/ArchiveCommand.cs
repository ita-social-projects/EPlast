using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Commands.Club
{
    public class ArchiveCommand : IRequest
    {
        public int CLubId { get; set; }
        public ArchiveCommand(int cLubId)
        {
            CLubId = cLubId;
        }
    }
}
