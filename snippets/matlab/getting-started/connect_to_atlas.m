function connect_to_atlas()
% ATLAS Automation API Example: Connect to ATLAS (MATLAB)
%
% Demonstrates: Loading the Automation API via .NET interop and connecting to
%               a running ATLAS instance
% Prerequisites: MATLAB with .NET support; ATLAS 11+ running on this machine
% Output:        The connected ATLAS version, or guidance if ATLAS is not running
% Related:       read_parameter_samples.m

atlasDir = getenv('ATLAS_DIR');
if isempty(atlasDir)
    atlasDir = 'C:\Program Files\McLaren Applied Technologies\ATLAS 10';
end

NET.addAssembly(fullfile(atlasDir, 'MAT.Atlas.Automation.Api.dll'));
NET.addAssembly(fullfile(atlasDir, 'MAT.Atlas.Automation.Client.dll'));
import MAT.Atlas.Automation.Client.Services.*;

clientName = 'AutomationApiExample';
application = ApplicationServiceClient(clientName);
cleanup = onCleanup(@() application.Dispose()); %#ok<NASGU>

try
    application.Connect(clientName);
catch e
    disp('Could not connect to ATLAS. Start ATLAS 11+ before running this example.');
    disp(e.message);
    return;
end

fprintf('Connected to ATLAS %s as %s\n', char(application.GetVersion()), clientName);
application.ShowMessage(sprintf('Hello from %s!', clientName));

end
