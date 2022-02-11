using AutoMapper;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.PrecautionHandlers
{
    public class GetAllPrecautionHandler: IRequestHandler<GetAllPrecautionQuery, IEnumerable<PrecautionDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetAllPrecautionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrecautionDTO>> Handle(GetAllPrecautionQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Precaution>, IEnumerable<PrecautionDTO>>(await _repositoryWrapper.Precaution.GetAllAsync());
        }
    }
}
