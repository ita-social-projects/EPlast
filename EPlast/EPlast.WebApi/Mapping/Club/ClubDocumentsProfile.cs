using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubDocumentsProfile:Profile
    {
        public ClubDocumentsProfile()
        {
            CreateMap<ClubDocumentsViewModel, ClubDocumentsDTO>().ReverseMap();
            CreateMap<ClubDocumentTypeViewModel, ClubDocumentTypeDTO>().ReverseMap();
        }
    }
}
