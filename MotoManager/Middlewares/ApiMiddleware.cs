namespace MotoManager.Api.Middlewares
{
    public class ApiMiddleware(RequestDelegate next)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                CorrelationId(context);

                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(Result<string>.Fail(Resource.INVALID_DATA)));
            }
        }

        private static void CorrelationId(HttpContext context)
        {
            var correlationId = context.Request.Headers[HeaderConstants.CorrelationIdHeader];

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[HeaderConstants.CorrelationIdHeader] = correlationId;
            }

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderConstants.CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });
        }
    }
}
