using System.Collections.Generic;
using MediatR;

namespace EPlast.BLL.Queries.Precaution
{
    public class GetAllPrecautionQuery : IRequest<IEnumerable<PrecautionDto>>
    {
    }
}
