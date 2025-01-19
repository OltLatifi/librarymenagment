namespace librarymenagment.MIddlewares
{
    public class AdminRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated && context.Request.Path == "/")
            {
                if (context.User.IsInRole("Admin"))
                {
                    context.Response.Redirect("/Admin/Categories");
                    return;
                }
            }

            await _next(context);
        }
    }

}
