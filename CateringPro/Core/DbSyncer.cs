using CateringPro.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotmim.Sync;
using Dotmim.Sync.SqlServer;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.Enumerations;
using System.Threading;
namespace CateringPro.Core
{
    public interface IDbSyncer
    {
        Task SyncDb();
    }
    public class DbSyncer : IDbSyncer
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CompanyUser> _logger;
        public DbSyncer(IConfiguration iConfig, ILogger<CompanyUser> logger)
        {
            _logger = logger;
            _config = iConfig;
        }
        public async Task SyncDb()
        {

            var serverConnectionString = _config.GetSection("ConnectionStrings").GetSection("RemoteConnection").Value;
            var clientConnectionString = _config.GetSection("ConnectionStrings").GetSection("LocalConnection").Value;
            var filename = _config.GetSection("ConnectionStrings").GetSection("LocalFileName").Value;

            // Create 2 Sql Sync providers
            // First provider is using the Sql change tracking feature. Don't forget to enable it on your database until running this code !
            // For instance, use this SQL statement on your server database : ALTER DATABASE AdventureWorks  SET CHANGE_TRACKING = ON  (CHANGE_RETENTION = 10 DAYS, AUTO_CLEANUP = ON)  
            // Otherwise, if you don't want to use Change Tracking feature, just change 'SqlSyncChangeTrackingProvider' to 'SqlSyncProvider'
            var serverProvider = new SqlSyncProvider(serverConnectionString);

            // Second provider is using plain old Sql Server provider, relying on triggers and tracking tables to create the sync environment
            var clientProvider = new Dotmim.Sync.Sqlite.SqliteSyncProvider(filename); //new SqlSyncProvider(clientConnectionString);

            // Tables involved in the sync process:
            var tables = new string[] { "Dishes", "Ingredients" };
            var cts = new CancellationTokenSource();
            var progress = new SynchronousProgress<ProgressArgs>(args =>
            _logger.LogWarning($"{args.Context.SyncStage}:\t{args.Message}"));

            // Creating an agent that will handle all the process
            var agent = new SyncAgent(clientProvider, serverProvider, CreateSyncOptions(),  CreateSyncSetup());
            var remoteProgress = new SynchronousProgress<ProgressArgs>(s =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{s.Context.SyncStage}:\t{s.Message}");
                Console.ResetColor();
                _logger.LogWarning($"{s.Context.SyncStage}:\t{s.Message}");
            });
            agent.AddRemoteProgress(remoteProgress);

            // do
            //   {
            // Launch the sync process
            try
            {

                var s1 = await agent.SynchronizeAsync(SyncType.Reinitialize, cts.Token, progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SyncError");
            }
            // Write results
            //Console.WriteLine(s1);

            // } while (Console.ReadKey().Key != ConsoleKey.Escape);

            Console.WriteLine("End");
        }
        private SyncSetup CreateSyncSetup()
        {
            var tables = new string[] { "Dishes", "Ingredients" };
            var syncSetup = new SyncSetup(tables);
            syncSetup.Tables.ToList().ForEach(t => {
                t.SyncDirection = SyncDirection.DownloadOnly;
            });
            return syncSetup;
        }
        private SyncOptions CreateSyncOptions()
        {
           
            var syncopt = new SyncOptions();
            return syncopt;
        }
    }
}
