using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace AFunction;

public class HealthCheckTrigger
{
    private readonly ILogger<HealthCheckTrigger> _logger;

    private readonly TelemetryClient _telemetryClient;

    public HealthCheckTrigger(ILogger<HealthCheckTrigger> logger, TelemetryConfiguration telemetryConfiguration)
    {
        _logger = logger;

        _telemetryClient = new TelemetryClient(telemetryConfiguration);
    }

    [Function(Constants.HealthCheckTrigger)]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogTrace(_telemetryClient, "Health Check Trigger Completed OK");

        return new OkObjectResult("Ok");
    }
}
