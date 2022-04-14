using System;
using System.Linq;
using System.Linq.Expressions;
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
                predicate: x => x.ID == request.CityId,
                selector: GetSelector());

            return _mapper.Map<City, CityDTO>(city);
        }

        private Expression<Func<City, City>> GetSelector()
        {
            Expression<Func<City, City>> expr = x => new City
            {
                ID = x.ID,
                Logo = x.Logo,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                Street = x.Street,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                CityURL = x.CityURL,
                HouseNumber = x.HouseNumber,
                OfficeNumber = x.OfficeNumber,
                PostIndex = x.PostIndex,

                Region =  new Region
                {
                    RegionName = x.Region.RegionName,
                    ID = x.Region.ID,
                },
                CityAdministration = x.CityAdministration.Where(x => x.Status).Select(x => new CityAdministration
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
                }).ToList(),
                CityMembers = x.CityMembers.Select(x => new CityMembers
                {
                    UserId = x.UserId,
                    User = new User
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        ImagePath = x.User.ImagePath
                    },
                    CityId = x.CityId,
                    IsApproved = x.IsApproved
                }).ToList(),
                CityDocuments = x.CityDocuments.Select(x => new CityDocuments
                {
                    BlobName = x.BlobName,
                    FileName = x.FileName,
                    SubmitDate = x.SubmitDate,
                    CityDocumentType = new CityDocumentType
                    {
                        Name = x.CityDocumentType.Name
                    }

                }).ToList()
            };
            return expr;
        }
    }
}
