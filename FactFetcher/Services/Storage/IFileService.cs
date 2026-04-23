namespace FactFetcher.Services.Storage;

public interface IFileService
{
    Task AppendLineAsync(string fileName, string content, CancellationToken cancellationToken);
}