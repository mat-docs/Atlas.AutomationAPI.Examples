// ATLAS Automation API Example: Create a Waveform Display
//
// Demonstrates: Creating a display in the active workbook, reusing it if it already exists
// Prerequisites: ATLAS 11+ running
// Output:        The id, name and type of the display
// Related:       add-parameters-to-display.cs, create-page-with-displays.cs
//
// CreateDisplay(type, name) accepts any ATLAS display type by name, e.g.
// "Waveform", "Numeric", "TimeDistance" or "Track Map".

using System;
using System.Linq;
using MAT.Atlas.Automation.Client.Services;

const string DisplayName = "Automation Demo";

// Reuse the display if it already exists, otherwise create a new waveform.
var displays = WorkbookServiceClient.Call(client => client.GetDisplays());
var display = displays.FirstOrDefault(d => d.Name == DisplayName)
              ?? WorkbookServiceClient.Call(client => client.CreateDisplay("Waveform", DisplayName));

Console.WriteLine($"Display '{display.Name}' ({display.Type}) is ready.");
Console.WriteLine($"  Id:    {display.Id.Id}");
Console.WriteLine($"  Live:  {display.IsLive}");
