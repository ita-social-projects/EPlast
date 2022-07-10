using AutoMapper;
using EPlast.BLL.DTO;

namespace EPlast.BLL.Mapping.Approver
{
    public class ApproverProfile : Profile
    {
        public ApproverProfile()
        {
            CreateMap<DataAccess.Entities.Approver, ApproverDto>().ReverseMap();
        }
    }
}
