// <copyright file="SessionLoader.cs" company="Motion Applied Ltd.">
// Copyright (c) Motion Applied Ltd.</copyright>

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using MAT.Atlas.Automation.Api.Models;
using MAT.Atlas.Automation.Client.Services;

namespace HelloWorld.CSharp
{
    internal class SessionLoader
    {
        private const int SecondsToMilliseconds = 1000;

        public string ConnectionString { get; set; }

        public string SessionKey { get; set; }

        public ObjectId SetId { get; set; }

        public int IntervalInSeconds => 1;

        public int TimeoutInSeconds => 30;

        public void LoadAndWait()
        {
            if (this.SetId == null)
            {
                throw new ArgumentNullException(nameof(this.SetId));
            }

            if (this.ConnectionString == null)
            {
                throw new ArgumentNullException(nameof(this.ConnectionString));
            }

            if (this.SessionKey == null)
            {
                throw new ArgumentNullException(nameof(this.SessionKey));
            }

            var applicationService =
                new ApplicationServiceClient(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName));

            var currentSessionsCount = SetServiceClient.Call(client => client.GetCompositeSessions(this.SetId)).Length;
            var expectedSessionsCount = currentSessionsCount + 1;
            applicationService.Call(client => client.LoadSqlRaceSessions(this.SetId,
                new[] { this.SessionKey },
                new[] { this.ConnectionString }));

            Console.WriteLine("Waiting for session to load...");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var busy = true;
            while (busy)
            {
                if (stopWatch.Elapsed.Seconds > this.TimeoutInSeconds)
                {
                    stopWatch.Stop();
                    throw new TimeoutException("Timed out!");
                }

                try
                {
                    var loadedSessions = SetServiceClient.Call(client => client.GetCompositeSessions(SetId));
                    if (loadedSessions.Length < expectedSessionsCount ||
                        loadedSessions.Any(s => string.IsNullOrWhiteSpace(s.Name)))
                    {
                        Thread.Sleep(this.IntervalInSeconds * SecondsToMilliseconds);
                        continue;
                    }

                    var set = SetServiceClient.Call(client => client.GetSet(this.SetId));
                    if (set.Parameters > 0) busy = false;
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            stopWatch.Stop();
        }
    }
}