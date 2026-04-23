using System.Net;
using FactFetcher.DTOs;
using FactFetcher.Services.Catfact;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FactFetcher.Tests.Integration;

public class FactEndpointIntegrationTests
{
    [Fact]
    public async Task PostFact_AppendsFactToFile_AndGetFactReturnsSavedContent()
    {
        using var factory = new TestAppFactory();
        using var client = factory.CreateClient();
        
        var getResponseNoContent = await client.GetAsync("/fact");
        getResponseNoContent.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, getResponseNoContent.StatusCode);

        var postResponse = await client.PostAsync("/fact", null);
        postResponse.EnsureSuccessStatusCode();
        
        var filePath = Path.Combine(factory.TempDirectory, ".runtime", "result.txt");
        Assert.True(File.Exists(filePath));
        
        var fileContent = await File.ReadAllTextAsync(filePath);
        Assert.Equal("Fact 1 | Length: 6\n", fileContent);
        
        var getResponseOk = await client.GetAsync("/fact");
        getResponseOk.EnsureSuccessStatusCode();
        Assert.Equal("Fact 1 | Length: 6\n", await getResponseOk.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.OK, getResponseOk.StatusCode);
    }

    private class TestAppFactory : WebApplicationFactory<Program>
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        public TestAppFactory()
        {
            Directory.CreateDirectory(TempDirectory);
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(TempDirectory);
            
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ICatfactService>();
                services.AddSingleton<ICatfactService, FakeCatfactService>();
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (Directory.Exists(TempDirectory))
                Directory.Delete(TempDirectory, true);
            base.Dispose(disposing);
        }
    }

    private class FakeCatfactService : ICatfactService
    {
        public Task<Catfact> GetCatfactAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new Catfact
            {
                Fact = "Fact 1",
                Length = 6
            });
        }
    }
}