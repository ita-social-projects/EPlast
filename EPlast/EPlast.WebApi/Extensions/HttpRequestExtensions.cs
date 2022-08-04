using Microsoft.AspNetCore.Http;
using System.Web;

namespace EPlast.WebApi.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetFrontEndURL(this HttpRequest request)
        {
            var frontendUrl = request?.Host.Host ?? "localhost";
            if (frontendUrl == "localhost" || frontendUrl == "127.0.0.1")
            {
                frontendUrl = "http://" + frontendUrl + ":3000";
            }
            else
            {
                frontendUrl = "https://" + frontendUrl;
            }
            return frontendUrl;
        }

        public static string GetFrontEndURL(this HttpRequest request, string tail)
        {
            return request.GetFrontEndURL() + (tail ?? "");
        }

        public static string GetFrontEndSignInURL(this HttpRequest request)
        {
            return request.GetFrontEndURL("/signin");
        }

        public static string GetFrontEndResetPasswordURL(this HttpRequest request, string token)
        {
            return request.GetFrontEndURL($"/resetPassword?token={HttpUtility.UrlEncode(token)}");
        }
    }
}
