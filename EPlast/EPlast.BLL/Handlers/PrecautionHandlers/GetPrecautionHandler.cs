using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.PrecautionHandlers
{
    public class GetPrecautionHandler : IRequestHandler<GetPrecautionQuery, PrecautionDto>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetPrecautionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }
        public async Task<PrecautionDto> Handle(GetPrecautionQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PrecautionDto>(await _repositoryWrapper.Precaution.GetFirstAsync(d => d.Id == request.Id));
        }
    }
}
