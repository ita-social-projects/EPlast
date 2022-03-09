using EPlast.BLL.DTO.Distinction;
using EPlast.DataAccess.Entities.Decision;
using MediatR;
using System;
using System.Collections.Generic;
namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionsForTableQuery : IRequest<IEnumerable<DecisionTableObject>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchData { get; set; }
        public GetDecisionsForTableQuery(string searchData, int page, int pageSize)
        {
            Page = page;
            SearchData = searchData;
            PageSize = pageSize;
        }   
    }
}
