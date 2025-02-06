using Microsoft.Extensions.Caching.Memory;

namespace HRAnalytics.API.Middleware
{
    // HRAnalytics.API/Middleware/CachingMiddleware.cs
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;

        public CachingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // POST, PUT, DELETE isteklerinde cache'i temizle
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                // Cache invalidation logic
                var cache = context.RequestServices.GetService<IMemoryCache>();
                var path = context.Request.Path.Value?.ToLower();
                if (path?.StartsWith("/api/") == true)
                {
                    var cacheKey = path.Split('/')[3]; // entity type
                    cache?.Remove(cacheKey);
                }
            }

            await _next(context);
        }
    }

    
}
