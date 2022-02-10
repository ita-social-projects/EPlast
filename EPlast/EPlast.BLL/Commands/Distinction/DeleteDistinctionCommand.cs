using EPlast.DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Distinction
{
    public class DeleteDistinctionCommand : IRequest
    {
        public int Id { get; set; }
        public User User { get; set; }

        public DeleteDistinctionCommand(int id, User user)
        {
            Id = id;
            User = user;
        }
    }
}
