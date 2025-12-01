<img src="/images/malogo.png" width="300" align="right" /><br><br><br>

# Motion Applied **ATLAS Automation API Sample Code**.

ATLAS Automation API allows an external application to control an instance of ATLAS that is currently running.

This solution contains the following samples: 
1. C# application. 
2. MATLAB script.
3. Python script
4. Visual Basic

All sample codes do the same<sup>1</sup>: creating a transient parameter, adding a Waveform Display, then `vCar` and the new parameter to it. 

**Instructions**: Before running, start ATLAS, load a session<sup>1</sup> with parameter `vCar` in it and execute the sample application<sup>2</sup>. 

>**Notes:**
>
><sup>1</sup> The C\# sample code uses an API call to automatically load a session into set (user will need to modify the constant properties `ConnectionString` and `SessionKey` accordingly). Note that connection strings are constructed differently depending on the database targeted, as follows:
>  - For SQL Server: `server=SQLServer\InstanceName;Initial Catalog=databaseName;Trusted_Connection=True;`
>  - For ssn2: `DbEngine=SQLite;Data Source=path\to\file.ssn2;Pooling=false;`
>  - For ssndb: `DbEngine=SQLite;Data Source=path\to\file.ssndb;Pooling=false;`
> 
> <sup>2</sup> You will need to reload the session after each run, as the same transient parameter cannot be added twice. 

ATLAS Automation API is available as a Nuget package to registered users from the **[McLaren Applied Nuget Repository](https://github.com/mat-docs/packages)**.

<br>

# COM Interface

A subset of the Automation API are exposed as COM objects. 

This example references the following libraries:
- MAT_Atlas_Automation_Client
- MAT_Atlas_Automation_Api
  
N.B. before these can be used they first must be registered. 

## Registering COM Libraries

The following DLLs need to be registered to allow the WCF API to be usable from COM (and thus
VBA):
- MAT.Atlas.Automation.Client
- MAT.Atlas.Automation.Api

If you are upgrading ATLAS 10 and plan to use the latest automation DLLs you must unregister previous registrations first. 

N.B. registration is not required to use the WCF API from C#, MATLAB or Python.

## From ATLAS 11.2.3.465 Onwards (.NET 6)
### Register
- Download dscom.exe from the release page (https://github.com/dspace-group/dscom, with this library you can register assemblies and classes for COM and programmatically generate TLBs at runtime)
- Run cmd.exe *as administrator*, change directory location to  dscom.exe location

- Register MAT.Atlas.Automation.Api.dll with the commands below.
```
.\dscom.exe tlbexport "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.dll" --out "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.tlb"
.\dscom.exe tlbregister "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.tlb"
```

- Register MAT.Atlas.Automation.Client.dll with the command below.
```
.\dscom.exe tlbexport "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.dll" --out "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.tlb"
.\dscom.exe tlbregister "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.tlb"
```

- Change directory location to `C:\Windows\System32`
- Register both libraries to the registry with regsvr32
```
cd C:\Windows\System32\
.\regsvr32.exe "C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.comhost.dll"
.\regsvr32.exe "C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.comhost.dll"
```

### Unregister
- Run cmd.exe *as administrator*
- Change directory location to dscom.exe location
- UnRegister MAT.Atlas.Automation.Api.dll and MAT.Atlas.Automation.Client.dll with the commands below.
```
.\dscom.exe tlbunregister "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.tlb"
.\dscom.exe tlbunregister "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.tlb"
```

- Change directory location to `C:\Windows\System32`
- Unregister both libraries from the registry with regsvr32
```
cd C:\Windows\System32
.\regsvr32.exe /u "C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.comhost.dll"
.\regsvr32.exe /u "C:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.comhost.dll"
```
### Debug and Run
- Launch ATLAS 10
- Load a Session into Set 1 via the Session Browser
- Open HelloWorld.Vba.xlsm
- Go to the Developer tab
- Select Visual Basic
- Double click the HelloWord under Modules to open
- Add breakpoints in and click the Atlas 10 Hello World button or press F5 to start debugging

## Prior To ATLAS 11.2.3.465 (.NET 4)
### Register
- Run cmd.exe as administrator
- Change directory to the location of regasm.exe (Registration Assembly Tool)
N.B. this can be found in the .Net Framework 4 installation folder. This version number may
vary depending upon the exact .Net version installed.
- Register MAT.Atlas.Automation.Api.dll and MAT.Atlas.Automation.Client.dll with the command below.
```
.\regasm "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.dll" /register /tlb /codebase
.\regasm "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.dll" /register /tlb /codebase
```

### Unregister
- Run cmd.exe as administrator
- Change directory to the location of regasm.exe (Registration Assembly Tool)
N.B. this can be found in the .Net Framework 4 installation folder. This version number may
vary depending upon the exact .Net version installed.
- Unregister MAT.Atlas.Automation.Api.dll and MAT.Atlas.Automation.Client.dll with the command below.
```
.\regasm "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Api.dll" /unregister /tlb
.\regasm "c:\Program Files\McLaren Applied Technologies\ATLAS 10\MAT.Atlas.Automation.Client.dll" /unregister /tlb
```

### Debug and Run
- Launch ATLAS 10
- Load a Session into Set 1 via the Session Browser
- Open HelloWorld.Vba.xlsm
- Go to the Developer tab
- Select Visual Basic
- Double click the HelloWord under Modules to open
- Add breakpoints in and click the Atlas 10 Hello World button or press F5 to start debugging
