using AP.CityDataDemo.Presentation.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AP.CityDataDemo.Presentation.Extensions
{
    public static class Registrator
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware (this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}
