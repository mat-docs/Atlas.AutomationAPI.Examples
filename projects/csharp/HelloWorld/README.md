# HelloWorld

The canonical Automation API demo, built as a Tier 2 project on top of the
shared [Atlas.Automation.Examples.Core](../Atlas.Automation.Examples.Core/)
library.

It reads `vCar:Chassis` from the loaded session, derives `vCar-50` as a
transient parameter, writes it back, and plots both on a waveform display named
**Automation Demo**.

## Prerequisites

- .NET 8 SDK
- `Atlas.AutomationAPI` NuGet package (registered users — see the repo README)
- ATLAS 11+ running, with a session containing `vCar:Chassis` loaded into Set 1

## Run

```bash
dotnet run --project projects/csharp/HelloWorld
```

The example is idempotent: it removes any previous `vCarMinus50` transient before
writing, so you can re-run it without reloading the session.

## What to look at

- [`Program.cs`](Program.cs) — the workflow, end to end
- [`AtlasConnection.cs`](../Atlas.Automation.Examples.Core/AtlasConnection.cs) — connect + locate the session
- The single-file equivalent: [snippets/csharp/parameters/compute-and-write-transient.cs](../../../snippets/csharp/parameters/compute-and-write-transient.cs)
