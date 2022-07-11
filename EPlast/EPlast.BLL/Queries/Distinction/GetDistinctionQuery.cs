using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace EPlast.BLL.Queries.Distinction
{
    public class GetDistinctionQuery : IRequest<DistinctionDto>
    {
        public int Id { get; set; }
        public GetDistinctionQuery(int id)
        {
            Id = id;
        }
    }
}
