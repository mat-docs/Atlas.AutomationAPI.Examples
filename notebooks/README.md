<img src="../images/MotionAppliedLogo.png" alt="Motion Applied" width="200">

# Notebooks (Tier 3)

Interactive Jupyter notebooks that drive a running ATLAS instance from Python
(via [pythonnet](https://pythonnet.github.io/)) and visualise the results inline
with matplotlib.

| Notebook | Description |
|----------|-------------|
| [01-getting-started.ipynb](01-getting-started.ipynb) | Connect to ATLAS, list sets/sessions, read `vCar`, and plot it |
| [02-transient-parameter.ipynb](02-transient-parameter.ipynb) | Derive `vCar-50`, write it back as a transient parameter, and show both on a display |

## Setup

```bash
pip install -r requirements.txt
jupyter lab
```

The notebooks locate ATLAS through the `ATLAS_DIR` environment variable, falling
back to the default install path
(`C:\Program Files\McLaren Applied Technologies\ATLAS 10`).

## Before you run

1. Start **ATLAS 11+**.
2. Load a session containing `vCar:Chassis` into **Set 1**.
3. Run the notebook cells top to bottom.

> Because the Automation API controls a live ATLAS, these notebooks need ATLAS
> running and a session loaded — they cannot run headless in CI.
