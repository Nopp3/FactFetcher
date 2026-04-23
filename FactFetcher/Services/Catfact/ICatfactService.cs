namespace FactFetcher.Services.Catfact;

public interface ICatfactService
{
    Task<Catfact> GetCatfactAsync(CancellationToken cancellationToken);
}