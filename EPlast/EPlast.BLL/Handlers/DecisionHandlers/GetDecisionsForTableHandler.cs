using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Entities.Decision;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            return await _repositoryWrapper.Decesion.GetDecisions(request.SearchData, request.Page, request.PageSize);
        }
    }
}
