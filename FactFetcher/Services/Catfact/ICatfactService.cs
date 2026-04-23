namespace FactFetcher.Services.Catfact;

public interface ICatfactService
{
    Task<DTOs.Catfact> GetCatfactAsync(CancellationToken cancellationToken);
}