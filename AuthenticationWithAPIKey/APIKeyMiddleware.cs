using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AuthenticationWithAPIKey
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class APIKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public APIKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(
                "api-key", out var apikeyHeader))
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("UnAutorize");
                return;
            }
            var apikeyvalue = _configuration.GetValue<string>("ApiKey");
            if (!apikeyvalue.Equals(apikeyHeader))
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Invalid Api Key");
                return;
            }
            await  _next(httpContext);
        }
    }

  
}
