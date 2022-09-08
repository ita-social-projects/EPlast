using EPlast.BLL.Interfaces.HostURL;
using Microsoft.Extensions.Options;

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
    }
}
