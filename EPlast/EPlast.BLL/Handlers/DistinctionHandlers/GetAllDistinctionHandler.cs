using AutoMapper;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class GetAllDistinctionHandler : IRequestHandler<GetAllDistinctionQuery, IEnumerable<DistinctionDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetAllDistinctionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DistinctionDTO>> Handle(GetAllDistinctionQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Distinction>, IEnumerable<DistinctionDTO>>(await _repositoryWrapper.Distinction.GetAllAsync());
        }
    }
}
