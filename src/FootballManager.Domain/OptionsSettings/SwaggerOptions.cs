namespace FootballManager.Domain.OptionsSettings
{
    public class SwaggerOptions
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public Contact Contact { get; set; }
        public License License { get; set; }
    }

    public class Contact
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }

    public class License
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }
}
