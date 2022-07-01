using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.DataAccess.Entities.Blank;

namespace EPlast.BLL.Mapping.Blank
{
    class BlankDocumentsProfile : Profile
    {
        public BlankDocumentsProfile()
        {
            CreateMap<BlankBiographyDocuments, BlankBiographyDocumentsDTO>().ReverseMap();
            CreateMap<AchievementDocuments, AchievementDocumentsDTO>().ReverseMap();
            CreateMap<ExtractFromUPUDocuments, ExtractFromUPUDocumentsDTO>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
        }
    }
}
