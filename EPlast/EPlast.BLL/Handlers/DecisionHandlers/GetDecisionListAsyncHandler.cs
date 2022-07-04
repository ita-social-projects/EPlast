using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class GetDecisionListAsyncHandler : IRequestHandler<GetDecisionListAsyncQuery, IEnumerable<DecisionWrapperDto>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetDecisionListAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DecisionWrapperDto>> Handle(GetDecisionListAsyncQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Decesion> decisions = await _repositoryWrapper.Decesion.GetAllAsync(include: dec =>
              dec.Include(d => d.DecesionTarget).Include(d => d.Organization));

            return _mapper
                .Map<IEnumerable<DecisionDto>>(decisions)
                    .Select(decision => new DecisionWrapperDto { Decision = decision });
        }
    }
}
