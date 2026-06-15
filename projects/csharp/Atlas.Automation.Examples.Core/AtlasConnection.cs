// <copyright file="AtlasConnection.cs" company="Motion Applied Ltd.">
// Copyright (c) Motion Applied Ltd.</copyright>

using System;
using System.Threading;
using MAT.Atlas.Automation.Api.Models;
using MAT.Atlas.Automation.Client.Services;

namespace Atlas.Automation.Examples.Core
{
    /// <summary>
    ///     Small convenience helpers shared by the example projects: connecting to ATLAS and
    ///     locating the first set / session so each example can focus on what it demonstrates.
    /// </summary>
    public static class AtlasConnection
    {
        /// <summary>
        ///     Connects to ATLAS, retrying while it finishes starting up.
        /// </summary>
        /// <param name="clientName">The name ATLAS will display for this client.</param>
        /// <param name="attempts">How many times to retry before giving up.</param>
        /// <returns>A connected <see cref="ApplicationServiceClient" /> (caller disposes).</returns>
        public static ApplicationServiceClient Connect(string clientName, int attempts = 10)
        {
            var application = new ApplicationServiceClient(clientName);
            for (var attempt = 1;; attempt++)
            {
                try
                {
                    application.Connect(clientName);
                    return application;
                }
                catch when (attempt < attempts)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        /// <summary>
        ///     Returns the id of the first configured set.
        /// </summary>
        public static ObjectId FirstSetId()
        {
            var sets = WorkbookServiceClient.Call(client => client.GetSets());
            if (sets == null || sets.Length == 0)
            {
                throw new InvalidOperationException("No sets configured in ATLAS.");
            }

            return sets[0].Id;
        }

        /// <summary>
        ///     Returns the id of the first session loaded into the first set.
        /// </summary>
        public static SessionId FirstSession()
        {
            var sessions = SetServiceClient.Call(client => client.GetCompositeSessions(FirstSetId()));
            if (sessions == null || sessions.Length == 0)
            {
                throw new InvalidOperationException("No sessions loaded. Load a session into Set 1 first.");
            }

            return sessions[0].Id;
        }
    }
}
