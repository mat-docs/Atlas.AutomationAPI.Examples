function WCFAPIExample()
% Make the WCF API .Net DLLs available 
NET.addAssembly('C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.dll')
NET.addAssembly('C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.dll')

import MAT.Atlas.Automation.Api.Enums.*;
import MAT.Atlas.Automation.Api.Models.*;
import MAT.Atlas.Automation.Client.Services.*;

% Get services
workbookServiceClient = WorkbookServiceClient
setServiceClient = SetServiceClient
sessionServiceClient = SessionServiceClient
parameterDataAccessServiceClient = ParameterDataAccessServiceClient
displayServiceClient = DisplayServiceClient

% Get first set
setsList = workbookServiceClient.GetSets();
if (setsList.Length == 0)
    disp('No Sets Configured');
    return;
end

setId = setsList(1).Id;

% Get first session
sessionsList = setServiceClient.GetCompositeSessions(setId);
if (sessionsList.Length == 0)
    disp('No Sessions Loaded');
    return;
end

sessionId = sessionsList(1).Id;

% Get vCar parameter
parameter = sessionServiceClient.GetSessionParameter(sessionId, 'vCar:Chassis');

% Get current time base
timebase = sessionServiceClient.GetSessionTimeBase(sessionId);

% Get estimated sample count
sampleCount = parameterDataAccessServiceClient.GetSamplesCountEstimate(sessionId, parameter.Identifier, timebase.StartTime, timebase.EndTime);

% Goto start of time base
parameterDataAccessServiceClient.Goto(sessionId, parameter.Identifier, timebase.StartTime);

% Get vCar data
parameterValues = parameterDataAccessServiceClient.GetNextSamples(sessionId, parameter.Identifier, sampleCount);

% Calculate vCar * 2
data = double(parameterValues.Data);
data2 = data * 2.0;
timestamps2 = int64(parameterValues.Time);

try

% Create transient parameter
groups = NET.createArray('System.String', 0);
transientParameter = sessionServiceClient.AddTransientParameter(sessionId, 'vCar2 Demo', 'vCar2 Demo', 'vCar2 Demo', groups, 0, 800);

catch e
    if (isa(e, 'NET.NetException') && e.ExceptionObject.Message == 'ParameterNotAdded')
        transientParameter = sessionServiceClient.GetSessionParameter(sessionId, 'vCar2 Demo');
        sessionServiceClient.RemoveDataFromTransientParameter(sessionId, transientParameter.Identifier);
    else
        disp(e.message);
        return;
    end
end

% Write vCar * 2
sessionServiceClient.AddTimeDataToTransientParameter(sessionId, transientParameter.Identifier, timestamps2, data2);

% Find or create waveform display
displays = workbookServiceClient.GetDisplays();

for i = 1:displays.Length
    if (displays(i).Name == 'DemoWaveform')
        displayId = displays(i).Id;
    end
end

if (exist('displayId', 'var') == 0)
    display = workbookServiceClient.CreateDisplay('Waveform', 'DemoWaveform');
    displayId = display.Id;
end

% Find or add display parameters
displayParameters = displayServiceClient.GetDisplayParameters(displayId);

vCarDisplayParameterPresent = false;
vCar2DisplayParameterPresent = false;

for i = 1:displayParameters.Length
    if (displayParameters(i).Identifier == parameter.Identifier)
        vCarDisplayParameterPresent = true
    end
    if (displayParameters(i).Identifier == transientParameter.Identifier)
        vCar2DisplayParameterPresent = true;
    end
end

if (~vCarDisplayParameterPresent)
    displayServiceClient.AddDisplayParameter(displayId, parameter.Identifier);
end

if (~vCar2DisplayParameterPresent)
    displayServiceClient.AddDisplayParameter(displayId, transientParameter.Identifier);
end

% Cleanup
workbookServiceClient.Dispose();
setServiceClient.Dispose();
sessionServiceClient.Dispose();
parameterDataAccessServiceClient.Dispose();
displayServiceClient.Dispose();

end
