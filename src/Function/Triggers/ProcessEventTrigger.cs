using Azure.Messaging.EventHubs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AFunction;

public class ProcessEventTrigger
{
    private readonly ILogger<ProcessEventTrigger> _logger;

    private readonly TelemetryClient _telemetryClient;

    public ProcessEventTrigger(ILogger<ProcessEventTrigger> logger, TelemetryConfiguration telemetryConfiguration)
    {
        _logger = logger;
        _telemetryClient = new TelemetryClient(telemetryConfiguration);
    }

    [Function(Constants.ProcessEventTrigger)]
    public void Run([EventHubTrigger("%EventHubName%", Connection = "EventHubConnection", ConsumerGroup = "%ConsumerGroup%")] EventData[] events)
    {
        foreach (EventData @event in events)
        {
            var id = @event.Properties["CorrelationId"];

            _logger.LogTrace(_telemetryClient, $"Retrieved item from event hub. CorrelationId: {id}");
        }
    }
}