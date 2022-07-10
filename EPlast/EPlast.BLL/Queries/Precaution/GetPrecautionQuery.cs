using MediatR;

namespace EPlast.BLL.Queries.Precaution
{
    public class GetPrecautionQuery : IRequest<PrecautionDto>
    {
        public int Id { get; set; }
        public GetPrecautionQuery(int id)
        {
            Id = id;
        }
    }
}
