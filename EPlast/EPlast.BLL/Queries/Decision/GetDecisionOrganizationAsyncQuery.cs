using EPlast.BLL.DTO;
using MediatR;
using System.Threading.Tasks;

namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionOrganizationAsyncQuery : IRequest<GoverningBodyDTO>
    {
        public GoverningBodyDTO governingBody { get; set; }
        public GetDecisionOrganizationAsyncQuery(GoverningBodyDTO _governingBody)
        {
            governingBody = _governingBody;
        }
    }
}
