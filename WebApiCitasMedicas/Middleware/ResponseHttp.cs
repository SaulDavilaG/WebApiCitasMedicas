
namespace WebApiCitasMedicas.Middleware
{

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseResponeHttp(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ResponseHttp>();
        }
    }

    public class ResponseHttp
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ResponseHttp> logger;

        public ResponseHttp(RequestDelegate next, ILogger<ResponseHttp> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            using (var ms= new MemoryStream())
            {
                var BodyOriginal = context.Response.Body;
                context.Response.Body = ms;

                await next(context);

                ms.Seek(0, SeekOrigin.Begin);
                string response = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(BodyOriginal);
                context.Response.Body = BodyOriginal;

                logger.LogInformation(response);
            }
        }
    }
}
