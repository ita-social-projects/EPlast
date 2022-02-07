using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.Region;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Region
{
    public class GetAllRegionsByPageAndIsArchiveQuery : IRequest<Tuple<IEnumerable<RegionObjectsDTO>, int>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string RegionName { get; set; }
        public bool IsArchived { get; set; }
        public GetAllRegionsByPageAndIsArchiveQuery(int page, int pageSize, string regionName, bool isArchived)
        {
            Page = page;
            PageSize = pageSize;
            RegionName = regionName;
            IsArchived = isArchived;
        }
    }
}