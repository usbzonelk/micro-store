using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GatewayService.Middlewares
{
    public class AuthCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Modify or add data to the request
            context.Items["MyKey"] = "MyValue";

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    public static class AuthCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthCheckMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthCheckMiddleware>();
        }
    }
}