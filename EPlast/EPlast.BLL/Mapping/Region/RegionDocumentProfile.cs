using AutoMapper;
using EPlast.BLL.DTO.Region;


namespace EPlast.BLL.Mapping.Region
{
    public class RegionDocumentProfile:Profile
    {
        public RegionDocumentProfile()
        {
            CreateMap<DataAccess.Entities.RegionDocuments, RegionDocumentDto>().ReverseMap();
        }


       
    }
}
