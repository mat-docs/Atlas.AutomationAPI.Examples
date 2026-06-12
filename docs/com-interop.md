# COM Interface (VBA / Excel)

A subset of the Automation API is exposed as COM objects so it can be used from
COM hosts such as **Excel VBA**. This example references two libraries:

- `MAT.Atlas.Automation.Client`
- `MAT.Atlas.Automation.Api`

> **Registration is only required for COM hosts (e.g. VBA).** You do **not** need
> to register anything to use the API from C#, Python or MATLAB.

If you are upgrading ATLAS and intend to use the latest automation DLLs, you must
**unregister previous registrations first**.

> The paths below assume the default install folder
> `C:\Program Files\McLaren Applied Technologies\ATLAS 10`. Adjust them to match
> your installation.

---

## .NET 8 (ATLAS 11.4.4.349-W47 onwards) and .NET 6 (ATLAS 11.2.3.465 onwards)

These builds register via [dscom](https://github.com/dspace-group/dscom), which
exports type libraries (TLBs) for .NET assemblies.

### Register

1. Download `dscom.exe` from the dscom release page.
2. Run `cmd.exe` **as administrator** and change directory to the `dscom.exe` location.
3. Export and register the API type library:

   ```
   .\dscom.exe tlbexport "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.dll" --out "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.tlb"
   .\dscom.exe tlbregister "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.tlb"
   ```

4. Export and register the Client type library:

   ```
   .\dscom.exe tlbexport "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.dll" --out "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.tlb"
   .\dscom.exe tlbregister "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.tlb"
   ```

5. Register the COM hosts with `regsvr32`:

   ```
   cd C:\Windows\System32\
   .\regsvr32.exe "C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.comhost.dll"
   .\regsvr32.exe "C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.comhost.dll"
   ```

### Unregister

1. Run `cmd.exe` **as administrator** and change directory to the `dscom.exe` location.
2. Unregister the type libraries:

   ```
   .\dscom.exe tlbunregister "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.tlb"
   .\dscom.exe tlbunregister "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.tlb"
   ```

3. Unregister the COM hosts:

   ```
   cd C:\Windows\System32
   .\regsvr32.exe /u "C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.comhost.dll"
   .\regsvr32.exe /u "C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.comhost.dll"
   ```

---

## Prior to ATLAS 11.2.3.465 (.NET Framework 4)

These builds register with `regasm.exe` (the .NET Framework Assembly Registration
Tool, found in the .NET Framework 4 installation folder — the exact version may
vary).

### Register

```
.\regasm "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.dll" /register /tlb /codebase
.\regasm "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.dll" /register /tlb /codebase
```

### Unregister

```
.\regasm "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.dll" /unregister /tlb
.\regasm "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.dll" /unregister /tlb
```

---

## Run the VBA example

1. Launch ATLAS and load a session into **Set 1** via the Session Browser.
2. Open [`projects/vba/HelloWorld.Vba.xlsm`](../projects/vba/HelloWorld.Vba.xlsm).
3. Go to the **Developer** tab → **Visual Basic**.
4. Double-click **HelloWorld** under **Modules**.
5. Add breakpoints if you like, then press **F5** to run `HelloWorldAtlas10`.
