using AutoMapper;
using EPlast.BussinessLayer.DTO;

namespace EPlast.BussinessLayer.Mapping.Approver
{
    public class ApproverProfile : Profile
    {
        public ApproverProfile()
        {
            CreateMap<DataAccess.Entities.Approver, ApproverDTO>().ReverseMap();
        }
    }
}
