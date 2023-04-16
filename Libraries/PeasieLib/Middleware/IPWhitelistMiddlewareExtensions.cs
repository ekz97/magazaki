using Microsoft.AspNetCore.Builder;

namespace PeasieLib.Middleware
{
    public static class IPWhitelistMiddlewareExtensions
    {
        public static IApplicationBuilder UseIPWhitelist(this
        IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPWhitelistMiddleware>();
        }
    }
}