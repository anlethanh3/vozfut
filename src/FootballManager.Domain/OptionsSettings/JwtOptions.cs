namespace FootballManager.Domain.OptionsSettings
{
    public class JwtOptions
    {
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
