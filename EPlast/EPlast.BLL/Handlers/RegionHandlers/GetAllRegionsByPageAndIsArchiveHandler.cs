using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Queries.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.RegionHandlers
{
    public class GetAllRegionsByPageAndIsArchiveHandler : IRequestHandler<GetAllRegionsByPageAndIsArchiveQuery, Tuple<IEnumerable<RegionObjectsDTO>, int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IRegionBlobStorageRepository _regionBlobStorage;

        public GetAllRegionsByPageAndIsArchiveHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IRegionBlobStorageRepository regionBlobStorageRepository)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _regionBlobStorage = regionBlobStorageRepository;
        }

        public async Task<Tuple<IEnumerable<RegionObjectsDTO>, int>> Handle(GetAllRegionsByPageAndIsArchiveQuery request, CancellationToken cancellationToken)
        {
            var filter = GetFilter(request.RegionName, request.IsArchived);
            var order = GetOrder();
            var selector = GetSelector();
            var tuple = await _repositoryWrapper.Region.GetRangeAsync(filter, selector, order, null, request.Page, request.PageSize);
            var regions = tuple.Item1;
            var rows = tuple.Item2;
            var regionPhotos = new Tuple<IEnumerable<RegionObject>, int>(_mapper.Map<IEnumerable<Region>, IEnumerable<RegionObject>>(regions), rows);

            foreach (var regionPhoto in regionPhotos.Item1)
            {
                try
                {
                    //If string is null or empty then image is default, and it is not stored in Blob storage :)
                    if (!string.IsNullOrEmpty(regionPhoto.Logo))
                    {
                        regionPhoto.Logo = await _regionBlobStorage.GetBlobBase64Async(regionPhoto.Logo);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cannot get image from blob storage because {ex}");
                }
            }

            return new Tuple<IEnumerable<RegionObjectsDTO>, int>(_mapper.Map<IEnumerable<RegionObject>, IEnumerable<RegionObjectsDTO>>(regionPhotos.Item1), regionPhotos.Item2);
        }

        private Expression<Func<Region, bool>> GetFilter(string regionName, bool isArchive)
        {
            var regionNameEmpty = string.IsNullOrEmpty(regionName);
            var governingBodyName = EnumExtensions.GetDescription(RegionsStatusType.RegionBoard);
            Expression<Func<Region, bool>> expr = (regionNameEmpty) switch
            {
                true => x => x.IsActive == isArchive && x.RegionName != governingBodyName,
                false => x => x.RegionName.Contains(regionName) && x.IsActive == isArchive 
            };
            return expr;
        }

        private Func<IQueryable<Region>, IQueryable<Region>> GetOrder()
        {
            Func<IQueryable<Region>, IQueryable<Region>> expr = x => x.OrderBy(e => e.ID);
            return expr;
        }

        private Expression<Func<Region, Region>> GetSelector()
        {
            Expression<Func<Region, Region>> expr = x => new Region { ID = x.ID, Logo = x.Logo, RegionName = x.RegionName };
            return expr;
        }
    }
}