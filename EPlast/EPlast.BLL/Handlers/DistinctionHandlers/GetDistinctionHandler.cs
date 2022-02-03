using AutoMapper;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class GetDistinctionHandler: IRequestHandler<GetDistinctionQuery, DistinctionDTO>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetDistinctionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }
        public async Task<DistinctionDTO> Handle(GetDistinctionQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<DistinctionDTO>(await _repositoryWrapper.Distinction.GetFirstAsync(d => d.Id == request.Id));             
        }
    }
}
