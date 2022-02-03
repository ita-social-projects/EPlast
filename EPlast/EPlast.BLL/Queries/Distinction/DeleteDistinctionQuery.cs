using EPlast.DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Distinction
{
    public class DeleteDistinctionQuery : IRequest
    {
        public int Id { get; set; }
        public User User { get; set; }

        public DeleteDistinctionQuery(int id, User user)
        {
            Id = id;
            User = user;
        }
    }
}
