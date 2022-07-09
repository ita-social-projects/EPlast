using System.Collections.Generic;
using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionTargetSearchListAsyncQuery : IRequest<IEnumerable<DecisionTargetDto>>
    {
        public string SearchedData { get; set; }
        public GetDecisionTargetSearchListAsyncQuery(string searchedData)
        {
            SearchedData = searchedData;
        }     
    }
}
