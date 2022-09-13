using System.Threading.Tasks;
using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionOrganizationAsyncQuery : IRequest<GoverningBodyDto>
    {
        public GoverningBodyDto GoverningBody { get; set; }
        public GetDecisionOrganizationAsyncQuery(GoverningBodyDto governingBody)
        {
            GoverningBody = governingBody;
        }
    }
}
