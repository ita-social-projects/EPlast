using EPlast.BLL.DTO;
using MediatR;
using System.Collections.Generic;

namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionTargetSearchListAsyncQuery: IRequest<IEnumerable<DecisionTargetDTO>>
    {
        public string searchedData { get; set; }
        public GetDecisionTargetSearchListAsyncQuery(string _searchedData)
        {
            searchedData = _searchedData;
        }     
    }
}
