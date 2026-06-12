<img src="../images/MotionAppliedLogo.png" alt="Motion Applied" width="200">

# Snippets (Tier 1)

Single-file, self-contained examples you can copy-paste and run against a live ATLAS.

Each snippet:
- Is fully self-contained, with all imports and a runnable entry point
- Talks to a **running ATLAS instance** over the Automation API (WCF)
- Includes a header comment explaining what it does, its prerequisites and output
- Is idempotent where possible (safe to re-run without restarting ATLAS)

> **All snippets need ATLAS running.** Unlike a file-based data library, the
> Automation API controls the ATLAS application itself, so start ATLAS 11+ and
> (for most snippets) load a session into Set 1 first. See
> [docs/getting-started.md](../docs/getting-started.md).

## Languages

- [C#](csharp/) — full coverage of the common workflows
- [Python](python/) — key scenarios via [pythonnet](https://pythonnet.github.io/)
- [MATLAB](matlab/) — key scenarios via .NET interop

(For Visual Basic / Excel via the COM interface, see [projects/vba](../projects/vba/).)

## Complete Index

### C#

| File | Description |
|------|-------------|
| [getting-started/01-connect-to-atlas.cs](csharp/getting-started/01-connect-to-atlas.cs) | Connect to ATLAS and read its version |
| [getting-started/02-list-sets-and-sessions.cs](csharp/getting-started/02-list-sets-and-sessions.cs) | Enumerate sets and the sessions loaded into them |
| [getting-started/03-load-session-into-set.cs](csharp/getting-started/03-load-session-into-set.cs) | Load a SQL Race session into a set and wait for it |
| [getting-started/04-read-parameter-samples.cs](csharp/getting-started/04-read-parameter-samples.cs) | Read time-series samples for a parameter |
| [displays/create-waveform-display.cs](csharp/displays/create-waveform-display.cs) | Create (or reuse) a waveform display |
| [displays/add-parameters-to-display.cs](csharp/displays/add-parameters-to-display.cs) | Add parameters to a display without duplicates |
| [displays/create-page-with-displays.cs](csharp/displays/create-page-with-displays.cs) | Create a page, associate a set, add a display |
| [parameters/add-transient-parameter.cs](csharp/parameters/add-transient-parameter.cs) | Create an in-memory transient parameter |
| [parameters/compute-and-write-transient.cs](csharp/parameters/compute-and-write-transient.cs) | Derive a channel from vCar and plot both |
| [parameters/parameter-statistics.cs](csharp/parameters/parameter-statistics.cs) | Min/max/mean/std-dev over the time base |
| [sessions/laps-and-events.cs](csharp/sessions/laps-and-events.cs) | List laps, find the fastest, read events |
| [sessions/session-details.cs](csharp/sessions/session-details.cs) | Read and write session metadata |

### Python

| File | Description |
|------|-------------|
| [getting-started/01_connect_to_atlas.py](python/getting-started/01_connect_to_atlas.py) | Load the API via pythonnet and connect |
| [getting-started/02_read_parameter_samples.py](python/getting-started/02_read_parameter_samples.py) | Read time-series samples for a parameter |
| [parameters/compute_and_write_transient.py](python/parameters/compute_and_write_transient.py) | Derive a channel from vCar and plot both |

### MATLAB

| File | Description |
|------|-------------|
| [getting-started/connect_to_atlas.m](matlab/getting-started/connect_to_atlas.m) | Load the API via .NET interop and connect |
| [getting-started/read_parameter_samples.m](matlab/getting-started/read_parameter_samples.m) | Read time-series samples for a parameter |
| [parameters/compute_and_write_transient.m](matlab/parameters/compute_and_write_transient.m) | Derive a channel from vCar and plot both |
