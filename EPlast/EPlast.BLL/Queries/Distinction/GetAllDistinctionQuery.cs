using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace EPlast.BLL.Queries.Distinction
{
    public class GetAllDistinctionQuery : IRequest<IEnumerable<DistinctionDto>>
    {
    }
}
