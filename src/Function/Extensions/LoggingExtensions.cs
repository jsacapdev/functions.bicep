using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace AFunction;

public static class LoggingExtensions
{
    public static void LogTrace(this ILogger logger, TelemetryClient telemetryClient, string message)
    {
        telemetryClient.TrackTrace(message);

        logger.LogInformation(message);
    }
}