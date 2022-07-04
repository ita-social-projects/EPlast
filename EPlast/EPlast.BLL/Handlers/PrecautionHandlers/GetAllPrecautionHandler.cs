using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.PrecautionHandlers
{
    public class GetAllPrecautionHandler : IRequestHandler<GetAllPrecautionQuery, IEnumerable<PrecautionDto>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetAllPrecautionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrecautionDto>> Handle(GetAllPrecautionQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Precaution>, IEnumerable<PrecautionDto>>(await _repositoryWrapper.Precaution.GetAllAsync());
        }
    }
}
