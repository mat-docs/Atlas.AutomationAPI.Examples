// ATLAS Automation API Example: Read Parameter Samples
//
// Demonstrates: Reading time-series samples for a parameter from a loaded session
// Prerequisites: ATLAS 11+ running; a session loaded into Set 1 containing 'vCar:Chassis'
// Output:        Sample count and the first few timestamp/value pairs
// Related:       02-list-sets-and-sessions.cs, ../parameters/compute-and-write-transient.cs
//
// Reading is a three-step pattern: estimate how many samples are in the range,
// Goto() the start of the time base, then GetNextSamples() to pull them.

using System;
using MAT.Atlas.Automation.Api.Enums;
using MAT.Atlas.Automation.Client.Services;

const string ParameterName = "vCar:Chassis";

var sets = WorkbookServiceClient.Call(client => client.GetSets());
if (sets.Length == 0) { Console.WriteLine("No sets configured."); return; }

var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(sets[0].Id));
if (sessions.Length == 0) { Console.WriteLine("No sessions loaded."); return; }

var sessionId = sessions[0].Id;

var parameter = SessionServiceClient.Call(client => client.GetSessionParameter(sessionId, ParameterName));
if (parameter == null)
{
    Console.WriteLine($"Parameter '{ParameterName}' not found in this session.");
    return;
}

var timebase = SessionServiceClient.Call(client => client.GetSessionTimeBase(sessionId));

var estimate = ParameterDataAccessServiceClient.Call(
    client => client.GetSamplesCountEstimate(sessionId, parameter.Identifier, timebase.StartTime, timebase.EndTime));

ParameterDataAccessServiceClient.Call(
    client => client.Goto(sessionId, parameter.Identifier, timebase.StartTime));

var samples = ParameterDataAccessServiceClient.Call(
    client => client.GetNextSamples(sessionId, parameter.Identifier, estimate));

Console.WriteLine($"{parameter.Identifier} ({parameter.Units})");
Console.WriteLine($"  estimated samples: {estimate}");
Console.WriteLine($"  returned samples:  {samples.SampleCount}");
Console.WriteLine("  first values:");
for (var i = 0; i < Math.Min(5, samples.SampleCount); i++)
{
    // Only 'Sample' status entries are real readings; the rest are gaps/interpolation.
    if (samples.DataStatus[i].HasFlag(DataStatusType.Sample))
    {
        Console.WriteLine($"    t={samples.Time[i]} ns  v={samples.Data[i]:F2}");
    }
}
