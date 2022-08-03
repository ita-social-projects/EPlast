using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
