using System;
using System.Collections.Generic;
using System.Text;
using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionListAsyncQuery : IRequest<IEnumerable<DecisionWrapperDTO>>
    {
    }
}
