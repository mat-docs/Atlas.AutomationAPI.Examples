<img src="images/MotionAppliedLogo.png" alt="Motion Applied" width="280" align="right" /><br><br><br>

# ATLAS Automation API Examples

Examples for the **ATLAS Automation API** — a .NET API that lets an external
application control a running instance of ATLAS: load sessions, build pages and
displays, create calculated (transient) parameters, read samples, and more.

ATLAS is used across **motorsport, aerospace, automotive, energy and defence**
for recording and analysing high-frequency sensor data.

> **ATLAS must be running.** Unlike a file-based data library, the Automation API
> drives the ATLAS application itself over WCF. Start ATLAS 11+ before running any
> example.

## Quick Start

```bash
# 1. Create a console project
dotnet new console -n TryAtlas
cd TryAtlas

# 2. Add the Automation API package (registered users — see Prerequisites)
dotnet add package Atlas.AutomationAPI

# 3. Copy a snippet into Program.cs and run (with ATLAS open)
#    e.g. snippets/csharp/getting-started/01-connect-to-atlas.cs
dotnet run
```

## I want to...

| Task | Language | Example |
|------|----------|---------|
| Connect to ATLAS and read its version | C# | [`01-connect-to-atlas.cs`](snippets/csharp/getting-started/01-connect-to-atlas.cs) |
| List the sets and loaded sessions | C# | [`02-list-sets-and-sessions.cs`](snippets/csharp/getting-started/02-list-sets-and-sessions.cs) |
| Load a session into a set | C# | [`03-load-session-into-set.cs`](snippets/csharp/getting-started/03-load-session-into-set.cs) |
| Read samples for a parameter | C# | [`04-read-parameter-samples.cs`](snippets/csharp/getting-started/04-read-parameter-samples.cs) |
| Create a waveform display | C# | [`create-waveform-display.cs`](snippets/csharp/displays/create-waveform-display.cs) |
| Add parameters to a display | C# | [`add-parameters-to-display.cs`](snippets/csharp/displays/add-parameters-to-display.cs) |
| Create a page with displays | C# | [`create-page-with-displays.cs`](snippets/csharp/displays/create-page-with-displays.cs) |
| Create a transient parameter | C# | [`add-transient-parameter.cs`](snippets/csharp/parameters/add-transient-parameter.cs) |
| Derive a channel and plot it | C# | [`compute-and-write-transient.cs`](snippets/csharp/parameters/compute-and-write-transient.cs) |
| Compute parameter statistics | C# | [`parameter-statistics.cs`](snippets/csharp/parameters/parameter-statistics.cs) |
| Read laps and events | C# | [`laps-and-events.cs`](snippets/csharp/sessions/laps-and-events.cs) |
| Read/write session metadata | C# | [`session-details.cs`](snippets/csharp/sessions/session-details.cs) |
| Connect and read samples | Python | [`01_connect_to_atlas.py`](snippets/python/getting-started/01_connect_to_atlas.py) |
| Connect and read samples | MATLAB | [`connect_to_atlas.m`](snippets/matlab/getting-started/connect_to_atlas.m) |
| Drive ATLAS from Excel | VBA | [`projects/vba`](projects/vba/) |

See [snippets/README.md](snippets/README.md) for the complete index.

## Repository Map

```
Atlas.AutomationAPI.Examples/
├── snippets/    Tier 1 — single-file, copy-paste examples (C#, Python, MATLAB)
├── projects/    Tier 2 — complete, buildable apps + a shared Core library, and the VBA/COM example
├── notebooks/   Tier 3 — interactive Jupyter notebooks (pythonnet)
├── docs/        Guides: getting started, connection strings, COM interop, troubleshooting
├── scripts/     CI helpers (snippet compile-checking)
└── images/      Logos
```

### Tiers explained

| Tier | What | Who it's for |
|------|------|-------------|
| **Snippets** | Single-file, self-contained examples. Copy, paste, run. | Anyone evaluating the API or wanting a quick recipe. |
| **Projects** | Buildable solutions with a shared `Core` library. | Teams building real applications. |
| **Notebooks** | Interactive notebooks with narrative and plots. | Engineers exploring session data live. |

## Language Support

| Language | Snippets | Projects | Notebooks |
|----------|:--------:|:--------:|:---------:|
| C# (.NET 8) | Yes | Yes | — |
| Python (pythonnet) | Yes | — | Yes |
| MATLAB (.NET interop) | Yes | — | — |
| Visual Basic (COM) | — | Yes | — |

## Prerequisites

- **ATLAS 11+** running on the machine
- **.NET 8 SDK** — [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **`Atlas.AutomationAPI`** NuGet package from the
  [Motion Applied NuGet feed](https://github.com/mat-docs/packages) (registered users)
- Python 3.8+ / MATLAB / Excel as needed for those languages

See [docs/getting-started.md](docs/getting-started.md) for full setup, and
[CONCEPTS.md](CONCEPTS.md) for the object model.

## A note on COM / VBA

Using the API from **C#, Python or MATLAB needs no registration**. Only the
COM-based VBA example requires the libraries to be registered first — see
[docs/com-interop.md](docs/com-interop.md).

## License

Provided by Motion Applied Ltd. as sample code for the ATLAS Automation API.
