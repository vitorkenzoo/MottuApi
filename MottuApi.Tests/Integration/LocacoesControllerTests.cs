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
    public class LocacoesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private const string API_KEY = "MottuApi-Secret-Key-2024-Development";

        public LocacoesControllerTests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase("TestDbLocacoes");
                    });
                });
            });

            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-API-KEY", API_KEY);
        }

        [Fact]
        public async Task GetAllLocacoes_DeveRetornar200()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/locacoes");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetLocacao_ComIdInexistente_DeveRetornar404()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/locacoes/99999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateLocacao_SemCliente_DeveRetornar404()
        {
            // Arrange
            var locacao = new
            {
                clienteId = 99999,
                dataFimPrevista = DateTime.UtcNow.AddDays(7).ToString("O")
            };

            var json = JsonSerializer.Serialize(locacao);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/locacoes", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}


