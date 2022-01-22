using AutoMapper;
using EPlast.BLL.DTO.Terms;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.TermsOfUse
{
    public class GetFirstRecordTermsHandler : IRequestHandler<GetFirstRecordQuery, TermsDTO>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetFirstRecordTermsHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<TermsDTO> Handle(GetFirstRecordQuery request, CancellationToken cancellationToken)
        {
            var terms = _mapper.Map<TermsDTO>(await _repoWrapper.TermsOfUse.GetFirstAsync());
            return terms;
        }
    }
}