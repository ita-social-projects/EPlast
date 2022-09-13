using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class GetDecisionOrganizationAsyncHandler : IRequestHandler<GetDecisionOrganizationAsyncQuery, GoverningBodyDto>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetDecisionOrganizationAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<GoverningBodyDto> Handle(GetDecisionOrganizationAsyncQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<GoverningBodyDto>(string.IsNullOrEmpty(request.GoverningBody.GoverningBodyName)
                   ? await _repoWrapper.GoverningBody.GetFirstAsync(x => x.ID == request.GoverningBody.Id)
                   : await _repoWrapper.GoverningBody.GetFirstAsync(x => x.OrganizationName.Equals(request.GoverningBody.GoverningBodyName)));

        }
    }
}
