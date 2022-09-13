using EPlast.BLL.DTO.Terms;
using EPlast.DataAccess.Entities;
using MediatR;

namespace EPlast.BLL.Commands.TermsOfUse
{
    public class ChangeTermsCommand : IRequest
    {
        public TermsDto TermsDto { get; set; }
        public User User { get; set; }

        public ChangeTermsCommand(TermsDto termsDto, User user)
        {
            TermsDto = termsDto;
            User = user;
        }
    }
}