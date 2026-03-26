# IntelliAssist ✨

Blazor Server app for simple AI-powered utilities (chat, translate, summarize) and a movie recommendation demo backed by EF Core.

## Prerequisites 🧰

- .NET SDK 10
- SQL Server (LocalDB or SQL Server)
- Ollama running locally (for AI features)

## Configure ⚙️

Update connection string in `IntelliAssist/appsettings.json`:

- `ConnectionStrings:DefaultConnection`

## Database (EF Core) 🗄️

From the solution folder:

```powershell
dotnet ef database update
```

## Run ▶️

```powershell
dotnet run --project IntelliAssist/IntelliAssist.csproj
```

Open the app and use the left navigation:

- Chat
- Translate
- Summarize
- Recommendation

## Notes 💡

- The movie recommendation feature reads `UserMovies` from SQL Server.
- For read-only queries, the repository uses EF Core `AsNoTracking()` for better performance.
