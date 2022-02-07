using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.DataAccess.Entities.UserEntities;
using MediatR;
using System;
using System.Collections.Generic;

namespace EPlast.BLL.Queries.Precaution
{
    public class GetUsersPrecautionsForTableQuery : IRequest<Tuple<IEnumerable<UserPrecautionsTableObject>, int>>
    {
        public PrecautionTableSettings TableSettings { get; set; }

        public GetUsersPrecautionsForTableQuery(PrecautionTableSettings tableSettings)
        {
            TableSettings = tableSettings;
        }
    }
}
