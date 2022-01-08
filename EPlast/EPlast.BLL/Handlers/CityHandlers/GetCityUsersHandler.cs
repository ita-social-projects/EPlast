using System.Collections.Generic;
using System.Linq;
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
    public class GetCityUsersHandler : IRequestHandler<GetCityUsersQuery, IEnumerable<CityUserDTO>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetCityUsersHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityUserDTO>> Handle(GetCityUsersQuery request, CancellationToken cancellationToken)
        {
            var cityMembers = await _repoWrapper.CityMembers.GetAllAsync(
                d => d.CityId == request.CityId && d.IsApproved,
                include: source => source
                    .Include(t => t.User));
            var users = cityMembers.Select(x => x.User);

            return _mapper.Map<IEnumerable<User>, IEnumerable<CityUserDTO>>(users);
        }
    }
}
