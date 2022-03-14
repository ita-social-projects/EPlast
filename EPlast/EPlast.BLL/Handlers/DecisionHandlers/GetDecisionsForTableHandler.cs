using System;
using System.Collections.Generic;
using System.Text;
using EPlast.DataAccess.Repositories;
using MediatR;
using EPlast.BLL.Queries.Decision;
using EPlast.BLL.DTO;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EPlast.DataAccess.Entities.Decision;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class GetDecisionsForTableHandler : IRequestHandler<GetDecisionsForTableQuery, IEnumerable<DecisionTableObject>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        
        public GetDecisionsForTableHandler(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<DecisionTableObject>>Handle(GetDecisionsForTableQuery request, CancellationToken cancellationToken)
        {
            return _repositoryWrapper.Decesion.GetDecisions(request.SearchData, request.Page, request.PageSize);
        }
    }
}
