// <copyright file="SessionLoader.cs" company="Motion Applied Ltd.">
// Copyright (c) Motion Applied Ltd.</copyright>

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MAT.Atlas.Automation.Api.Models;
using MAT.Atlas.Automation.Client.Services;

namespace Atlas.Automation.Examples.Core
{
    /// <summary>
    ///     Loads a SQL Race session into a set and waits until ATLAS reports it as fully loaded.
    ///     Loading is asynchronous, so the caller is blocked until the new session appears (or a
    ///     timeout elapses).
    /// </summary>
    public sealed class SessionLoader
    {
        /// <summary>How long to wait for the session to load before giving up.</summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>How often to poll ATLAS while waiting.</summary>
        public TimeSpan PollInterval { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        ///     Asks ATLAS to load a SQL Race session, then waits for it to appear in the set.
        /// </summary>
        /// <param name="application">A connected application client.</param>
        /// <param name="setId">The set to load the session into.</param>
        /// <param name="sessionKey">The SQL Race session key (GUID).</param>
        /// <param name="connectionString">The session's connection string (see docs/connection-strings.md).</param>
        public void LoadAndWait(ApplicationServiceClient application, ObjectId setId, string sessionKey, string connectionString)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (string.IsNullOrWhiteSpace(sessionKey)) throw new ArgumentNullException(nameof(sessionKey));
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            var before = SetServiceClient.Call(client => client.GetCompositeSessions(setId)).Length;

            application.Call(client => client.LoadSqlRaceSessions(
                setId,
                new[] { sessionKey },
                new[] { connectionString }));

            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < this.Timeout)
            {
                var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(setId));
                if (sessions.Length > before && sessions.All(s => !string.IsNullOrWhiteSpace(s.Name)))
                {
                    return;
                }

                Thread.Sleep(this.PollInterval);
            }

            throw new TimeoutException($"Session did not load within {this.Timeout.TotalSeconds:F0}s.");
        }
    }
}
