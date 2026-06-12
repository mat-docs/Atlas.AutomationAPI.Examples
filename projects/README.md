<img src="../images/MotionAppliedLogo.png" alt="Motion Applied" width="200">

# Projects (Tier 2)

Complete, buildable applications that show production-quality patterns, built on
a shared `Core` library so each project can focus on what it demonstrates.

## C#

All C# projects live in the `Atlas.Automation.Examples.sln` solution:

```bash
cd projects/csharp
dotnet build Atlas.Automation.Examples.sln
```

| Project | Type | Description |
|---------|------|-------------|
| [Atlas.Automation.Examples.Core](csharp/Atlas.Automation.Examples.Core/) | Library | Shared helpers: connect-with-retry, locate the first session, load-and-wait |
| [HelloWorld](csharp/HelloWorld/) | Console | The canonical demo: read vCar, derive vCar-50, plot both |

The Automation API package reference is declared once in
[csharp/Directory.Build.props](csharp/Directory.Build.props) and inherited by
every project.

## Visual Basic (COM)

| Project | Description |
|---------|-------------|
| [vba](vba/) | The same HelloWorld workflow driven from Excel VBA through the COM interface |

The COM interface must be registered before use — see
[docs/com-interop.md](../docs/com-interop.md).
