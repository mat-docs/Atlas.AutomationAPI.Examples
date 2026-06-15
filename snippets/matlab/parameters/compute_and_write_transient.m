function compute_and_write_transient()
% ATLAS Automation API Example: Compute and Write a Transient Parameter (MATLAB)
%
% Demonstrates: Read vCar, derive vCar-50, write it back as a transient
%               parameter, and plot both on a waveform display.
% Prerequisites: MATLAB with .NET support; ATLAS 11+ running with a session loaded
%                into Set 1 that contains 'vCar:Chassis'
% Output:        An "Automation Demo" waveform showing vCar and vCar-50
% Related:       read_parameter_samples.m
%
% Idempotent: the previous transient is removed first, so re-running is safe.

atlasDir = getenv('ATLAS_DIR');
if isempty(atlasDir)
    atlasDir = 'C:\Program Files\McLaren Applied Technologies\ATLAS 10';
end

NET.addAssembly(fullfile(atlasDir, 'MAT.Atlas.Automation.Api.dll'));
NET.addAssembly(fullfile(atlasDir, 'MAT.Atlas.Automation.Client.dll'));
import MAT.Atlas.Automation.Client.Services.*;

sourceParameter = 'vCar:Chassis';
transientId = 'vCarMinus50:Demo';
displayName = 'Automation Demo';
offset = 50.0;

workbook = WorkbookServiceClient;
sets = SetServiceClient;
sessionService = SessionServiceClient;
pda = ParameterDataAccessServiceClient;
displays = DisplayServiceClient;

% --- 1. Find the loaded session ---
setList = workbook.GetSets();
if (setList.Length == 0)
    disp('No sets configured.');
    return;
end
sessions = sets.GetCompositeSessions(setList(1).Id);
if (sessions.Length == 0)
    disp('No sessions loaded.');
    return;
end
sessionId = sessions(1).Id;

% --- 2. Read the source parameter ---
parameter = sessionService.GetSessionParameter(sessionId, sourceParameter);
if isempty(parameter)
    fprintf('%s not found.\n', sourceParameter);
    return;
end

timebase = sessionService.GetSessionTimeBase(sessionId);
estimate = pda.GetSamplesCountEstimate(sessionId, parameter.Identifier, timebase.StartTime, timebase.EndTime);
pda.Goto(sessionId, parameter.Identifier, timebase.StartTime);
samples = pda.GetNextSamples(sessionId, parameter.Identifier, estimate);

timestamps = int64(samples.Time);
values = double(samples.Data) - offset;     % the derived channel
fprintf('Read %d samples of %s.\n', samples.SampleCount, char(parameter.Identifier));

% --- 3. (Re)create the transient parameter and write the derived data ---
try
    sessionService.RemoveTransientParameter(sessionId, transientId);
catch
    % not present yet
end

groups = NET.createArray('System.String', 1);
groups(1) = 'Demo';
transient = sessionService.AddTransientParameter(sessionId, transientId, 'vCarMinus50', ...
    'vCar offset by 50 km/h', groups, 0, 800);
sessionService.AddTimeDataToTransientParameter(sessionId, transient.Identifier, timestamps, values);
fprintf('Wrote %d samples to %s.\n', numel(values), char(transient.Identifier));

% --- 4. Show both channels on a waveform display ---
displayId = [];
displayList = workbook.GetDisplays();
for i = 1:displayList.Length
    if strcmp(char(displayList(i).Name), displayName)
        displayId = displayList(i).Id;
    end
end
if isempty(displayId)
    display = workbook.CreateDisplay('Waveform', displayName);
    displayId = display.Id;
end

onDisplay = displays.GetDisplayParameters(displayId);
wanted = {char(parameter.Identifier), char(transient.Identifier)};
for w = 1:numel(wanted)
    present = false;
    for i = 1:onDisplay.Length
        if strcmp(char(onDisplay(i).Identifier), wanted{w})
            present = true;
        end
    end
    if ~present
        displays.AddDisplayParameter(displayId, wanted{w});
    end
end

fprintf('Display %s now shows %s and %s.\n', displayName, wanted{1}, wanted{2});

% Clean up
workbook.Dispose();
sets.Dispose();
sessionService.Dispose();
pda.Dispose();
displays.Dispose();

end
