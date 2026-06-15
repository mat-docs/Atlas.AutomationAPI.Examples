// ATLAS Automation API Example: Add a Transient Parameter
//
// Demonstrates: Creating a transient (in-memory) parameter on a loaded session
// Prerequisites: ATLAS 11+ running; a session loaded into Set 1
// Output:        Confirmation that the transient parameter exists
// Related:       compute-and-write-transient.cs
//
// A transient parameter lives only in the current ATLAS session - it is not
// written back to the database. Re-running is safe here because we remove any
// previous version first (the original could only be run once per session).

using System;
using MAT.Atlas.Automation.Client.Services;

const string Identifier = "vCarMinus50:Demo";
const string DisplayName = "vCarMinus50";
const string Description = "vCar offset by 50 km/h (demo transient)";
const string Group = "Demo";

var sets = WorkbookServiceClient.Call(client => client.GetSets());
if (sets.Length == 0) { Console.WriteLine("No sets configured."); return; }

var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(sets[0].Id));
if (sessions.Length == 0) { Console.WriteLine("No sessions loaded."); return; }

var sessionId = sessions[0].Id;

// Remove a previous run's parameter so this snippet is idempotent.
try
{
    SessionServiceClient.Call(client => client.RemoveTransientParameter(sessionId, Identifier));
}
catch
{
    // Not present yet - nothing to remove.
}

var transient = SessionServiceClient.Call(
    client => client.AddTransientParameter(
        sessionId,
        Identifier,
        DisplayName,
        Description,
        new[] { Group },
        minimum: 0,
        maximum: 800));

Console.WriteLine($"Created transient parameter '{transient.Identifier}'.");
Console.WriteLine("Add data to it with compute-and-write-transient.cs.");
