# ─────────────────────────────────────────────────────────────
# ATLAS Automation API Example: Connect to ATLAS (Python)
# ─────────────────────────────────────────────────────────────
#
# Demonstrates: Loading the Automation API via pythonnet and connecting to a
#               running ATLAS instance
# Prerequisites: Python 3.8+, pythonnet (pip install -r ../requirements.txt),
#                ATLAS 11+ running on this machine
# Output:        The connected ATLAS version, or guidance if ATLAS is not running
# Related:       02_read_parameter_samples.py
# ─────────────────────────────────────────────────────────────

import os
from time import sleep

from pythonnet import load

# --- Locate ATLAS and start the CoreCLR runtime it ships with ---
ATLAS_DIR = os.environ.get(
    "ATLAS_DIR", r"C:\Program Files\McLaren Applied Technologies\ATLAS 10"
)
load("coreclr", runtime_config=os.path.join(ATLAS_DIR, "MAT.Atlas.Host.runtimeconfig.json"))

import clr  # noqa: E402  (must come after load())

clr.AddReference(os.path.join(ATLAS_DIR, "MAT.Atlas.Automation.Api.dll"))
clr.AddReference(os.path.join(ATLAS_DIR, "MAT.Atlas.Automation.Client.dll"))

from MAT.Atlas.Automation.Client.Services import ApplicationServiceClient  # noqa: E402

CLIENT_NAME = "AutomationApiExample"


def connect(client, attempts=10):
    """Connect to ATLAS, retrying while it finishes starting up."""
    for attempt in range(attempts):
        try:
            client.Connect(CLIENT_NAME)
            return True
        except Exception as ex:  # noqa: BLE001
            print(f"  attempt {attempt + 1}/{attempts}: {ex}")
            sleep(1)
    return False


application = ApplicationServiceClient(CLIENT_NAME)
try:
    print("Connecting to ATLAS...")
    if not connect(application):
        print("Could not connect. Tip: start ATLAS 11+ before running this example.")
        raise SystemExit(1)

    print(f"Connected to ATLAS {application.GetVersion()} as '{CLIENT_NAME}'.")
    application.ShowMessage(f"Hello from {CLIENT_NAME}!")
finally:
    application.Dispose()
