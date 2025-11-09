using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MottuApi.Core.Interfaces;
using MottuApi.Data;
using MottuApi.Infrastructure.Repositories;
using MottuApi.Middleware;
using MottuApi.Services;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Conexão com o Banco de Dados ---
var connectionString = builder.Configuration.GetConnectionString("OracleDb");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));

// --- 1.5. Health Checks ---
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("oracle_db");

// --- 2. Injeção de Dependência (DI) ---
// Registra os repositórios
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ILocacaoRepository, LocacaoRepository>();

// Registra os serviços
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ILocacaoService, LocacaoService>();
builder.Services.AddSingleton<IPredictionService, PredictionService>();

// --- 3. AutoMapper ---
// Registra o AutoMapper, que irá procurar por perfis de mapeamento no projeto
builder.Services.AddAutoMapper(typeof(Program));

// --- 4. Configuração de Controllers e Serialização JSON ---
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Converte Enums para strings no JSON de resposta (ex: "Disponivel" em vez de 0)
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


// --- 4.5. Configuração de Versionamento da API ---
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader()
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();

// --- 5. Configuração do Swagger ---
builder.Services.AddSwaggerGen(options =>
{
    // Configuração de segurança para API Key
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key necessária para acessar os endpoints. Inclua no header como: X-API-KEY",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, new List<string>() }
    };

    options.AddSecurityRequirement(securityRequirement);

    // Habilita o uso dos comentários XML na documentação do Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});


var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"Mottu API {description.GroupName.ToUpperInvariant()}"
            );
        }
    });
}

app.UseHttpsRedirection();

// Adiciona o middleware de API Key antes do Authorization
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

// Mapeia o endpoint de Health Check (não requer API Key)
app.MapHealthChecks("/health");

app.MapControllers();

// --- Seed Data: Popular banco com dados iniciais se estiver vazio ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await DataSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao popular banco de dados com dados iniciais.");
    }
}

app.Run();

// Torna a classe Program acessível para testes de integração
public partial class Program { }
