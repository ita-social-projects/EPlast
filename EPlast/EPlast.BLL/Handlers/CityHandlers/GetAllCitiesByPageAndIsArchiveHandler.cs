using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetAllCitiesByPageAndIsArchiveHandler : IRequestHandler<GetAllCitiesByPageAndIsArchiveQuery, Tuple<IEnumerable<CityObjectDTO>, int>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ICityBlobStorageRepository _cityBlobStorage;
        private readonly IMapper _mapper;

        public GetAllCitiesByPageAndIsArchiveHandler(IRepositoryWrapper repoWrapper, 
            ICityBlobStorageRepository cityBlobStorage,
            IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _cityBlobStorage = cityBlobStorage;
            _mapper = mapper;
        }

        public async Task<Tuple<IEnumerable<CityObjectDTO>, int>> Handle(GetAllCitiesByPageAndIsArchiveQuery request, CancellationToken cancellationToken)
        {
            var tuple = await _repoWrapper.City.GetCitiesObjects(request.Page, request.PageSize, request.Name, request.IsArchive, request.Oblast);
            var cities = tuple.Item1;
            //get images from blob storage
            foreach (var city in cities)
            {
                try
                {
                    //If string is null or empty then image is default, and it is not stored in Blob storage :)
                    if (!string.IsNullOrEmpty(city.Logo))
                    {
                        city.Logo = await _cityBlobStorage.GetBlobBase64Async(city.Logo);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cannot get image from blob storage because {ex}");
                }
            }
            var rows = tuple.Item2;
            return new Tuple<IEnumerable<CityObjectDTO>, int>(_mapper.Map<IEnumerable<DataAccess.Entities.CityObject>, IEnumerable<CityObjectDTO>>(cities), rows);
        }
    }
}
