using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MottuApi.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace MottuApi.Tests.Integration
{
    public class ClientesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private const string API_KEY = "MottuApi-Secret-Key-2024-Development";

        public ClientesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o DbContext do Oracle
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Adiciona o DbContext em memória
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });

            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-API-KEY", API_KEY);
        }

        [Fact]
        public async Task CreateCliente_ComDadosValidos_DeveRetornar201()
        {
            // Arrange
            var cliente = new
            {
                nome = "João Silva",
                cpf = "12345678900",
                dataNascimento = "1990-05-20",
                numeroCNH = "98765432100",
                tipoCNH = "A"
            };

            var json = JsonSerializer.Serialize(cliente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/clientes", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateCliente_SemAPIKey_DeveRetornar401()
        {
            // Arrange
            var clientSemKey = _factory.CreateClient();
            var cliente = new
            {
                nome = "João Silva",
                cpf = "12345678901",
                dataNascimento = "1990-05-20",
                numeroCNH = "98765432101",
                tipoCNH = "A"
            };

            var json = JsonSerializer.Serialize(cliente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await clientSemKey.PostAsync("/api/v1/clientes", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetCliente_ComIdInexistente_DeveRetornar404()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/clientes/99999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllClientes_DeveRetornar200()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/clientes");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}


