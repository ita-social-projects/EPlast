using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityByIdWthFullInfoHandler : IRequestHandler<GetCityByIdWthFullInfoQuery, CityDTO>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetCityByIdWthFullInfoHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<CityDTO> Handle(GetCityByIdWthFullInfoQuery request, CancellationToken cancellationToken)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(
                predicate: c => c.ID == request.CityId,
                include: source => source
                    .Include(l => l.CityDocuments)
                    .ThenInclude(d => d.CityDocumentType)
                    .Include(r => r.Region)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User));
            
            return _mapper.Map<City, CityDTO>(city);
        }
    }
}
