namespace WrocRide.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseCustomMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            return app;
        }

        public static WebApplication MapHubs(this WebApplication app)
        {
            app.MapHub<NotificationHub>("notification-hub");

            return app;
        }
    }
}
