# ─────────────────────────────────────────────────────────────
# ATLAS Automation API Example: Compute and Write a Transient Parameter (Python)
# ─────────────────────────────────────────────────────────────
#
# Demonstrates: The end-to-end workflow - read vCar, derive vCar-50, write it back
#               as a transient parameter, and plot both on a waveform display.
# Prerequisites: Python 3.8+, pythonnet; ATLAS 11+ running with a session loaded
#                into Set 1 that contains 'vCar:Chassis'
# Output:        An "Automation Demo" waveform showing vCar and vCar-50
# Related:       02_read_parameter_samples.py
#
# Idempotent: re-run as often as you like - the previous transient is removed first.
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
    DisplayServiceClient,
    ParameterDataAccessServiceClient,
    SessionServiceClient,
    SetServiceClient,
    WorkbookServiceClient,
)

SOURCE_PARAMETER = "vCar:Chassis"
TRANSIENT_ID = "vCarMinus50:Demo"
DISPLAY_NAME = "Automation Demo"
OFFSET = 50.0

workbook = WorkbookServiceClient()
sets = SetServiceClient()
session_service = SessionServiceClient()
pda = ParameterDataAccessServiceClient()
displays = DisplayServiceClient()

try:
    # --- 1. Find the loaded session ---
    set_list = workbook.GetSets()
    if len(set_list) == 0:
        print("No sets configured.")
        raise SystemExit(1)

    session_list = sets.GetCompositeSessions(set_list[0].Id)
    if len(session_list) == 0:
        print("No sessions loaded.")
        raise SystemExit(1)

    session_id = session_list[0].Id

    # --- 2. Read the source parameter ---
    parameter = session_service.GetSessionParameter(session_id, SOURCE_PARAMETER)
    if parameter is None:
        print(f"'{SOURCE_PARAMETER}' not found.")
        raise SystemExit(1)

    timebase = session_service.GetSessionTimeBase(session_id)
    estimate = pda.GetSamplesCountEstimate(
        session_id, parameter.Identifier, timebase.StartTime, timebase.EndTime
    )
    pda.Goto(session_id, parameter.Identifier, timebase.StartTime)
    samples = pda.GetNextSamples(session_id, parameter.Identifier, estimate)

    timestamps = []
    values = []
    for i in range(samples.SampleCount):
        if samples.DataStatus[i].HasFlag(DataStatusType.Sample):
            timestamps.append(samples.Time[i])
            values.append(samples.Data[i] - OFFSET)
    print(f"Read {len(values)} samples of {parameter.Identifier}.")

    # --- 3. (Re)create the transient parameter and write the derived data ---
    try:
        session_service.RemoveTransientParameter(session_id, TRANSIENT_ID)
    except Exception:  # noqa: BLE001  (not present yet)
        pass

    transient = session_service.AddTransientParameter(
        session_id, TRANSIENT_ID, "vCarMinus50", "vCar offset by 50 km/h", [""], 0, 800
    )
    session_service.AddTimeDataToTransientParameter(
        session_id, transient.Identifier, timestamps, values
    )
    print(f"Wrote {len(values)} samples to {transient.Identifier}.")

    # --- 4. Show both channels on a waveform display ---
    display_id = None
    for display in workbook.GetDisplays():
        if display.Name == DISPLAY_NAME:
            display_id = display.Id
            break
    if display_id is None:
        display_id = workbook.CreateDisplay("Waveform", DISPLAY_NAME).Id

    on_display = {p.Identifier for p in displays.GetDisplayParameters(display_id)}
    for identifier in (parameter.Identifier, transient.Identifier):
        if identifier not in on_display:
            displays.AddDisplayParameter(display_id, identifier)

    print(f"Display '{DISPLAY_NAME}' now shows {parameter.Identifier} and {transient.Identifier}.")
finally:
    for client in (workbook, sets, session_service, pda, displays):
        client.Dispose()
