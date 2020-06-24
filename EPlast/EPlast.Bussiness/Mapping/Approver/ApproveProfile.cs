using AutoMapper;
using EPlast.Bussiness.DTO;

namespace EPlast.Bussiness.Mapping.Approver
{
    public class ApproverProfile : Profile
    {
        public ApproverProfile()
        {
            CreateMap<DataAccess.Entities.Approver, ApproverDTO>().ReverseMap();
        }
    }
}
