
using EPlast.Resources;
using Microsoft.Extensions.Localization;

namespace EPlast.BLL.Interfaces.Resources
{
    public interface IResources
    {
        IStringLocalizer<AuthenticationErrors> ResourceForErrors { get; }
        IStringLocalizer<Genders> ResourceForGender { get; }
    }
}
