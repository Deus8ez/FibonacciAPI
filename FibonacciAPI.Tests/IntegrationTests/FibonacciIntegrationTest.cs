using FibonacciAPI.Controllers;
using FibonacciAPI.DTOs;
using FibonacciAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Policy;
using System.Text.Json;

namespace FibonacciAPI.Tests.IntegrationTests
{
    public class FibonacciIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        
        private readonly WebApplicationFactory<Program> _factory;
        private int startIndex = 0;
        private int endIndex = 5;
        private bool fromCache = true;
        private int timeout = 100;
        public FibonacciIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact(Timeout = 100)]
        public async Task ReturnsCorrectSequence()
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IFibonacciCache>();

            // Act
            var response = await client.GetAsync($"api/fibonacci?startIndex={startIndex}&endIndex={endIndex}&fromCache={fromCache}&milliseconds={timeout}");
            var result = JsonSerializer.Deserialize<GetSequenceResponse>(await response.Content.ReadAsStringAsync());
            var cachedArray = cache.GetArrayWithLongestSequence();

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(new int[] { 0, 1, 1, 2, 3 }, result?.sequence);
            Assert.Equal(cachedArray[..^1], result?.sequence);
        }

        [Fact(Timeout = 100)]
        public async Task ReturnsCorrectSequence_LongerSequenceCached()
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IFibonacciCache>();
            var newEndIndex = 10;

            // Act
            await client.GetAsync($"api/fibonacci?startIndex={startIndex}&endIndex={newEndIndex}&fromCache={fromCache}&milliseconds={timeout}");
            var response = await client.GetAsync($"api/fibonacci?startIndex={startIndex}&endIndex={endIndex}&fromCache={fromCache}&milliseconds={timeout}");
            var result = JsonSerializer.Deserialize<GetSequenceResponse>(await response.Content.ReadAsStringAsync());
            var cachedArray = cache.GetArrayWithLongestSequence();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 }, cachedArray[..^1]);
            Assert.NotEqual(cachedArray[..^1], result?.sequence);
        }

        [Fact(Timeout = 100)]
        public async Task ShouldThrowIncorrectIndexException()
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IFibonacciCache>();
            startIndex = 5;
            endIndex = 0;

            // Act
            var response = await client.GetAsync($"api/fibonacci?startIndex={startIndex}&endIndex={endIndex}&fromCache={fromCache}&milliseconds={timeout}");

            // Assert
            Assert.Equal(400, (int)response.StatusCode);
        }
    }
}
