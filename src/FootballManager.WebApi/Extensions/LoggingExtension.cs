using Serilog;
using Serilog.Exceptions;

namespace FootballManager.WebApi.Extensions
{
    public static class LoggingExtension
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogging() =>
          (hostingContext, loggerConfiguration) =>
          {
              var env = hostingContext.HostingEnvironment;

              loggerConfiguration
                  .Enrich.FromLogContext()
                  .Enrich.WithThreadId()
                  .Enrich.WithThreadName()
                  .WriteTo.Console()
                  .Enrich.WithMachineName()
                  .Enrich.WithExceptionDetails()
                  .WriteTo.Debug()
                  .WriteTo.File(@"logs\log-.log", rollingInterval: RollingInterval.Day,
                                                        fileSizeLimitBytes: 1024 * 1024,
                                                        rollOnFileSizeLimit: true,
                                                        retainedFileCountLimit: 31)
                  .Enrich.WithProperty("Environment", env.EnvironmentName);
          };
    }
}
