namespace PasechnikovaPR33p19.Middleware
{
    public class BooksProtectionMiddleware
    {
        private readonly RequestDelegate _next;

        public BooksProtectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString().StartsWith("/books/"))
            {
                if (httpContext.User.Identity?.IsAuthenticated != true)
                {
                    httpContext.Response.Redirect("/user/login/");
                    return Task.CompletedTask;
                }
            }
            return _next(httpContext);
        }

    }

    public static class BooksProtectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseBooksProtection(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BooksProtectionMiddleware>();
        }
    }

}
