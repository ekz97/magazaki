using Microsoft.AspNetCore.Http;

namespace PeasieLib
{
    public static class ResultsExtensions
    {
        public static IResult Html(this IResultExtensions resultExtensions, string html)
        {
            ArgumentNullException.ThrowIfNull(resultExtensions, nameof(resultExtensions));

            return new HtmlResult(html);
        }
    }
}
