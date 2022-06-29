using AutoMapper;
using EPlast.BLL.DTO.Account;

namespace EPlast.BLL.Mapping.User
{
    public class RegisterDto_User : Profile
    {
        public RegisterDto_User()
        {
            CreateMap<RegisterDto, DataAccess.Entities.User>()
                .ForMember(
                    u => u.UserName,
                    o => o.MapFrom(dto => dto.Email)
                )
                .ForMember(
                    u => u.UserProfile,
                    o => o.MapFrom(dto => new DataAccess.Entities.UserProfile()
                    {
                        Birthday = dto.Birthday,
                        Address = dto.Address,
                        GenderID = dto.GenderId,
                        TwitterLink = dto.TwitterLink,
                        FacebookLink = dto.FacebookLink,
                        InstagramLink = dto.InstagramLink,
                        Oblast = dto.Oblast,
                        Referal = dto.Referal
                    })
                );
        }
    }
}
