<img src="/images/MotionAppliedLogo.png" width="300" align="right" /><br><br><br>

# Motion Applied **ATLAS Automation API Python Sample Code**.

1. Imports modules and stores paths (1-51)
2. Opens Atlas and confirms connection (54-71)
3. Loads sets and sessions (74-85)
4. Creates a range of variables needed (88-111)
5. Creates a transient parameter (113-131)
6. Creates waveform (134-143)
7. Displays parameters in waveform (146-163)
8. Closes clients (166-170)

This program is a sample of how to control an instance Atlas that is currently running externally through the APIs available with Python.

**Instructions**: 
1. Enter the following line of code into the terminal:
<code>pip install -r requirements.txt</code>
2. Before running, start ATLAS, load a session<sup>1</sup> with parameter `vCar` in it and execute the sample application<sup>2</sup>. 

>**Notes:** 
You will need to reload the session after each run, as the same transient parameter cannot be added twice. 

ATLAS Automation API is available as a Nuget package to registered users from the **[McLaren Applied Nuget Repository](https://github.com/mat-docs/packages)**.