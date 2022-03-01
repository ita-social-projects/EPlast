using MediatR;
using EPlast.BLL.DTO;
using AutoMapper;
using EPlast.DataAccess.Repositories;
using System.Threading.Tasks;
using System.Threading;
using EPlast.BLL.Queries.Decision;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class GetDecisionOrganizationAsyncHandler : IRequestHandler<GetDecisionOrganizationAsyncQuery, GoverningBodyDTO>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetDecisionOrganizationAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<GoverningBodyDTO> Handle(GetDecisionOrganizationAsyncQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<GoverningBodyDTO>(string.IsNullOrEmpty(request.GoverningBody.GoverningBodyName)
                   ? await _repoWrapper.GoverningBody.GetFirstAsync(x => x.ID == request.GoverningBody.Id)
                   : await _repoWrapper.GoverningBody.GetFirstAsync(x => x.OrganizationName.Equals(request.GoverningBody.GoverningBodyName)));

        }
    }
}
