using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubDocumentsProfile : Profile
    {
        public ClubDocumentsProfile()
        {
            CreateMap<DatabaseEntities.ClubDocuments, ClubDocumentsDto>().ReverseMap();
            CreateMap<DatabaseEntities.ClubDocumentType, ClubDocumentTypeDto>().ReverseMap();
        }
    }
}
