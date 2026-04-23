# FactFetcher

Simple ASP.NET Core Web API that fetches cat facts from `https://catfact.ninja/fact` and stores them in a local `.txt` file.

## Features

- fetches cat facts from an external API
- appends each fetched fact to a local text file
- exposes endpoints for saving and reading facts
- includes unit and integration tests

## Requirements

- .NET 10 SDK

## Run

```bash
dotnet run --project FactFetcher
```

By default the application runs on:

- `http://localhost:5050`

## Endpoints

Fetch a new cat fact and append it to the file:

```bash
curl -i -X POST http://localhost:5050/fact
```

Read saved facts:

```bash
curl -i http://localhost:5050/fact
```

## Storage

Saved facts are stored in:

```text
FactFetcher/.runtime/result.txt
```

## Tests

Run all tests with:

```bash
dotnet test
```
