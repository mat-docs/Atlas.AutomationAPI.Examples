// <copyright file="Program.cs" company="Motion Applied Ltd.">
// Copyright (c) Motion Applied Ltd.</copyright>

// HelloWorld - the canonical Automation API demo, built as a Tier 2 project on
// top of the shared Atlas.Automation.Examples.Core library.
//
// It reads vCar from the loaded session, derives vCar-50 as a transient
// parameter, and plots both on a waveform display. Run it with ATLAS 11+ open
// and a session containing 'vCar:Chassis' loaded into Set 1.
//
//   dotnet run --project projects/csharp/HelloWorld
//
// The example is idempotent - re-run it without reloading the session.

using System;
using System.Collections.Generic;
using System.Linq;
using Atlas.Automation.Examples.Core;
using MAT.Atlas.Automation.Api.Enums;
using MAT.Atlas.Automation.Client.Services;

const string SourceParameter = "vCar:Chassis";
const string TransientId = "vCarMinus50:Demo";
const string DisplayName = "Automation Demo";
const double Offset = 50.0;

using var application = AtlasConnection.Connect("HelloWorld");
Console.WriteLine($"Connected to ATLAS {application.GetVersion()}.");

var sessionId = AtlasConnection.FirstSession();

// Read the source parameter across the whole time base.
var parameter = SessionServiceClient.Call(client => client.GetSessionParameter(sessionId, SourceParameter));
if (parameter == null)
{
    Console.WriteLine($"'{SourceParameter}' not found in the loaded session.");
    return;
}

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
        values.Add(batch.Data[i] - Offset);
    }
}
done:
Console.WriteLine($"Read {timestamps.Count} samples of {parameter.Identifier}.");

// (Re)create the transient parameter and write the derived data.
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

// Show both channels on a waveform display.
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
application.ShowMessage("HelloWorld complete - see the 'Automation Demo' waveform.");
