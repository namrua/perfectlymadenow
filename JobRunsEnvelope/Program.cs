using System;
using System.Configuration;
using System.Net;
using System.Threading;
using AutomationSystem.Main.Core;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Core;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.DesignTools.Diagnostics;
using PerfectlyMadeInc.WebEx;

namespace JobRunsEnvelope
{
    /// <summary>
    /// Main entry point class
    /// </summary>
    class Program
    {
        // private constants
        private const int jobExecutionInMinutes = 1;
        private const int jobSchedulingInMinutes = 60;
        private const int jobSchedulingIntervalInMinutes = 24 * 60;

        // private components
        private static readonly ITracer tracer = new ComponentTracerFactory().CreateTracer<Program>();
        private static IJobScheduler scheduler;
        private static IJobRunExecutorManager executorManager;

        // private fields
        private static DateTime nextScheduling;
        private static DateTime nextExecution;

        // main entry point method
        static void Main()
        {
            tracer.Info("STARTED");

            try
            {
                // initializes components
                InitializeComponents();

                tracer.Info("Components initialized");
                tracer.Info($"Config = {ConfigurationManager.AppSettings["ConfigType"]}, Version = {ConfigurationManager.AppSettings["GitVersion"]}");

                // starts 
                var now = DateTime.Now;
                nextScheduling = nextExecution = now;
                while (true)
                {
                    // scheduling
                    if (nextScheduling <= now)
                    {
                        tracer.Info("Scheduling...");
                        var jobRunsId = scheduler.ScheduleJobRuns(now, now.AddMinutes(jobSchedulingIntervalInMinutes));
                        nextScheduling = nextScheduling.AddMinutes(jobSchedulingInMinutes);
                        tracer.Info($"Next scheduling time: {nextScheduling}\nScheduled JobRuns: {string.Join(", ", jobRunsId)}");
                    }

                    // executing
                    if (nextExecution <= now)
                    {
                        tracer.Info("Execution...");
                        executorManager.ExecuteAllJobRuns(now);
                        nextExecution = nextExecution.AddMinutes(jobExecutionInMinutes);
                        tracer.Info($"Next execution time: {nextExecution}");
                    }

                    // waiting time computation
                    now = DateTime.Now;
                    var msToWait = (int)(nextExecution - now).TotalMilliseconds;
                    if (msToWait <= 0)
                    {
                        tracer.Warning("Next execution time was exceed during job runs execution.");
                        continue;
                    }

                    // sleeping
                    tracer.Info($"Waiting of {msToWait}ms ...");
                    Thread.Sleep(msToWait);
                    now = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                tracer.Error(e, "Execution causes exception");
            }
            finally
            {
                tracer.Info("FINISHED");
            }
        }


        // initializes components
        private static void InitializeComponents()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDesignToolsServices();
            serviceCollection.AddSharedServices();
            serviceCollection.AddWebExServices();
            serviceCollection.AddMainServices();
            var serviceProvider = serviceCollection.BuildServiceProvider();

                // initializes components
            scheduler = serviceProvider.GetService<IJobScheduler>();
            executorManager = serviceProvider.GetService<IJobRunExecutorManager>();

            // Enable TLS 1.2
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol &= ~(SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11);
        }

    }

}
