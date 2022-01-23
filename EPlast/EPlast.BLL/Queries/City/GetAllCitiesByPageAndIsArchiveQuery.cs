﻿using System;
using System.Collections.Generic;
using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetAllCitiesByPageAndIsArchiveQuery : IRequest<Tuple<IEnumerable<CityObjectDTO>, int>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public bool IsArchive { get; set; }

        public GetAllCitiesByPageAndIsArchiveQuery(int page, int pageSize, string name, bool isArchive)
        {
            Page = page;
            PageSize = pageSize;
            Name = name;
            IsArchive = isArchive;
        }
    }
}
