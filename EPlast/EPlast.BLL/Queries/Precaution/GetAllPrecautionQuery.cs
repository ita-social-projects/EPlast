using MediatR;
using System.Collections.Generic;

namespace EPlast.BLL.Queries.Precaution
{
    public class GetAllPrecautionQuery: IRequest<IEnumerable<PrecautionDTO>>
    {
    }
}
