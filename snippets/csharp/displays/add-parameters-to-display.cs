// ATLAS Automation API Example: Add Parameters to a Display
//
// Demonstrates: Adding parameters to a display without creating duplicates
// Prerequisites: ATLAS 11+ running; a session loaded that contains the parameters below
// Output:        What was added and what was already present
// Related:       create-waveform-display.cs
//
// Parameters are referenced by their fully-qualified identifier ("Name:Group").

using System;
using System.Linq;
using MAT.Atlas.Automation.Client.Services;

const string DisplayName = "Automation Demo";
string[] parametersToAdd = { "vCar:Chassis", "gLat:Chassis" };

var displays = WorkbookServiceClient.Call(client => client.GetDisplays());
var display = displays.FirstOrDefault(d => d.Name == DisplayName)
              ?? WorkbookServiceClient.Call(client => client.CreateDisplay("Waveform", DisplayName));

var present = DisplayServiceClient.Call(client => client.GetDisplayParameters(display.Id))
    .Select(p => p.Identifier)
    .ToHashSet();

foreach (var identifier in parametersToAdd)
{
    if (present.Contains(identifier))
    {
        Console.WriteLine($"= {identifier} already on display");
        continue;
    }

    DisplayServiceClient.Call(client => client.AddDisplayParameter(display.Id, identifier));
    Console.WriteLine($"+ {identifier} added");
}
