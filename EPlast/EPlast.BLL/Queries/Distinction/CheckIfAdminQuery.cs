using EPlast.DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Distinction
{
    public class CheckIfAdminQuery: IRequest
    {
        public User User { get; set; }

        public CheckIfAdminQuery(User user)
        {
            User = user;
        } 
    }
}
