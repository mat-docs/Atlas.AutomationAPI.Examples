Attribute VB_Name = "HelloWorld"
Sub HelloWorldAtlas10()

' Services
Dim workbookService As New WorkbookServiceClient
Dim setsService As New SetServiceClient
Dim sessionsService As New SessionServiceClient
Dim pdaService As New ParameterDataAccessServiceClient
Dim displayService As New DisplayServiceClient

' Get first Set
Dim sets() As MAT_Atlas_Automation_Api.Set
sets = workbookService.GetSets()
Dim setsLength As Long
setsLength = UBound(sets) - LBound(sets) + 1
If setsLength = 0 Then
    MsgBox "No Sets"
    Exit Sub
End If

' Get first Session
Dim sessions() As MAT_Atlas_Automation_Api.CompositeSession
sessions = setsService.GetCompositeSessions(sets(0).ID)
Dim sessionsLength As Long
sessionsLength = UBound(sessions) - LBound(sessions) + 1
If sessionsLength = 0 Then
   MsgBox "No Sessions Loaded"
   Exit Sub
End If

Dim session As MAT_Atlas_Automation_Api.CompositeSession
Set session = sessions(0)

' Get vCar parameter
Dim parameter As MAT_Atlas_Automation_Api.parameter
Set parameter = sessionsService.GetSessionParameter(session.ID, "vCar:Chassis")

' Get current timebase
Dim timebase As MAT_Atlas_Automation_Api.timebase
Set timebase = sessionsService.GetSessionTimeBase(session.ID)

' Goto start of timebase
Call pdaService.GotoVba(session.ID, parameter.Identifier, timebase.StartTimeVba)

' Get vCar values
Dim parameterValues As MAT_Atlas_Automation_Api.parameterValues
Set parameterValues = pdaService.GetNextSamplesToTimeVba(session.ID, parameter.Identifier, timebase.EndTimeVba)

' Calculate vCar - 50
Dim data2() As Double
data2 = parameterValues.data
For N = 0 To parameterValues.SampleCountVba - 1
    data2(N) = data2(N) - 50
Next N
    
' Create transient parameter
Dim transientParameter As MAT_Atlas_Automation_Api.parameter
Set transientParameter = sessionsService.AddTransientParameterVba(session.ID, "vCar2 Demo", "vCar2 Demo", "vCar2 Demo", "", 0#, 800#)

' Write vCar - 50
Call sessionsService.AddTimeDataToTransientParameterVba(session.ID, transientParameter.Identifier, parameterValues.TimeVba, data2)

' Create waveform display
Dim display As MAT_Atlas_Automation_Api.display
Set display = workbookService.CreateDisplay("Waveform", "VBA Demo")

' Add parameters to waveform
Call displayService.AddDisplayParameter(display.ID, parameter.Identifier)
Call displayService.AddDisplayParameter(display.ID, transientParameter.Identifier)

End Sub
