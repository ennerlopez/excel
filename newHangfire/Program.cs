using System;
using System.Data;
using Hangfire;
using Hangfire.Oracle.Core;

namespace newHangfire
{
    class Program
    {
        static void Main(string[] args)
        {
            
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()

                
                .UseStorage(new OracleStorage(
                    "user id=digitaleconomy;password=DigitalEconomy1234!;data source=//localhost:51521/XE",
                    
                    new OracleStorageOptions
                    {
                        
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        //SchemaName = "DIGITALECONOMY"
                    }));
                


            BackgroundJob.Enqueue(() => Notificar());

            using (var server = new BackgroundJobServer())
            {
                Console.ReadLine();
            }
        }
        public static void Notificar() {
            throw new ArgumentNullException();
            Console.WriteLine("Hello, world!");
        }
    }
}