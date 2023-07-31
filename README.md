<img src="/images/malogo.png" width="300" align="right" /><br><br><br>

# McLaren Applied **ATLAS Automation API Sample Code**.

ATLAS Automation API allows an external application to control an instance of ATLAS that is currently running.

This solution contains the following samples: 
1. C# application. 
2. MATLAB script.
3. Python script

All sample codes do the same<sup>1</sup>: creating a transient parameter, adding a Waveform Display, then `vCar` and the new parameter to it. 

**Instructions**: Before running, start ATLAS, load a session<sup>1</sup> with parameter `vCar` in it and execute the sample application<sup>2</sup>. 

>**Notes:** 
<sup>1</sup> The C\# sample code uses an API call to automatically load a session into set (user will need to modify the constant properties `ConnectionString` and `SessionKey` accordingly). Note that connection strings are constructed differently depending on the database targeted, as follows:
- For SQL Server: Server=serverIdentifier;Database=databaseName;Trusted_Connection=True;
- For SQLite: DBEngine=SQLite;Data Source=path\to\file.ssn2" 
(<sup>2</sup>) You will need to reload the session after each run, as the same transient parameter cannot be added twice. 

ATLAS Automation API is available as a Nuget package to registered users from the **[McLaren Applied Nuget Repository](https://github.com/mat-docs/packages)**.