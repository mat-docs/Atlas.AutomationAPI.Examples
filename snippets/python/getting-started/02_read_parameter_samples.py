# ─────────────────────────────────────────────────────────────
# ATLAS Automation API Example: Read Parameter Samples (Python)
# ─────────────────────────────────────────────────────────────
#
# Demonstrates: Reading time-series samples for a parameter from a loaded session
# Prerequisites: Python 3.8+, pythonnet; ATLAS 11+ running with a session loaded
#                into Set 1 that contains 'vCar:Chassis'
# Output:        Sample count and the first few timestamp/value pairs
# Related:       01_connect_to_atlas.py, ../parameters/compute_and_write_transient.py
# ─────────────────────────────────────────────────────────────

import os

from pythonnet import load

ATLAS_DIR = os.environ.get(
    "ATLAS_DIR", r"C:\Program Files\McLaren Applied Technologies\ATLAS 10"
)
load("coreclr", runtime_config=os.path.join(ATLAS_DIR, "MAT.Atlas.Host.runtimeconfig.json"))

import clr  # noqa: E402

clr.AddReference(os.path.join(ATLAS_DIR, "MAT.Atlas.Automation.Api.dll"))
clr.AddReference(os.path.join(ATLAS_DIR, "MAT.Atlas.Automation.Client.dll"))

from MAT.Atlas.Automation.Api.Enums import DataStatusType  # noqa: E402
from MAT.Atlas.Automation.Client.Services import (  # noqa: E402
    ParameterDataAccessServiceClient,
    SessionServiceClient,
    SetServiceClient,
    WorkbookServiceClient,
)

PARAMETER_NAME = "vCar:Chassis"

workbook = WorkbookServiceClient()
sets = SetServiceClient()
session_service = SessionServiceClient()
pda = ParameterDataAccessServiceClient()

try:
    set_list = workbook.GetSets()
    if len(set_list) == 0:
        print("No sets configured.")
        raise SystemExit(1)

    sessions = sets.GetCompositeSessions(set_list[0].Id)
    if len(sessions) == 0:
        print("No sessions loaded.")
        raise SystemExit(1)

    session_id = sessions[0].Id

    parameter = session_service.GetSessionParameter(session_id, PARAMETER_NAME)
    if parameter is None:
        print(f"Parameter '{PARAMETER_NAME}' not found in this session.")
        raise SystemExit(1)

    timebase = session_service.GetSessionTimeBase(session_id)
    estimate = pda.GetSamplesCountEstimate(
        session_id, parameter.Identifier, timebase.StartTime, timebase.EndTime
    )

    pda.Goto(session_id, parameter.Identifier, timebase.StartTime)
    samples = pda.GetNextSamples(session_id, parameter.Identifier, estimate)

    print(f"{parameter.Identifier} ({parameter.Units})")
    print(f"  estimated samples: {estimate}")
    print(f"  returned samples:  {samples.SampleCount}")
    print("  first values:")
    for i in range(min(5, samples.SampleCount)):
        if samples.DataStatus[i].HasFlag(DataStatusType.Sample):
            print(f"    t={samples.Time[i]} ns  v={samples.Data[i]:.2f}")
finally:
    for client in (workbook, sets, session_service, pda):
        client.Dispose()
