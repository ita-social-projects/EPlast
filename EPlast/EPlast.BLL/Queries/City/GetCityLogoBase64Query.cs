using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityLogoBase64Query : IRequest<string>
    {
        public string LogoName { get; set; }

        public GetCityLogoBase64Query(string logoName)
        {
            LogoName = logoName;
        }
    }
}