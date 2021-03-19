using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping
{
    public class MethodicDocumentProfile: Profile
    {
        public MethodicDocumentProfile() {
            CreateMap<MethodicDocument, MethodicDocumentDTO>()
                    .ForMember(d => d.GoverningBody, o => o.MapFrom(s => s.Organization))
                    .ReverseMap();
        }

    }
}
