namespace EPlast.BLL.Interfaces.HostURL
{
    public interface IHostURLService
    {
        string BackEndURL { get; }
        string FrontEndURL { get; }
        string GetSignInURL();
        string GetSignInURL(int error);
        string GetResetPasswordURL(string token);
        public string GetUserTableURL(string search);
        public string GetUserTableURL((string firstName, string lastName) user);
    }
}