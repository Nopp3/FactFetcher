using FactFetcher.Services.Catfact;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ICatfactService, CatfactService>(client =>
    client.BaseAddress = new Uri("https://catfact.ninja/"));

var app = builder.Build();

app.MapGet("/fact", async (ICatfactService catfactService, CancellationToken cancellationToken) =>
    await catfactService.GetCatfactAsync(cancellationToken));

app.Run();