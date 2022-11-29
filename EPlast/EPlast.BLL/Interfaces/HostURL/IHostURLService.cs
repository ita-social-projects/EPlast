namespace EPlast.BLL.Interfaces.HostURL
{
    public interface IHostUrlService
    {
        string BackEndApiURL { get; }
        string FrontEndURL { get; }
        string SignInURL { get; }
        string CitiesURL { get; }
        string GetSignInURL(int error);
        string GetResetPasswordURL(string token);
        string GetUserTableURL(string search);
        string GetUserTableURL((string firstName, string lastName) user);
        string GetConfirmEmailApiURL(string userId, string token);
        string GetUserPageMainURL(string userId);
        string GetCitiesURL(int userId);
    }
}