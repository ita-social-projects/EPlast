using EPlast.DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Distinction
{
    public class AddDistinctionCommand: IRequest
    {
        public DistinctionDTO DistinctionDTO { get; set; }
        public User User { get; set; }

        public AddDistinctionCommand(DistinctionDTO distinctionDTO, User user)
        {
            DistinctionDTO = distinctionDTO;
            User = user;
        }
    }
}
