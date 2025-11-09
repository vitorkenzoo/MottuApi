using System.Net;

namespace MottuApi.Middleware
{
    /// <summary>
    /// Middleware para validação de API Key via header X-API-KEY
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            // Permite acesso ao Swagger e Health Check sem API Key
            var path = context.Request.Path.Value?.ToLower() ?? "";
            if (path.StartsWith("/swagger") || path.StartsWith("/health"))
            {
                await _next(context);
                return;
            }

            // Verifica se o header X-API-KEY está presente
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("API Key não fornecida. Por favor, inclua o header X-API-KEY.");
                return;
            }

            // Obtém a API Key configurada no appsettings.json
            var apiKey = configuration["ApiKey:SecretKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("API Key não configurada no servidor.");
                return;
            }

            // Valida se a API Key fornecida corresponde à configurada
            if (!apiKey.Equals(extractedApiKey.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("API Key inválida.");
                return;
            }

            // Se passou na validação, continua para o próximo middleware
            await _next(context);
        }
    }
}


