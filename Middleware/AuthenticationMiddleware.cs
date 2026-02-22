using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string TokenHeader = "Authorization";
        private const string ValidToken = "Bearer mysecrettoken"; // Example token

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(TokenHeader, out var token) || token != ValidToken)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{ \"error\": \"Unauthorized\" }");
                return;
            }
            await _next(context);
        }
    }
}
