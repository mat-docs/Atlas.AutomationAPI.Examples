# VBA (COM Interface)

The same HelloWorld workflow as the other examples, driven from Excel VBA through
the Automation API's COM interface.

## Files

- [`HelloWorld.Vba.xlsm`](HelloWorld.Vba.xlsm) — the Excel workbook with the macro
- [`Modules/HelloWorld.bas`](Modules/HelloWorld.bas) — the macro source (for review/diffing)

## Prerequisites

1. ATLAS 11+ installed, with the COM interface **registered** — see
   [docs/com-interop.md](../../docs/com-interop.md).
2. A session containing `vCar:Chassis` loaded into Set 1.

## Run

1. Launch ATLAS and load a session into Set 1 via the Session Browser.
2. Open `HelloWorld.Vba.xlsm`.
3. Go to the **Developer** tab → **Visual Basic**.
4. Open `HelloWorld` under **Modules**.
5. Press **F5** (or add breakpoints and step through) to run `HelloWorldAtlas10`.

The macro reads `vCar:Chassis`, derives `vCar-50` as a transient parameter, and
adds both to a waveform display — the same end-to-end flow as the C#, Python and
MATLAB examples.
