using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Mapping
{
    public class ApproverProfile:Profile
    {
        public ApproverProfile()
        {
            CreateMap<Approver, ApproverDTO>();
            CreateMap<ApproverDTO, Approver>();
            CreateMap<ApproverViewModel, ApproverDTO>();
            CreateMap<ApproverDTO, ApproverViewModel>();
        }
    }
}
