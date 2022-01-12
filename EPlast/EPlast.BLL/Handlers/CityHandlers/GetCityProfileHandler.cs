using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityProfileHandler : IRequestHandler<GetCityProfileQuery, CityProfileDTO>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly ICityAccessService _cityAccessService;

        public GetCityProfileHandler(IMediator mediator, 
            IRepositoryWrapper repoWrapper,
            UserManager<User> userManager,
            ICityAccessService cityAccessService)
        {
            _mediator = mediator;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _cityAccessService = cityAccessService;
        }

        public async Task<CityProfileDTO> Handle(GetCityProfileQuery request, CancellationToken cancellationToken)
        {
            var profileQuery = new GetCityProfileBasicQuery(request.CityId);
            var cityProfileDto = await _mediator.Send(profileQuery, cancellationToken);
            var userId = await _userManager.GetUserIdAsync(request.User);
            var userRoles = await _userManager.GetRolesAsync(request.User);
            cityProfileDto.City.CanCreate = userRoles.Contains(Roles.Admin);
            cityProfileDto.City.CanEdit = await _cityAccessService.HasAccessAsync(request.User, request.CityId);
            cityProfileDto.City.CanJoin = (await _repoWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.User.Id == userId && u.CityId == request.CityId)) == null;

            return cityProfileDto;
        }
    }
}
