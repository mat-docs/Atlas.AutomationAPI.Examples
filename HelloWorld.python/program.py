# Import python modules
import os
from time import sleep
from pythonnet import load

load(
    "coreclr",
    runtime_config=r"C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Host.runtimeconfig.json",
)

import clr

# Import required system modules
from System.Collections.Generic import *
from System.Collections.ObjectModel import *
from System import *
from System.IO import Path
from System import AppDomain

# Make the WCF API .Net DLLs available
AUTOMATION_API_DLL_PATH = r"C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.dll"
AUTOMATION_CLIENT_DLL_PATH = r"C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.dll"

clr.AddReference("System.Collections")
clr.AddReference("System.Core")
clr.AddReference("System.IO")

if not os.path.isfile(AUTOMATION_API_DLL_PATH):
    raise Exception(f"Couldn't find Automation API DLL at {AUTOMATION_API_DLL_PATH}.")

clr.AddReference(AUTOMATION_API_DLL_PATH)

if not os.path.isfile(AUTOMATION_CLIENT_DLL_PATH):
    raise Exception(
        f"Couldn't find Automation Client DLL at {AUTOMATION_CLIENT_DLL_PATH}."
    )

clr.AddReference(AUTOMATION_CLIENT_DLL_PATH)

# Import Atlas modules
from MAT.Atlas.Automation.Api.Enums import *
from MAT.Atlas.Automation.Api.Models import *
from MAT.Atlas.Automation.Client.Services import *

# Get services
workbook_service_client = WorkbookServiceClient()
set_service_client = SetServiceClient()
session_service_client = SessionServiceClient()
parameter_data_access_service_client = ParameterDataAccessServiceClient()
display_service_client = DisplayServiceClient()
application_service_client = ApplicationServiceClient()

# Wait for ATLAS connection
print("Connecting to ATLAS")
connecting = True
i = 0
while connecting:
    try:
        application_service_client.Connect(
            Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)
        )
        connecting = False
    except Exception:
        print(f"Unable to connect due to {Exception}. Trying again.")
        if i >= 9:
            connecting = False
        else:
            i += 1
            sleep(1)
if i >= 9:
    raise RuntimeError("Failed to connect to ATLAS.")

# Get first set
sets_list = workbook_service_client.GetSets()
if len(sets_list) == 0:
    print("No Sets Configured")

set_id = sets_list[0].Id

# Get first session
sessions_list = set_service_client.GetCompositeSessions(set_id)
if len(sessions_list) == 0:
    print("No Sessions Loaded")

session_id = sessions_list[0].Id

# Get vCar parameter
parameter = session_service_client.GetSessionParameter(session_id, "vCar:Chassis")

# Get current time base
timebase = session_service_client.GetSessionTimeBase(session_id)

# Get estimated sample count
sampleCount = parameter_data_access_service_client.GetSamplesCountEstimate(
    session_id, parameter.Identifier, timebase.StartTime, timebase.EndTime
)

# Goto start of time base
parameter_data_access_service_client.Goto(
    session_id, parameter.Identifier, timebase.StartTime
)

# Get vCar data
parameter_values = parameter_data_access_service_client.GetNextSamples(
    session_id, parameter.Identifier, sampleCount
)

# Calculate vCar * 2
data = list(parameter_values.Data)
data2 = [element * 2 for element in data]
timestamps2 = list(parameter_values.Time)

try:
    # Create transient parameter
    groups = [""]
    trasient_parameter = session_service_client.AddTransientParameter(
        session_id, "vCar2 Demo", "vCar2 Demo", "vCar2 Demo", groups, 0, 800)
except Exception as e:
    print(str(e))
    if "ParameterNotAdded" in str(e):
        print("L119 if statement worked")
        trasient_parameter = session_service_client.GetSessionParameter(
            session_id, "vCar2 Demo")
        session_service_client.RemoveDataFromTransientParameter(
            session_id, trasient_parameter.Identifier)
    else:
        print(str(e))

# Write vCar * 2
session_service_client.AddTimeDataToTransientParameter(
    session_id, trasient_parameter.Identifier, timestamps2, data2)

# Find or create waveform display
displays = workbook_service_client.GetDisplays()

display_id = None
for count, display in enumerate(displays):
    if display.Name == "DemoWaveform":
        display_id = count.Id

if display_id == None:
    display = workbook_service_client.CreateDisplay("Waveform", "DemoWaveform")
    display_id = display.Id

# Find or add display parameters
display_parameters = display_service_client.GetDisplayParameters(display_id)

vCar_display_parameter_present = False
vCar2_display_parameter_present = False

# Displays parameters if not already in waveform
for count, display_parameter in enumerate(display_parameters):
    if display_parameter.Identifier == parameter.Identifier:
        vCar_display_parameter_present = True
    if display_parameter.Identifier == trasient_parameter.Identifier:
        vCar2_display_parameter_present = True

if not vCar_display_parameter_present:
    display_service_client.AddDisplayParameter(display_id, parameter.Identifier)

if not vCar2_display_parameter_present:
    display_service_client.AddDisplayParameter(
        display_id, trasient_parameter.Identifier)

# Cleanup
workbook_service_client.Dispose()
set_service_client.Dispose()
session_service_client.Dispose()
parameter_data_access_service_client.Dispose()
display_service_client.Dispose()
