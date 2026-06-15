// ATLAS Automation API Example: Connect to ATLAS
//
// Demonstrates: Connecting to a running ATLAS instance and reading its version
// Prerequisites: ATLAS 11+ running on this machine
// Output:        The connected ATLAS version, or guidance if ATLAS is not running
// Related:       02-list-sets-and-sessions.cs
//
// The Automation API talks to a running ATLAS over WCF. Every *ServiceClient
// opens its channel on first use, so most snippets need no explicit Connect().
// This one connects explicitly so it can report a clear message when ATLAS is
// not running, and registers a friendly client name that ATLAS will display.

using System;
using MAT.Atlas.Automation.Client.Services;

const string ClientName = "AutomationApiExample";

using var application = new ApplicationServiceClient(ClientName);

try
{
    application.Connect(ClientName);

    var version = application.GetVersion();
    Console.WriteLine($"Connected to ATLAS {version} as '{ClientName}'.");

    // Pop a message in the ATLAS UI to prove the round-trip works.
    application.ShowMessage($"Hello from {ClientName}!");
}
catch (Exception ex)
{
    Console.WriteLine("Could not connect to ATLAS.");
    Console.WriteLine($"  {ex.Message}");
    Console.WriteLine("Tip: Start ATLAS 11+ before running this example.");
}
