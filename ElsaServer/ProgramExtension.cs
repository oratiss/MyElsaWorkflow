using ElsaServer.Middlewares;

namespace ElsaServer
{
    public static class ProgramExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app) => app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
