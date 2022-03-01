using EPlast.BLL.DTO;
using MediatR;
using System.Collections.Generic;

namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionTargetSearchListAsyncQuery: IRequest<IEnumerable<DecisionTargetDTO>>
    {
        public string SearchedData { get; set; }
        public GetDecisionTargetSearchListAsyncQuery(string searchedData)
        {
            SearchedData = searchedData;
        }     
    }
}
