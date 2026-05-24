using CETrackerApi.Security;

namespace CETrackerApi.Middleware;

public class TokenAccessorMiddleware : IMiddleware
{
    private TokenAccessor _tokenAccessor { get; set; }

    public TokenAccessorMiddleware(TokenAccessor tokenAccessor)
    {
        _tokenAccessor = tokenAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var bearerToken = context.Request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);
        _tokenAccessor.DecodeToken(bearerToken);

        await next(context);
    }
}
