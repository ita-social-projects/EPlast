using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Distinction
{
    public class GetAllDistinctionQuery : IRequest<IEnumerable<DistinctionDTO>> 
    {
    }
}
