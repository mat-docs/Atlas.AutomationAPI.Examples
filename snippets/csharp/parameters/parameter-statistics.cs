// ATLAS Automation API Example: Parameter Statistics
//
// Demonstrates: Computing min / max / mean / standard deviation for a parameter
//               over the session time base, without reading every sample yourself
// Prerequisites: ATLAS 11+ running; a session loaded into Set 1 containing 'vCar:Chassis'
// Output:        A statistics summary for the parameter
// Related:       ../getting-started/04-read-parameter-samples.cs
//
// GetDataStatistics takes a start time and a *duration* (not an end time) and a
// bitmask of StatisticOption flags describing which statistics to compute.

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
if (parameter == null) { Console.WriteLine($"'{ParameterName}' not found."); return; }

var timebase = SessionServiceClient.Call(client => client.GetSessionTimeBase(sessionId));
var duration = timebase.EndTime - timebase.StartTime;

var stats = ParameterDataAccessServiceClient.Call(
    client => client.GetDataStatistics(
        sessionId,
        parameter.Identifier,
        timebase.StartTime,
        duration,
        StatisticOption.Min | StatisticOption.Max | StatisticOption.Mean | StatisticOption.StandardDeviation));

Console.WriteLine($"{parameter.Identifier} ({parameter.Units}) over {duration / 1_000_000_000.0:F1} s:");
Console.WriteLine($"  samples:  {stats.NumberOfSamples}");
Console.WriteLine($"  min:      {stats.Minimum:F2}");
Console.WriteLine($"  max:      {stats.Maximum:F2}");
Console.WriteLine($"  mean:     {stats.Mean:F2}");
Console.WriteLine($"  std dev:  {stats.StandardDeviation:F2}");
