using System;
using System.Reflection;

using Xunit;
using Xunit.Abstractions;

using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace NCurses.Core.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine($"Please attach a debugger to process {Process.GetCurrentProcess().Id}...");
                while (!Debugger.IsAttached) { Thread.Sleep(100); }
            }
#endif

            DiscoverAndRunTests();
        }

        private static void DiscoverAndRunTests()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            IRunnerLogger logger = new ConsoleRunnerLogger(true);
            IMessageSink messageSink = new DefaultRunnerReporterWithTypesMessageHandler(logger);

            XunitFrontController frontController = new XunitFrontController
            (
                AppDomainSupport.Denied,
                currentAssembly.GetLocalCodeBase(),
                sourceInformationProvider: new NullSourceInformationProvider(),
                diagnosticMessageSink: messageSink
            );

            ITestFrameworkDiscoveryOptions discoveryOptions = TestFrameworkOptions.ForDiscovery();
            discoveryOptions.SetMethodDisplay(TestMethodDisplay.ClassAndMethod);

            ITestFrameworkExecutionOptions executionOptions = TestFrameworkOptions.ForExecution();
            executionOptions.SetDisableParallelization(true);
            executionOptions.SetMaxParallelThreads(1);
            executionOptions.SetDiagnosticMessages(true);

            frontController.RunAll(messageSink, discoveryOptions, executionOptions);
        }
    }
}
