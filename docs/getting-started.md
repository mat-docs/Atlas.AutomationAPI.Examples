# Getting Started

This guide takes you from nothing to a running Automation API example.

## 1. What the Automation API is

The ATLAS Automation API lets an external application **control a running
instance of ATLAS** — load sessions, create displays and pages, add parameters,
write transient (calculated) channels, read samples, and more. It talks to ATLAS
over WCF, so **ATLAS must be running** for any example to work.

It is distributed as two assemblies that ship with ATLAS:

- `MAT.Atlas.Automation.Api.dll` — the contracts (models, enums, service interfaces)
- `MAT.Atlas.Automation.Client.dll` — the client classes you call

## 2. Prerequisites

- **ATLAS 11+** installed and running
- For C#: the **.NET 8 SDK** and the `Atlas.AutomationAPI` NuGet package
  from the [Motion Applied NuGet feed](https://github.com/mat-docs/packages)
  (registered users)
- For Python: **Python 3.8+** and `pythonnet`
- For MATLAB: **MATLAB** with .NET interop (Windows)
- For VBA: the COM interface registered — see [com-interop.md](com-interop.md)

## 3. Load a session in ATLAS

Most examples expect a session to be loaded:

1. Launch ATLAS.
2. Open the **Session Browser**.
3. Load a session that contains the parameter `vCar:Chassis` into **Set 1**.

(Alternatively, load one programmatically — see
[snippets/csharp/getting-started/03-load-session-into-set.cs](../snippets/csharp/getting-started/03-load-session-into-set.cs).)

## 4. Run your first example (C#)

```bash
dotnet new console -n TryAtlas
cd TryAtlas
dotnet add package Atlas.AutomationAPI
```

Copy the contents of
[snippets/csharp/getting-started/01-connect-to-atlas.cs](../snippets/csharp/getting-started/01-connect-to-atlas.cs)
into `Program.cs`, then:

```bash
dotnet run
```

You should see the connected ATLAS version printed, and a message pop up in
ATLAS. From there, work through the rest of the
[getting-started snippets](../snippets/csharp/getting-started/).

## 5. Where to go next

| You want to… | Go to |
|--------------|-------|
| Copy-paste a quick recipe | [snippets/](../snippets/) |
| Build a real application | [projects/](../projects/) |
| Explore data interactively | [notebooks/](../notebooks/) |
| Understand the building blocks | [CONCEPTS.md](../CONCEPTS.md) |
| Fix a problem | [troubleshooting.md](troubleshooting.md) |
