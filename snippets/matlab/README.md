# MATLAB Snippets

Examples that drive ATLAS from MATLAB using .NET interop.

| Folder | Description |
|--------|-------------|
| [getting-started/](getting-started/) | Connect and read parameter samples |
| [parameters/](parameters/) | Compute and write a transient parameter |

## Setup

MATLAB includes .NET interop out of the box on Windows. Each script locates
ATLAS through the `ATLAS_DIR` environment variable, falling back to the default
install path, then loads the assemblies:

```matlab
NET.addAssembly(fullfile(atlasDir, 'MAT.Atlas.Automation.Api.dll'));
NET.addAssembly(fullfile(atlasDir, 'MAT.Atlas.Automation.Client.dll'));
```

## Run

Start ATLAS 11+ and load a session into Set 1, then call the function from the
MATLAB command window (the file name matches the function name):

```matlab
connect_to_atlas
read_parameter_samples
```
