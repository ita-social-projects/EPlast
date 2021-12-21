using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers
{
    public class GetByIdHander : IRequestHandler<GetByIdQuery, ClubDTO>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetByIdHander(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<ClubDTO> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(
                     predicate: c => c.ID == request.ClubId,
                     include: source => source
                        .Include(c => c.ClubAdministration)
                            .ThenInclude(t => t.AdminType)
                        .Include(k => k.ClubAdministration)
                            .ThenInclude(a => a.User)
                        .Include(m => m.ClubMembers)
                            .ThenInclude(u => u.User)
                        .Include(l => l.ClubDocuments)
                            .ThenInclude(d => d.ClubDocumentType));

            return _mapper.Map<Club, ClubDTO>(club);
        }
    }
}
