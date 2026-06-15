// ATLAS Automation API Example: Session Details (Metadata)
//
// Demonstrates: Reading and writing session detail key/value metadata
// Prerequisites: ATLAS 11+ running; a session loaded into Set 1
// Output:        Existing details, then a detail added/updated by this example
// Related:       laps-and-events.cs
//
// Session details are arbitrary name/value metadata attached to a session -
// useful for tagging sessions with test conditions, operator names, etc.

using System;
using MAT.Atlas.Automation.Client.Services;

const string DetailName = "AutomationExample.LastRun";

var sets = WorkbookServiceClient.Call(client => client.GetSets());
if (sets.Length == 0) { Console.WriteLine("No sets configured."); return; }

var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(sets[0].Id));
if (sessions.Length == 0) { Console.WriteLine("No sessions loaded."); return; }

var sessionId = sessions[0].Id;

// --- Read existing details ---
var details = SessionServiceClient.Call(client => client.GetSessionDetails(sessionId));
Console.WriteLine($"Existing details: {details.Length}");
foreach (var detail in details)
{
    Console.WriteLine($"  {detail.Name} = {detail.Value}");
}

// --- Add or update one of our own ---
var stamp = $"run-{Environment.TickCount}";
var added = SessionServiceClient.Call(client => client.AddOrUpdateSessionDetail(sessionId, DetailName, stamp));
Console.WriteLine($"\nSet {added.Name} = {added.Value}");
