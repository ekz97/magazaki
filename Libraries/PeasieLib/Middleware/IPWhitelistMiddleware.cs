using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace PeasieLib.Middleware
{
    public class IPWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPWhitelistOptions _iPWhitelistOptions;
        private readonly ILogger<IPWhitelistMiddleware> _logger;

        public IPWhitelistMiddleware(RequestDelegate next,
        ILogger<IPWhitelistMiddleware> logger,
            IOptions<IPWhitelistOptions> applicationOptionsAccessor)
        {
            _iPWhitelistOptions = applicationOptionsAccessor.Value;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger?.LogDebug("-> IPWhitelistMiddleware");
            //if (context.Request.Method != HttpMethod.Get.Method)
            {
                var ipAddress = context.Connection.RemoteIpAddress;
                List<string>? whiteListIPList =
                _iPWhitelistOptions.Whitelist;
                var isIPWhitelisted = whiteListIPList?.Where(ip => IPAddress.Parse(ip)
                .Equals(ipAddress))
                .Any();
                if (isIPWhitelisted != null)
                {
                    if ((bool)!isIPWhitelisted)
                    {
                        _logger.LogWarning("Request from Remote IP address: {RemoteIp} is forbidden.", ipAddress); 
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        _logger?.LogDebug("<- IPWhitelistMiddleware");
                        return;
                    }
                }
            }
            _logger?.LogDebug("<- IPWhitelistMiddleware");
            await _next.Invoke(context);
        }
    }
}