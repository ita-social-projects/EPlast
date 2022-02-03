using EPlast.BLL.DTO.Distinction;
using EPlast.DataAccess.Entities.UserEntities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Distinction
{
    public class GetUsersDistinctionsForTableQuery: IRequest<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>
    {
        public DistictionTableSettings TableSettings { get; set; }

        public GetUsersDistinctionsForTableQuery(DistictionTableSettings tableSettings)
        {
            TableSettings = tableSettings;
        } 
    }
}
