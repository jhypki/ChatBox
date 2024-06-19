using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Middleware;
public class UserIdMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var jwtToken = context.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (jwtToken != null)
            {
                context.Items["UserId"] = jwtToken;
            }
        }

        await _next(context);
    }
}
