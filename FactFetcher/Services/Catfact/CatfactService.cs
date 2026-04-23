namespace FactFetcher.Services.Catfact;

public class CatfactService(HttpClient httpClient) : ICatfactService
{
    public async Task<DTOs.Catfact> GetCatfactAsync(CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync("fact", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var fact = await response.Content.ReadFromJsonAsync<DTOs.Catfact>(cancellationToken);

        if (fact?.Length == null || fact.Fact == null)
            throw new CatfactApiException("Problem with catfact response");
        
        return fact;
    }
}