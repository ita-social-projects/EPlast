using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class ApproverProfile:Profile
    {
        public ApproverProfile()
        {
            CreateMap<Approver, ApproverDTO>().ReverseMap();
            CreateMap<ApproverViewModel, ApproverDTO>().ReverseMap();
        }
    }
}
