using EPlast.BLL.DTO;
using MediatR;
namespace EPlast.BLL.Queries.Decision
{
    public class GetDecisionAsyncQuery : IRequest<DecisionDto>
    {
        public int Id { get; set; }
        public GetDecisionAsyncQuery(int id)
        {
            Id = id;
        }
    }
}
