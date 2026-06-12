function read_parameter_samples()
% ATLAS Automation API Example: Read Parameter Samples (MATLAB)
%
% Demonstrates: Reading time-series samples for a parameter from a loaded session
% Prerequisites: MATLAB with .NET support; ATLAS 11+ running with a session loaded
%                into Set 1 that contains 'vCar:Chassis'
% Output:        Sample count and the first few timestamp/value pairs
% Related:       connect_to_atlas.m, ../parameters/compute_and_write_transient.m

atlasDir = getenv('ATLAS_DIR');
if isempty(atlasDir)
    atlasDir = 'C:\Program Files\McLaren Applied Technologies\ATLAS 10';
end

NET.addAssembly(fullfile(atlasDir, 'MAT.Atlas.Automation.Api.dll'));
NET.addAssembly(fullfile(atlasDir, 'MAT.Atlas.Automation.Client.dll'));
import MAT.Atlas.Automation.Client.Services.*;

parameterName = 'vCar:Chassis';

workbook = WorkbookServiceClient;
sets = SetServiceClient;
sessionService = SessionServiceClient;
pda = ParameterDataAccessServiceClient;

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

parameter = sessionService.GetSessionParameter(sessionId, parameterName);
if isempty(parameter)
    fprintf('Parameter %s not found in this session.\n', parameterName);
    return;
end

timebase = sessionService.GetSessionTimeBase(sessionId);
estimate = pda.GetSamplesCountEstimate(sessionId, parameter.Identifier, timebase.StartTime, timebase.EndTime);
pda.Goto(sessionId, parameter.Identifier, timebase.StartTime);
samples = pda.GetNextSamples(sessionId, parameter.Identifier, estimate);

time = int64(samples.Time);
data = double(samples.Data);

fprintf('%s (%s)\n', char(parameter.Identifier), char(parameter.Units));
fprintf('  estimated samples: %d\n', estimate);
fprintf('  returned samples:  %d\n', samples.SampleCount);
fprintf('  first values:\n');
for i = 1:min(5, samples.SampleCount)
    fprintf('    t=%d ns  v=%.2f\n', time(i), data(i));
end

% Clean up
workbook.Dispose();
sets.Dispose();
sessionService.Dispose();
pda.Dispose();

end
