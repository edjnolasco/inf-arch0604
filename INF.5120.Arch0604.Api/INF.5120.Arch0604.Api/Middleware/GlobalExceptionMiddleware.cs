using System.Text.Json;
using INF._5120.Arch0604.Api.Common;

namespace INF._5120.Arch0604.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}, Method: {Method}",
                    traceId,
                    context.Request.Path,
                    context.Request.Method);

                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ApiErrorResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Ocurrió un error interno en el servidor.",
                Details = _environment.IsDevelopment() ? exception.Message : null,
                TraceId = traceId,
                TimestampUtc = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}