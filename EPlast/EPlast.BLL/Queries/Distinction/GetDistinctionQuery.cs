using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Distinction
{
    public class GetDistinctionQuery : IRequest<DistinctionDTO>
    {
        public int Id { get; set; }
        public GetDistinctionQuery(int id)
        {
            Id = id;
        }
    }
}
