using EPlast.BLL.DTO;
using MediatR;
using System.Threading.Tasks;

namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionOrganizationAsyncQuery : IRequest<GoverningBodyDTO>
    {
        public GoverningBodyDTO GoverningBody { get; set; }
        public GetDecisionOrganizationAsyncQuery(GoverningBodyDTO governingBody)
        {
            GoverningBody = governingBody;
        }
    }
}
