# C# Snippets

| Folder | Description |
|--------|-------------|
| [getting-started/](getting-started/) | Connect, list sets/sessions, load a session, read samples |
| [displays/](displays/) | Create displays and pages, add parameters |
| [parameters/](parameters/) | Transient parameters and statistics |
| [sessions/](sessions/) | Laps, events and session metadata |

## How to run a snippet

Each `.cs` file is self-contained C# with top-level statements. Drop one into a
console project that references the Automation API and run it:

```bash
dotnet new console -n TryAtlas
cd TryAtlas
# Reference the Automation API NuGet package (registered users):
dotnet add package Atlas.AutomationAPI
# Copy a snippet's contents into Program.cs, then:
dotnet run
```

Start ATLAS 11+ and load a session into Set 1 before running (see each snippet's
header for its exact prerequisites).

## Prerequisites

- .NET 8 SDK
- `Atlas.AutomationAPI` NuGet package from the [Motion Applied NuGet feed](https://github.com/mat-docs/packages)
- A running ATLAS 11+ instance

See [docs/getting-started.md](../../docs/getting-started.md) for setup details.

## Verifying snippets

All C# snippets are compile-checked in CI against the installed ATLAS assemblies:

```powershell
pwsh scripts/verify-snippets.ps1
```
