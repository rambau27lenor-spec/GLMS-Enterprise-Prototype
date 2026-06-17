using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace GLMS_ST10104382.Tests
{
    public class ApiIntegrationTests :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContracts_ReturnsStatusCode200()
        {
            var response = await _client.GetAsync("/api/contracts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetContracts_ReturnsJsonContent()
        {
            var response = await _client.GetAsync("/api/contracts");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(content));
        }
    }
}