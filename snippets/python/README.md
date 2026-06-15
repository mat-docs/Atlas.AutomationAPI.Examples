# Python Snippets

Examples that drive ATLAS from Python using [pythonnet](https://pythonnet.github.io/).

| Folder | Description |
|--------|-------------|
| [getting-started/](getting-started/) | Connect and read parameter samples |
| [parameters/](parameters/) | Compute and write a transient parameter |

## Setup

```bash
pip install -r requirements.txt
```

The snippets locate ATLAS through the `ATLAS_DIR` environment variable, falling
back to the default install path
(`C:\Program Files\McLaren Applied Technologies\ATLAS 10`). They load the CoreCLR
runtime that ATLAS ships with, then reference the Automation API assemblies:

```python
load("coreclr", runtime_config=os.path.join(ATLAS_DIR, "MAT.Atlas.Host.runtimeconfig.json"))
clr.AddReference(os.path.join(ATLAS_DIR, "MAT.Atlas.Automation.Api.dll"))
clr.AddReference(os.path.join(ATLAS_DIR, "MAT.Atlas.Automation.Client.dll"))
```

## Run

Start ATLAS 11+ and load a session into Set 1, then:

```bash
python getting-started/01_connect_to_atlas.py
```
