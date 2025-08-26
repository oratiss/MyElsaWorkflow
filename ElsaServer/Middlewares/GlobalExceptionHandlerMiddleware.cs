using Refit;
using System.Text.Json;

namespace ElsaServer.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            IHostEnvironment hostEnvironment,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            this._next = next;
            this._hostEnvironment = hostEnvironment;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            this._logger.LogError(exception.Message, (object)exception);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            string text = exception.Message /*: "General Error Happened."*/;
            return context.Response.WriteAsync(text);
        }
    }
}
