namespace AuthService.Helpers
{
    public class JwtSecureMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtSecureMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            //Читаем токен из куки
            var token = context.Request.Cookies[".Application.DeveloperCode"];
            //Если токен есть
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }

            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Xss-Protection", "1");
            context.Response.Headers.Add("X-Frame-Options", "DENY");

            await _next(context);
        }
    }
}
