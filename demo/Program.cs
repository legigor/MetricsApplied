using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Prometheus;
using Prometheus.Advanced;
using Serilog;

namespace MetricsDemo
{
    class Program
    {
        static readonly Counter   requests = Metrics.CreateCounter(  "demo_ping_google_requests_total",           "Total requests");
        static readonly Counter   errors =   Metrics.CreateCounter(  "demo_ping_google_errors_total",             "Total errors");
        static readonly Histogram duration = Metrics.CreateHistogram("demo_ping_google_request_duration_seconds", "Request duration");

        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            // Disable default .NET metrics
            DefaultCollectorRegistry.Instance.Clear();

            var server = new MetricServer(5656);
            server.Start();

            Task.WaitAll(Enumerable.Repeat(PingGoogle(), 100).ToArray());
        }

        static async Task PingGoogle()
        {
            while (true)
            {
                requests.Inc();

                var sw = Stopwatch.StartNew();

                try
                {
                    await Google.Ping();
                    sw.Stop();

                    Log.Information("Ping duration: {duration}", sw.Elapsed.TotalSeconds);
                }
                catch (Exception e)
                {
                    sw.Stop();
                    errors.Inc();

                    Log.Error("Error when pinging: {error}", e.Message);
                }

                duration.Observe(sw.Elapsed.TotalSeconds);
            }
        }
    }
}
