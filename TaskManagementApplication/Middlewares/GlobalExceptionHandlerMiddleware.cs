namespace TaskManagementApplication.Middlewares
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
            _next = next;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception.Message, exception);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            string text = exception.Message /*: "General Error Happened."*/;
            return context.Response.WriteAsync(text);
        }
    }
}
