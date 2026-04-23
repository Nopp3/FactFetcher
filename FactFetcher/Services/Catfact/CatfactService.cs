namespace FactFetcher.Services.Catfact;

public class CatfactService(HttpClient httpClient) : ICatfactService
{
    public async Task<Catfact> GetCatfactAsync(CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync("fact", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var fact = await response.Content.ReadFromJsonAsync<Catfact>(cancellationToken);

        if (fact?.Length == null || fact.Fact == null)
            throw new CatfactApiException("Problem with catfact response");
        
        return fact;
    }
}