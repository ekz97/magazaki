using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace PeasieLib.Extensions;

public static class HttpContextExtensions
{
    public static string ResolveClientIpAddress(this HttpContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("HttpContext is null");
        }

        IHttpConnectionFeature? connection = context.Features.Get<IHttpConnectionFeature>();

        if (connection == null || connection.RemoteIpAddress == null)
        {
            throw new ArgumentNullException("IHttpConnectionFeature is null");
        }

        return connection.RemoteIpAddress.ToString();
    }

    public static string ResolveServerIpAddress(this HttpContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("HttpContext is null");
        }

        IHttpConnectionFeature? connection = context.Features.Get<IHttpConnectionFeature>();

        if (connection == null || connection.LocalIpAddress == null)
        {
            throw new ArgumentNullException("IHttpConnectionFeature is null");
        }

        return connection.LocalIpAddress.ToString();
    }
}