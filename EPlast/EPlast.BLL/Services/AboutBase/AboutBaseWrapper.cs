using EPlast.BLL.Interfaces.AboutBase;

namespace EPlast.BLL.Services.AboutBase
{
    public class AboutBaseWrapper : IAboutBaseWrapper
    {
        public IAboutBasePicturesManager AboutBasePicturesManager { get; }

        public AboutBaseWrapper(IAboutBasePicturesManager aboutBasePicturesManager)
        {
            AboutBasePicturesManager = aboutBasePicturesManager;
        }
    }
}
