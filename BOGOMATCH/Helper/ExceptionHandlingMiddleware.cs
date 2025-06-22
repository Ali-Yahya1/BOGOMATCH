using Serilog;

namespace BOGOMATCH.Helper
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);  
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while processing request {RequestPath}", context.Request.Path);
                Log.Error("----------------");
                context.Response.ContentType = "application/json";
                switch (ex)
                {
                    case ArgumentException _:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("{\"error\": \"Bad request, invalid argument\"}");
                        break;

                    case UnauthorizedAccessException _:
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("{\"error\": \"Unauthorized access\"}");
                        break;

                    case InvalidOperationException _:
                        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                        await context.Response.WriteAsync("{\"error\": \"Invalid operation\"}");
                        break;

                    case Exception _:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync("{\"error\": \"An unexpected error occurred\"}");
                        break;
                }
            }
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
