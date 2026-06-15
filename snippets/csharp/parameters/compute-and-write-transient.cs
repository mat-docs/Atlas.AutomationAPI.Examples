// ATLAS Automation API Example: Compute and Write a Transient Parameter
//
// Demonstrates: The end-to-end "hello world" of the Automation API -
//               read a parameter, derive a new channel from it, write it back
//               as a transient parameter, and plot both on a waveform display.
// Prerequisites: ATLAS 11+ running; a session loaded into Set 1 containing 'vCar:Chassis'
// Output:        A "Automation Demo" waveform showing vCar and vCar-50
// Related:       add-transient-parameter.cs, ../getting-started/04-read-parameter-samples.cs
//
// This is the cleaned-up version of the classic sample. It is idempotent:
// re-run it as often as you like without reloading the session.

using System;
using System.Collections.Generic;
using System.Linq;
using MAT.Atlas.Automation.Api.Enums;
using MAT.Atlas.Automation.Client.Services;

const string SourceParameter = "vCar:Chassis";
const string TransientId = "vCarMinus50:Demo";
const string DisplayName = "Automation Demo";
const double Offset = 50.0;

// --- 1. Find the loaded session ---
var sets = WorkbookServiceClient.Call(client => client.GetSets());
if (sets.Length == 0) { Console.WriteLine("No sets configured."); return; }

var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(sets[0].Id));
if (sessions.Length == 0) { Console.WriteLine("No sessions loaded."); return; }

var sessionId = sessions[0].Id;

// --- 2. Read the source parameter across the whole time base ---
var parameter = SessionServiceClient.Call(client => client.GetSessionParameter(sessionId, SourceParameter));
if (parameter == null) { Console.WriteLine($"'{SourceParameter}' not found."); return; }

var timebase = SessionServiceClient.Call(client => client.GetSessionTimeBase(sessionId));
var estimate = ParameterDataAccessServiceClient.Call(
    client => client.GetSamplesCountEstimate(sessionId, parameter.Identifier, timebase.StartTime, timebase.EndTime));

ParameterDataAccessServiceClient.Call(client => client.Goto(sessionId, parameter.Identifier, timebase.StartTime));

var timestamps = new List<long>((int)estimate);
var values = new List<double>((int)estimate);
while (true)
{
    var batch = ParameterDataAccessServiceClient.Call(
        client => client.GetNextSamples(sessionId, parameter.Identifier, estimate));
    if (batch.SampleCount == 0) break;

    for (var i = 0; i < batch.SampleCount; i++)
    {
        if (!batch.DataStatus[i].HasFlag(DataStatusType.Sample)) continue;
        if (batch.Time[i] > timebase.EndTime) goto done;

        timestamps.Add(batch.Time[i]);
        values.Add(batch.Data[i] - Offset);   // <-- the derived channel
    }
}
done:
Console.WriteLine($"Read {timestamps.Count} samples of {parameter.Identifier}.");

// --- 3. (Re)create the transient parameter and write the derived data ---
try { SessionServiceClient.Call(client => client.RemoveTransientParameter(sessionId, TransientId)); }
catch { /* not present yet */ }

var transient = SessionServiceClient.Call(
    client => client.AddTransientParameter(
        sessionId, TransientId, "vCarMinus50", "vCar offset by 50 km/h",
        new[] { "Demo" }, minimum: 0, maximum: 800));

SessionServiceClient.Call(
    client => client.AddTimeDataToTransientParameter(
        sessionId, transient.Identifier, timestamps.ToArray(), values.ToArray()));

Console.WriteLine($"Wrote {values.Count} samples to {transient.Identifier}.");

// --- 4. Show both channels on a waveform display ---
var displays = WorkbookServiceClient.Call(client => client.GetDisplays());
var display = displays.FirstOrDefault(d => d.Name == DisplayName)
              ?? WorkbookServiceClient.Call(client => client.CreateDisplay("Waveform", DisplayName));

var onDisplay = DisplayServiceClient.Call(client => client.GetDisplayParameters(display.Id))
    .Select(p => p.Identifier).ToHashSet();

foreach (var id in new[] { parameter.Identifier, transient.Identifier })
{
    if (!onDisplay.Contains(id))
    {
        DisplayServiceClient.Call(client => client.AddDisplayParameter(display.Id, id));
    }
}

Console.WriteLine($"Display '{display.Name}' now shows {parameter.Identifier} and {transient.Identifier}.");
