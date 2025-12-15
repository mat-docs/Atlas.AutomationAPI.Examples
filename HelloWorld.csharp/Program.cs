// <copyright file="Program.cs" company="Motion Applied Ltd.">
// Copyright (c) Motion Applied Ltd.</copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAT.Atlas.Automation.Api.Enums;
using MAT.Atlas.Automation.Client.Services;

namespace HelloWorld.CSharp
{
    public static class Program
    {
        private const string DemoDisplayName = "Demo Waveform";
        private const string ParameterName = "vCar:Chassis";
        private const string TransientParameterDescription = "vCar2 Demo";
        private const string TransientParameterGroups = "";
        private const string TransientParameterIdentifier = "vCar2 Demo";
        private const double TransientParameterMaximum = 800;
        private const double TransientParameterMinimum = 0;
        private const string TransientParameterName = "vCar2 Demo";

        private const string ConnectionString = @"DBEngine=SQLite;Data Source=path\to\file.ssn2";
        private const string SessionKey = "0000-0000-0000-0000-0000";

        /// <summary>
        ///     This example demonstrates how to add a transient parameter within ATLAS 10. It does this by calling the Client
        ///     directly from within a C# Console Application.
        /// </summary>
        /// <prerequisites>
        ///     As a prerequisite this example expects ATLAS 10 to  be running.
        ///     `ConnectionString` and `SessionKey` will need to be modified accordingly to target the desired session.
        /// </prerequisites>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Get first set
            var sets = WorkbookServiceClient.Call(client => client.GetSets());
            if (sets == null ||
                sets.Length == 0)
            {
                Console.WriteLine("No Sets Configured");
                return;
            }

            var setId = sets[0].Id;

            // Load session into set
            var sessionLoader = new SessionLoader
            {
                ConnectionString = ConnectionString,
                SessionKey = SessionKey,
                SetId = setId
            };
            sessionLoader.LoadAndWait();

            // Get first session
            var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(setId));
            if (sessions == null ||
                sessions.Length == 0)
            {
                Console.WriteLine("No Sessions Loaded");
                return;
            }

            var sessionId = sessions[0].Id;

            // Create transient parameter
            var transientParameter = SessionServiceClient.Call(
                client => client.AddTransientParameter(
                    sessionId,
                    TransientParameterIdentifier,
                    TransientParameterName,
                    TransientParameterDescription,
                    TransientParameterGroups.Split(";".ToCharArray()),
                    TransientParameterMinimum,
                    TransientParameterMaximum));

            // Get time base
            var timebase = SessionServiceClient.Call(client => client.GetSessionTimeBase(sessionId));

            // Get vCar parameter
            var parameter = SessionServiceClient.Call(client => client.GetSessionParameter(sessionId, ParameterName));

            // Get estimated sample count
            var sampleCount = ParameterDataAccessServiceClient.Call(
                client => client.GetSamplesCountEstimate(sessionId, parameter.Identifier, timebase.StartTime, timebase.EndTime));

            // Goto start of time base
            ParameterDataAccessServiceClient.Call(client => client.Goto(sessionId, parameter.Identifier, timebase.StartTime));

            var timestamps2 = new List<long>((int)sampleCount);
            var values2 = new List<double>((int)sampleCount);

            var finished = false;
            while (!finished)
            {
                // Get vCar data
                var values = ParameterDataAccessServiceClient.Call(client => client.GetNextSamples(sessionId, parameter.Identifier, sampleCount));
                if (values.SampleCount == 0)
                {
                    break;
                }

                // Calculate vCar - 50
                for (var i = 0; i < values.SampleCount; ++i)
                {
                    if (values.DataStatus[i].HasFlag(DataStatusType.Sample))
                    {
                        var timestamp = values.Time[i];
                        if (timestamp > timebase.EndTime)
                        {
                            finished = true;
                            break;
                        }

                        timestamps2.Add(values.Time[i]);
                        values2.Add(values.Data[i] - 50);
                    }
                }
            }

            // Write vCar - 50
            SessionServiceClient.Call(client => client.AddTimeDataToTransientParameter(sessionId, transientParameter.Identifier, timestamps2.ToArray(), values2.ToArray()));

            // Find or create display
            var displays = WorkbookServiceClient.Call(client => client.GetDisplays());
            var display = displays.FirstOrDefault(d => d.Name == DemoDisplayName) ??
                          WorkbookServiceClient.Call(client => client.CreateDisplay("Waveform", DemoDisplayName));

            var displayParameters = DisplayServiceClient.Call(client => client.GetDisplayParameters(display.Id));

            // Add vCar if not already
            if (!displayParameters.Any(d => d.Identifier == ParameterName || d.Name == ParameterName))
            {
                DisplayServiceClient.Call(client => client.AddDisplayParameter(display.Id, ParameterName));
            }

            // Add "vCar2 Demo" if not already
            if (displayParameters.All(d => d.Identifier != transientParameter.Identifier))
            {
                DisplayServiceClient.Call(client => client.AddDisplayParameter(display.Id, transientParameter.Identifier));
            }
        }

        private static async Task<TResult> WaitOnAsync<TResult>(
            string description,
            Func<TResult> getResource,
            Func<TResult, bool> isReady,
            TimeSpan? timeout = null)
        {
            Console.WriteLine($"Waiting for {description}");

            var startedWaitingAt = DateTime.UtcNow;
            var waitOnIterations = 30;
            var delayInMilliseconds = 1000;
            for (var i = 0; i < waitOnIterations; ++i)
            {
                try
                {
                    var result = getResource();
                    if (isReady(result))
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                if (timeout.HasValue)
                {
                    var waitedFor = DateTime.UtcNow - startedWaitingAt;
                    if (waitedFor > timeout.Value)
                    {
                        break;
                    }
                }

                await Task.Delay(delayInMilliseconds);
            }

            Console.WriteLine("Timed out!");
            return default;
        }
    }
}