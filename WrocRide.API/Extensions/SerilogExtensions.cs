namespace WrocRide.API.Extensions
{
    public static class SerilogExtensions
    {
        public static IHostBuilder UseConfiguredSerilog(this IHostBuilder host)
        {
            host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            return host;
        }

    }
}
