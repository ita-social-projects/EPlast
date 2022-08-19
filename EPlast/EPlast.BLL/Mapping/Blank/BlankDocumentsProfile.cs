using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.DataAccess.Entities.Blank;

namespace EPlast.BLL.Mapping.Blank
{
    class BlankDocumentsProfile : Profile
    {
        public BlankDocumentsProfile()
        {
            CreateMap<BlankBiographyDocuments, BlankBiographyDocumentsDto>().ReverseMap();
            CreateMap<AchievementDocuments, AchievementDocumentsDto>().ReverseMap();
            CreateMap<ExtractFromUpuDocuments, ExtractFromUpuDocumentsDto>().ReverseMap();
           
        }
    }
}
