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
using EPlast.DataAccess.Entities;
using System.Linq;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class GetDecisionListAsyncHandler : IRequestHandler<GetDecisionListAsyncQuery, IEnumerable<DecisionWrapperDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetDecisionListAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DecisionWrapperDTO>> Handle(GetDecisionListAsyncQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Decesion> decisions = await _repositoryWrapper.Decesion.GetAllAsync(include: dec =>
              dec.Include(d => d.DecesionTarget).Include(d => d.Organization));

            return _mapper
                .Map<IEnumerable<DecisionDTO>>(decisions)
                    .Select(decision => new DecisionWrapperDTO { Decision = decision });
        }
    }
}
