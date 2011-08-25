﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MongoDB.Driver;

namespace ØvelsesPlanTests
{
    public class MongoIntegrationSpec : IDisposable
    {
        protected const int mongoPort = 27020;
        private readonly Process mongoProcess;
        protected MongoServer mongoServer;
        protected readonly string mongoConnectionString;
        protected string databaseName = "OevelsesPlan";

        public MongoIntegrationSpec()
        {
            const string dataPath = "oevelsesplantest";
            Directory.CreateDirectory(dataPath);
            var mongoStartupCommand = new ProcessStartInfo("mongod.exe", string.Format("--dbpath {0} --port {1}", dataPath, mongoPort));
            mongoProcess = Process.Start(mongoStartupCommand);

            mongoServer = MongoServer.Create("mongodb://localhost:27020");
            mongoConnectionString = "mongodb://localhost:" + mongoPort + "/" + databaseName;

            mongoServer.DropDatabase(databaseName);
        }

        public void Dispose()
        {
            mongoServer.Disconnect();
            if (!mongoProcess.HasExited)
                mongoProcess.CloseMainWindow();
        }
    }
}