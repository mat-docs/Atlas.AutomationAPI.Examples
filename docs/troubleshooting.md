# Troubleshooting

## "Could not connect to ATLAS" / connection times out

- **ATLAS isn't running.** The Automation API controls a *running* ATLAS — start
  ATLAS 11+ first.
- **ATLAS is still starting up.** The connect helpers retry for ~10 seconds; if
  ATLAS is slow to start, run the example again once it's ready.
- **Version mismatch.** The example must use the automation DLLs that ship with
  the ATLAS build you are running. For C# this is the `Atlas.AutomationAPI` NuGet
  package version that matches your ATLAS; for Python/MATLAB it's the DLLs in your
  ATLAS install folder (`ATLAS_DIR`).

## "No sets configured"

The active workbook has no sets. Create one in ATLAS, or open the Session
Browser and load a session (which creates Set 1).

## "No sessions loaded"

A set exists but is empty. Load a session into it via the Session Browser, or
programmatically — see
[03-load-session-into-set.cs](../snippets/csharp/getting-started/03-load-session-into-set.cs).

## Parameter not found

- Check the **fully-qualified identifier** (`Name:Group`, e.g. `vCar:Chassis`),
  not just the display name.
- Confirm the parameter exists in the loaded session (it varies by session).

## "ParameterNotAdded" when adding a transient parameter

A transient parameter with that identifier already exists in the session. Either:

- Remove it first with `RemoveTransientParameter` (the C#, Python and MATLAB
  examples do this automatically so they are safe to re-run), or
- Reload the session before running again (required for the VBA example).

## Python: `clr.AddReference` fails / DLL not found

- Set `ATLAS_DIR` to your ATLAS install folder if it isn't the default
  (`C:\Program Files\McLaren Applied Technologies\ATLAS 10`).
- Make sure `load("coreclr", ...)` runs **before** `import clr`.
- Use a Python build whose bitness matches ATLAS (64-bit).

## MATLAB: `NET.addAssembly` fails

- Confirm MATLAB is using the .NET CLR (`NET.isNETSupported` should be true on
  Windows).
- Point `ATLAS_DIR` at the correct install folder.

## VBA: types like `WorkbookServiceClient` aren't recognised

The COM interface isn't registered, or an old registration is in the way. See
[com-interop.md](com-interop.md) — unregister any previous version, then register
the current one.
