namespace EPlast.BLL.Services.Jwt
{
    public class JwtOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Time { get; set; }
    }
}
