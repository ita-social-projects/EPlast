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
        public string BackEndApiURL => _hostUrl.BackEnd + "/api";
        public string SignInURL => GetFrontEndURL("/signin");
        public string UserTableURL => GetFrontEndURL("/user/table");
        public string ResetPasswordURL => GetFrontEndURL("/resetPassword");
        public string CitiesURL => GetFrontEndURL("/cities");
        public string UserPageMainURL => GetFrontEndURL("/userpage/main");

        public string GetFrontEndURL(string tail)
        {
            return FrontEndURL + tail;
        }

        public string GetSignInURL(int error)
        {
            return $"{SignInURL}?error={error}";
        }

        public string GetResetPasswordURL(string token)
        {
            return $"{ResetPasswordURL}?token={HttpUtility.UrlEncode(token)}";
        }

        public string GetUserTableURL(string search)
        {
            return $"{UserTableURL}?search={HttpUtility.UrlEncode(search)}";
        }

        public string GetUserTableURL((string firstName, string lastName) user)
        {
            return GetUserTableURL($"{user.firstName} {user.lastName}");
        }

        public string GetConfirmEmailApiURL(string userId, string token)
        {
            return $"{BackEndApiURL}/Auth/confirmEmail?userId={userId}&token={HttpUtility.UrlEncode(token)}";
        }

        public string GetUserPageMainURL(string userId)
        {
            return $"{UserPageMainURL}/{userId}";
        }       
        
        public string GetCitiesURL(int cityId)
        {
            return $"{CitiesURL}/{cityId}";
        }
    }
}
