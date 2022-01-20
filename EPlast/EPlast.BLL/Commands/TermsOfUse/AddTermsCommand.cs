using EPlast.BLL.DTO.Terms;
using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Commands.TermsOfUse
{
    public class AddTermsCommand : IRequest
    {
        public TermsDTO TermsDto { get; set; }
        public User User { get; set; }

        public AddTermsCommand(TermsDTO termsDto, User user)
        {
            TermsDto = termsDto;
            User = user;
        }
    }
}