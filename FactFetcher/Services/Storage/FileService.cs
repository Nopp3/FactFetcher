namespace FactFetcher.Services.Storage;

public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
{
    public async Task AppendLineAsync(string fileName, string content, CancellationToken cancellationToken)
    {
        var directory = Path.Combine(webHostEnvironment.ContentRootPath, ".runtime");
        Directory.CreateDirectory(directory);
        var filePath = Path.Combine(directory, fileName);
        
        await File.AppendAllTextAsync(filePath, content + "\n", cancellationToken);
    }
}