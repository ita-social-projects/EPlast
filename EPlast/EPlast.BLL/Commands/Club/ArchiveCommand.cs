using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Commands.Club
{
    public class ArchiveCommand : IRequest
    {
        public int ClubId { get; set; }
        public ArchiveCommand(int cLubId)
        {
            ClubId = cLubId;
        }
    }
}
