using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Terms;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.TermsOfUse
{
    public class GetFirstRecordTermsHandler : IRequestHandler<GetFirstRecordQuery, TermsDto>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetFirstRecordTermsHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<TermsDto> Handle(GetFirstRecordQuery request, CancellationToken cancellationToken)
        {
            var terms = _mapper.Map<TermsDto>(await _repoWrapper.TermsOfUse.GetFirstAsync());
            return terms;
        }
    }
}