using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using GatewayService.Utils;

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
            context.User = null;
            try
            {
                var authorizationHeader = context.Request.Headers.Authorization.ToString();
                
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();

                    var principal = JWTManager.DecodeJwt(jwtToken);

                    context.User = principal;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

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