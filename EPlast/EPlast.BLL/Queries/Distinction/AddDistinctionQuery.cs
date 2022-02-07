using EPlast.DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Distinction
{
    public class AddDistinctionQuery: IRequest
    {
        public DistinctionDTO DistinctionDTO { get; set; }
        public User User { get; set; }

        public AddDistinctionQuery(DistinctionDTO distinctionDTO, User user)
        {
            DistinctionDTO = distinctionDTO;
            User = user;
        }
    }
}
