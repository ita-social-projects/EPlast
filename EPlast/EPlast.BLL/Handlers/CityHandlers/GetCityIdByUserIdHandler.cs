using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityIdByUserIdHandler : IRequestHandler<GetCityIdByUserIdQuery, int>
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public GetCityIdByUserIdHandler(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<int> Handle(GetCityIdByUserIdQuery request, CancellationToken cancellationToken)
        {
            var city = await _repoWrapper.CityMembers.GetFirstAsync(x=>x.UserId == request.UserId );
            return city.CityId;
        }
    }
}
