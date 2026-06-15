// ATLAS Automation API Example: Load a Session into a Set
//
// Demonstrates: Loading a SQL Race session into a set by key + connection string,
//               then waiting until ATLAS reports it as fully loaded
// Prerequisites: ATLAS 11+ running; a valid session key and connection string
// Output:        Confirmation once the session appears in the set
// Related:       02-list-sets-and-sessions.cs
// Docs:          ../../docs/connection-strings.md
//
// Loading is asynchronous: LoadSqlRaceSessions returns immediately and ATLAS
// loads in the background, so we poll the set until the new session shows up.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MAT.Atlas.Automation.Client.Services;

// --- Configure these for your session (see docs/connection-strings.md) ---
const string ConnectionString = @"DbEngine=SQLite;Data Source=C:\path\to\file.ssn2;Pooling=false;";
const string SessionKey = "00000000-0000-0000-0000-000000000000";

var sets = WorkbookServiceClient.Call(client => client.GetSets());
if (sets.Length == 0)
{
    Console.WriteLine("No sets configured.");
    return;
}

var setId = sets[0].Id;
var before = SetServiceClient.Call(client => client.GetCompositeSessions(setId)).Length;

using var application = new ApplicationServiceClient("AutomationApiExample");
application.Call(client => client.LoadSqlRaceSessions(setId, new[] { SessionKey }, new[] { ConnectionString }));

Console.WriteLine("Loading session...");
var timeout = TimeSpan.FromSeconds(30);
var stopwatch = Stopwatch.StartNew();
while (stopwatch.Elapsed < timeout)
{
    var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(setId));
    if (sessions.Length > before && sessions.All(s => !string.IsNullOrWhiteSpace(s.Name)))
    {
        Console.WriteLine($"Loaded: {sessions.Last().Name}");
        return;
    }

    Thread.Sleep(1000);
}

Console.WriteLine("Timed out waiting for the session to load.");
