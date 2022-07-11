using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.Approver;

namespace EPlast.WebApi.Mapping.Approver
{
    public class ApproverProfile : Profile
    {
        public ApproverProfile()
        {
            CreateMap<ApproverDto, ApproverViewModel>().ReverseMap();
        }
    }
}
