using System.Collections.Generic;
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
    public class GetAdministrationHandler : IRequestHandler<GetAdministrationQuery, IEnumerable<CityAdministrationGetDto>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetAdministrationHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityAdministrationGetDto>> Handle(GetAdministrationQuery request, CancellationToken cancellationToken)
        {
            var admins = await _repoWrapper.CityAdministration.GetAllAsync(
                predicate: d => d.CityId == request.CityId && d.Status,
                include: source => source
                    .Include(t => t.User)
                    .Include(t => t.City)
                    .Include(t => t.AdminType));
            var admin = _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationGetDto>>(admins);
            return admin;
        }
    }
}
