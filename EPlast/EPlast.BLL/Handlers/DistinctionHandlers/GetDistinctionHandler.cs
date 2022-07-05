using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class GetDistinctionHandler : IRequestHandler<GetDistinctionQuery, DistinctionDto>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetDistinctionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }
        public async Task<DistinctionDto> Handle(GetDistinctionQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<DistinctionDto>(await _repositoryWrapper.Distinction.GetFirstAsync(d => d.Id == request.Id));
        }
    }
}
