// ATLAS Automation API Example: List Sets and Sessions
//
// Demonstrates: Enumerating the sets in the active workbook and the sessions in each
// Prerequisites: ATLAS 11+ running with at least one session loaded
//                (Session Browser -> load a session into Set 1)
// Output:        A tree of sets and their composite sessions
// Related:       01-connect-to-atlas.cs, 03-load-session-into-set.cs
//
// A "set" is a container that sessions are loaded into. Pages and displays are
// associated with a set, and compare mode operates across the sessions in a set.

using System;
using MAT.Atlas.Automation.Client.Services;

var sets = WorkbookServiceClient.Call(client => client.GetSets());
if (sets == null || sets.Length == 0)
{
    Console.WriteLine("No sets configured. Open the Session Browser in ATLAS and load a session.");
    return;
}

foreach (var set in sets)
{
    Console.WriteLine($"Set '{set.Name}'  (parameters: {set.Parameters})");

    var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(set.Id));
    if (sessions.Length == 0)
    {
        Console.WriteLine("    (no sessions loaded)");
        continue;
    }

    foreach (var session in sessions)
    {
        var primary = session.IsPrimarySession ? "* " : "  ";
        Console.WriteLine($"  {primary}{session.Name}  [{session.State}]  recorded {session.TimeOfRecording:u}");
    }
}
