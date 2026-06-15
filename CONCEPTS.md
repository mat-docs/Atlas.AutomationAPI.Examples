<img src="images/MotionAppliedLogo.png" alt="Motion Applied" width="200">

# Concepts

The Automation API drives the ATLAS application, so its building blocks mirror
what you see in the ATLAS UI. Understanding these makes every example easier to
read.

## The object model

| Concept | What it is | Key API |
|---------|-----------|---------|
| **Workbook** | The top-level ATLAS document. Holds sets, pages and displays. | `IWorkbookService` |
| **Set** | A container that sessions are loaded into. Compare mode operates across the sessions in a set. | `ISetService` |
| **Session** | A recording loaded into a set. A **composite session** can combine several sessions (e.g. for comparison or appended laps). | `ISessionService` |
| **Parameter** | A channel of time-series data, identified as `Name:Group` (e.g. `vCar:Chassis`). | `ISessionService`, `IParameterDataAccessService` |
| **Transient parameter** | An in-memory parameter you create and write to at runtime (a calculated channel). Not persisted to the database. | `AddTransientParameter`, `AddTimeDataToTransientParameter` |
| **Page** | A tab in the workbook holding one or more displays, associated with a set. | `IPageService` |
| **Display** | A visualisation (Waveform, Numeric, Track Map, …) showing parameters. | `IDisplayService` |
| **Time base** | The visible time window and cursor position, in nanoseconds. | `ITimeBaseService` |
| **Lap / Segment** | A named span of the session (a lap, test point or regime). | `GetSessionLaps`, `GetSessionFastestLap` |
| **Event** | A timestamped annotation/marker in the session. | `GetSessionEvents`, `AddSessionEvent` |

## How a typical example flows

```
Connect ─▶ GetSets ─▶ GetCompositeSessions ─▶ GetSessionParameter
                                                      │
                          GetSessionTimeBase ◀────────┘
                                  │
       GetSamplesCountEstimate ─▶ Goto ─▶ GetNextSamples  (read)
                                  │
        AddTransientParameter ─▶ AddTimeDataToTransientParameter  (write)
                                  │
              CreateDisplay ─▶ AddDisplayParameter  (visualise)
```

## A note on time

All timestamps are **nanoseconds** (`Int64`). To convert to seconds, divide by
1,000,000,000. The COM/VBA surface mirrors each time-based call with a `…Vba`
variant that uses `Double` instead of `Int64`, because VBA has no 64-bit integer.

## Industry applicability

ATLAS uses generic concepts that map to any domain:

| ATLAS | Motorsport | Aerospace | Energy | Automotive |
|-------|-----------|-----------|--------|------------|
| Session | Race / Practice | Flight / Sortie | Test Run | Drive Cycle |
| Lap | Lap | Test Point | Regime | Segment |
| Parameter | Channel | Measurement | Tag | Signal |
| Event | Flag / Marker | Phase change | Alarm | Trigger |
