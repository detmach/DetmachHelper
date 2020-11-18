using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Helper.Detmach
{
    public static class CanonicalUrlMiddlewareExtensions
    {
        public static IApplicationBuilder UseCanonicalUrlMiddleware(this IApplicationBuilder builder, CanonicalURLMiddlewareOptions options)
        {
            return builder.UseMiddleware<CanonicalURLMiddleware>(options);
        }
    }
    public class CanonicalURLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CanonicalURLMiddlewareOptions _options;
        public CanonicalURLMiddleware(RequestDelegate next, CanonicalURLMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }
        public async Task Invoke(HttpContext context)
        {
            var canonicalUrl = context.Request.Path.ToString();
            if (_options.RemoveTrailingSlash)
            {
                if (canonicalUrl.Length > 1 && string.Equals(canonicalUrl[canonicalUrl.Length - 1], '/'))
                {
                    canonicalUrl = canonicalUrl.Substring(0, canonicalUrl.Length - 1);
                }
            }

            var queryString = context.Request.QueryString.ToString();
            if (_options.EnforceLowerCaseUrls)
            {
                //If you want lowercase urls but the querystrings are case sensitive
                if (_options.QueryStringCaseSensitive && !string.IsNullOrEmpty(queryString))
                {
                    canonicalUrl = canonicalUrl.ToLower() + queryString;
                }
                else
                {
                    canonicalUrl = (canonicalUrl + queryString).ToLower(); ;
                }
            }

            var oldPath = context.Request.Path.ToString() + context.Request.QueryString.ToString();
            if (!string.Equals(canonicalUrl, oldPath))
            {
                context.Response.Redirect(canonicalUrl);
            }

            await _next.Invoke(context);
        }
    }

    /// <summary>
    /// Options for CanonicalUrlConfigurations
    /// </summary>
    public class CanonicalURLMiddlewareOptions
    {
        /// <summary>
        /// A flag that tell the middlware if it should make the urls lowercase
        /// </summary>
        public bool EnforceLowerCaseUrls { get; set; }

        /// <summary>
        /// A flag the represents if the QUeryString of the url is case sensitive
        /// and should not be altered
        /// </summary>
        public bool QueryStringCaseSensitive { get; set; }

        /// <summary>
        /// A flag that represents if the middleware should remove the trailing
        /// slash from the middleware
        /// </summary>
        public bool RemoveTrailingSlash { get; set; }

        public CanonicalURLMiddlewareOptions() { }

    }
}
