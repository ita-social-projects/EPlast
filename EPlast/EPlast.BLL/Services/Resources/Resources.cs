
using EPlast.BLL.Interfaces.Resources;
using EPlast.Resources;
using Microsoft.Extensions.Localization;

namespace EPlast.BLL.Services.Resources
{
    public class Resources: IResources
    {
        public Resources(IStringLocalizer<AuthenticationErrors> resourceForErrors,
            IStringLocalizer<Genders> resourceForGender)
        {
            ResourceForErrors = resourceForErrors;
            ResourceForGender = resourceForGender;
        }

        public IStringLocalizer<AuthenticationErrors> ResourceForErrors { get; }
        public IStringLocalizer<Genders> ResourceForGender { get; }

    }
}
