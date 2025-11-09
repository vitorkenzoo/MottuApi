using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace MottuApi.Tests.Integration
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HealthCheckTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            // Health check não requer API Key
        }

        [Fact]
        public async Task HealthCheck_DeveRetornar200()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthCheck_SemAPIKey_DeveFuncionar()
        {
            // Arrange
            var clientSemKey = _factory.CreateClient();

            // Act
            var response = await clientSemKey.GetAsync("/health");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}


