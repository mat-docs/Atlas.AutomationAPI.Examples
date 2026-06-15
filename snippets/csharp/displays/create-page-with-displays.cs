// ATLAS Automation API Example: Create a Page with a Display
//
// Demonstrates: Creating a page, associating it with a set, activating it,
//               and adding a display that shows that set's data
// Prerequisites: ATLAS 11+ running with at least one set
// Output:        The created page and the displays it contains
// Related:       create-waveform-display.cs, add-parameters-to-display.cs
//
// Pages and displays are both associated with a set; the association controls
// which session's data they show when the set's primary session changes.

using System;
using MAT.Atlas.Automation.Client.Services;

var sets = WorkbookServiceClient.Call(client => client.GetSets());
if (sets.Length == 0) { Console.WriteLine("No sets configured."); return; }

var setId = sets[0].Id;

// Create and activate a page bound to the set.
var page = WorkbookServiceClient.Call(client => client.CreatePage("Automation Page"));
PageServiceClient.Call(client => client.AssociateSetToPage(page.Id, setId));
PageServiceClient.Call(client => client.ActivatePage(page.Id));

// Displays created while the page is active are placed on it.
var display = WorkbookServiceClient.Call(client => client.CreateDisplay("Waveform", "Page Waveform"));
DisplayServiceClient.Call(client => client.AssociateSetToDisplay(display.Id, setId));

var onPage = PageServiceClient.Call(client => client.GetDisplaysOfPage(page.Id));
Console.WriteLine($"Page '{page.Title}' is associated with set '{sets[0].Name}'.");
Console.WriteLine($"Displays on the page: {onPage.Length}");
