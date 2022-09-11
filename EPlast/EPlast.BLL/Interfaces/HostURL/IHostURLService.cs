namespace EPlast.BLL.Interfaces.HostURL
{
    public interface IHostURLService
    {
        string BackEndURL { get; }
        string FrontEndURL { get; }
        string GetFrontEndURL(string tail);
        string GetSignInURL();
        string GetSignInURL(string tail);
        string GetSignInURL(int error);
        string GetResetPasswordURL(string token);
    }
}