using EPlast.BLL.Interfaces.HostURL;
using Microsoft.Extensions.Options;
using System.Web;

namespace EPlast.BLL.Services.HostURL
{
    public class HostURLService : IHostURLService
    {
        private readonly HostURLOptions _hostUrl;

        public HostURLService(IOptions<HostURLOptions> options)
        {
            _hostUrl = options.Value;
        }

        public string FrontEndURL => _hostUrl.FrontEnd;
        public string BackEndURL => _hostUrl.BackEnd;

        public string GetFrontEndURL(string tail)
        {
            return FrontEndURL + tail;
        }

        public string GetSignInURL()
        {
            return GetFrontEndURL("/signin");
        }

        public string GetSignInURL(string tail)
        {
            return GetSignInURL() + tail;
        }
        
        public string GetSignInURL(int error)
        {
            return GetSignInURL($"?error={error}");
        }

        public string GetResetPasswordURL(string token)
        {
            return GetFrontEndURL($"/resetPassword?token={HttpUtility.UrlEncode(token)}");
        }
    }
}
