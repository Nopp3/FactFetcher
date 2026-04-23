using FactFetcher.Services.Catfact;
using FactFetcher.Services.Storage;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ICatfactService, CatfactService>(client =>
    client.BaseAddress = new Uri("https://catfact.ninja/"));

builder.Services.AddTransient<IFileService, FileService>();

var app = builder.Build();

app.MapPost("/fact", async (ICatfactService catfactService, IFileService fileService, CancellationToken cancellationToken) =>
{
    var catfact = await catfactService.GetCatfactAsync(cancellationToken);
    var content = $"{catfact.Fact} | Length: {catfact.Length}";
    await fileService.AppendLineAsync("result.txt", content, cancellationToken);
    return Results.Ok("Correctly added: " + content);
});

app.MapGet("/fact", async (IFileService fileService, CancellationToken cancellationToken) =>
{
    var content = await  fileService.ReadAllTextAsync("result.txt", cancellationToken);
    
    return string.IsNullOrEmpty(content) ? Results.NoContent() : Results.Text(content, "text/plain; charset=utf-8");
});

app.Run();