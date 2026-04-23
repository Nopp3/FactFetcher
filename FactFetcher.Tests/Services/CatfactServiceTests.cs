using System.Net;
using FactFetcher.Services.Catfact;

namespace FactFetcher.Tests.Services;

public class CatfactServiceTests
{
    [Fact]
    public async Task GetCatfactAsync_Returns_Catfact()
    {
        const string responseBody = """
                                    {
                                    "fact": "Baking chocolate is the most dangerous chocolate to your cat.",
                                    "length": 61
                                    }
                                    """;
        using var httpClient = new HttpClient(new FakeHttpMessageHandler(responseBody));
        httpClient.BaseAddress = new Uri("https://catfact.ninja/");

        var service = new CatfactService(httpClient);
        
        var result = await service.GetCatfactAsync(CancellationToken.None);
        
        Assert.Equal(61, result.Length);
        Assert.Equal("Baking chocolate is the most dangerous chocolate to your cat.", result.Fact);
    }
    
    [Fact]
    public async Task GetCatfactAsync_Throws_Exception_When_Fact_Is_Not_Found()
    {
        const string responseBody = """
                                    {

                                    "length": 61
                                    }
                                    """;
        using var httpClient = new HttpClient(new FakeHttpMessageHandler(responseBody));
        httpClient.BaseAddress = new Uri("https://catfact.ninja/");

        var service = new CatfactService(httpClient);
        
        await Assert.ThrowsAsync<CatfactApiException>(async () => await service.GetCatfactAsync(CancellationToken.None));
    }
    
    [Fact]
    public async Task GetCatfactAsync_Throws_Exception_When_Length_Is_Not_Found()
    {
        const string responseBody = """
                                    {
                                    "fact": "Baking chocolate is the most dangerous chocolate to your cat."
                                    
                                    }
                                    """;
        using var httpClient = new HttpClient(new FakeHttpMessageHandler(responseBody));
        httpClient.BaseAddress = new Uri("https://catfact.ninja/");

        var service = new CatfactService(httpClient);
        
        await Assert.ThrowsAsync<CatfactApiException>(async () => await service.GetCatfactAsync(CancellationToken.None));
    }
    
    private class FakeHttpMessageHandler(string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(statusCode);
            response.Content = new StringContent(responseBody);
            return Task.FromResult(response);
        }
    }
}