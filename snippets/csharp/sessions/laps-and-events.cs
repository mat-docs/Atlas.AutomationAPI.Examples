// ATLAS Automation API Example: Laps and Events
//
// Demonstrates: Listing laps, finding the fastest lap, and reading session events
// Prerequisites: ATLAS 11+ running; a session loaded into Set 1 that contains laps
// Output:        A lap table, the fastest lap, and the first few session events
// Related:       session-details.cs
//
// Laps and events are generic markers: in motorsport they are laps and flags;
// in aerospace/energy they map to test points, regimes or named segments.

using System;
using MAT.Atlas.Automation.Client.Services;

var sets = WorkbookServiceClient.Call(client => client.GetSets());
if (sets.Length == 0) { Console.WriteLine("No sets configured."); return; }

var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(sets[0].Id));
if (sessions.Length == 0) { Console.WriteLine("No sessions loaded."); return; }

var sessionId = sessions[0].Id;

// --- Laps ---
var laps = SessionServiceClient.Call(client => client.GetSessionLaps(sessionId));
Console.WriteLine($"Laps: {laps.Length}");
foreach (var lap in laps)
{
    Console.WriteLine($"  #{lap.Number,-3} {lap.Name,-12} {lap.LapTime / 1_000_000_000.0,8:F3} s");
}

if (laps.Length > 0)
{
    var fastest = SessionServiceClient.Call(client => client.GetSessionFastestLap(sessionId));
    Console.WriteLine($"Fastest: #{fastest.Number} ({fastest.LapTime / 1_000_000_000.0:F3} s)");
}

// --- Events ---
var events = SessionServiceClient.Call(client => client.GetSessionEvents(sessionId));
Console.WriteLine($"\nEvents: {events.Length}");
foreach (var sessionEvent in events[..Math.Min(5, events.Length)])
{
    Console.WriteLine($"  [{sessionEvent.Priority}] {sessionEvent.GroupName}: {sessionEvent.Description}");
}
