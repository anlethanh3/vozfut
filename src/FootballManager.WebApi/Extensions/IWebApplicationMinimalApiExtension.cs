namespace FootballManager.WebApi.Extensions
{
    public static class IWebApplicationMinimalApiExtension
    {
        public static WebApplication UseMinimalApi(this WebApplication app)
        {
            return app.UseV1Builder();
        }

        private static WebApplication UseV1Builder(this WebApplication app)
        {

            return app;
        }
    }
}
