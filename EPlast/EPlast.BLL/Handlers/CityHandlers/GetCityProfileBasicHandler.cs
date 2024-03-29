﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers.Helpers;
using EPlast.BLL.Queries.City;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityProfileBasicHandler : IRequestHandler<GetCityProfileBasicQuery, CityProfileDto>
    {
        private readonly IMediator _mediator;
        
        public GetCityProfileBasicHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CityProfileDto> Handle(GetCityProfileBasicQuery request, CancellationToken cancellationToken)
        {
            var query = new GetCityByIdWthFullInfoQuery(request.CityId);
            var city = await _mediator.Send(query, cancellationToken);
            if (city == null)
            {
                return null;
            }

            const int membersToShow = 9;
            const int followersToShow = 6;
            const int adminsToShow = 6;
            const int documentToShow = 6;
            var cityHead = CityHelpers.GetCityHead(city.CityAdministration);
            var cityHeadDeputy = CityHelpers.GetCityHeadDeputy(city.CityAdministration);
            var cityAdmins = city.CityAdministration
                .Take(adminsToShow)
                .ToList();
            city.AdministrationCount = city.CityAdministration == null ? 0
                : city.CityAdministration.Count();
            var members = city.CityMembers
                .Where(m => m.IsApproved)
                .Take(membersToShow)
                .ToList();
            city.MemberCount = city.CityMembers
                .Count(m => m.IsApproved);
            var followers = city.CityMembers
                .Where(m => !m.IsApproved)
                .Take(followersToShow)
                .ToList();
            city.FollowerCount = city.CityMembers
                .Count(m => !m.IsApproved);
            var cityDoc = city.CityDocuments.Take(documentToShow).ToList();
            city.DocumentsCount = city.CityDocuments.Count();

            var cityProfileDto = new CityProfileDto
            {
                City = city,
                Head = cityHead,
                HeadDeputy = cityHeadDeputy,
                Members = members,
                Followers = followers,
                Admins = cityAdmins,
                Documents = cityDoc,
            };

            return cityProfileDto;
        }
    }
}
