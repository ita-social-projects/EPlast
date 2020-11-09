using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubDocumentsProfile : Profile
    {
        public ClubDocumentsProfile()
        {
            CreateMap<DatabaseEntities.ClubDocuments, ClubDocumentsDTO>().ReverseMap();
            CreateMap<DatabaseEntities.ClubDocumentType, ClubDocumentTypeDTO>().ReverseMap();
        }
    }
}
