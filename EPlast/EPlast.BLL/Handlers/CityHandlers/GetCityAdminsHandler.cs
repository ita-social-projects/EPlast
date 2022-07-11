using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers.Helpers;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityAdminsHandler : IRequestHandler<GetCityAdminsQuery, CityAdministrationViewModelDto>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetCityAdminsHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<CityAdministrationViewModelDto> Handle(GetCityAdminsQuery request, CancellationToken cancellationToken)
        {
            var admins = await _repoWrapper.CityAdministration.GetAllAsync(
                selector: GetSelector(),
                predicate: x => x.CityId == request.CityId && x.Status);
            if (admins == null)
            {
                return null;
            }
            var adminsDTO = _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDto>>(admins);
            var cityHead = CityHelpers.GetCityHead(adminsDTO);
            var cityHeadDeputy = CityHelpers.GetCityHeadDeputy(adminsDTO);
            var cityAdmins = CityHelpers.GetCityAdmins(adminsDTO);
            var cityAdministrationViewModelDTO = new CityAdministrationViewModelDto
            {
                Administration = cityAdmins,
                Head = cityHead,
                HeadDeputy = cityHeadDeputy
            };

            return cityAdministrationViewModelDTO;
        }

        private Expression<Func<CityAdministration, CityAdministration>> GetSelector()
        {
            Expression<Func<CityAdministration, CityAdministration>> expr = x => new CityAdministration
            {
                    ID = x.ID,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status,
                    User = new User
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        ImagePath = x.User.ImagePath
                    },
                    AdminType = new AdminType
                    {
                        AdminTypeName = x.AdminType.AdminTypeName
                    },
                    CityId = x.CityId
            };
            return expr;
        }
    }
}
