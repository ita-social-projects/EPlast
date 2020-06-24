using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;

namespace EPlast.BusinessLogicLayer.Mapping.Approver
{
    public class ApproverProfile : Profile
    {
        public ApproverProfile()
        {
            CreateMap<DataAccess.Entities.Approver, ApproverDTO>().ReverseMap();
        }
    }
}
