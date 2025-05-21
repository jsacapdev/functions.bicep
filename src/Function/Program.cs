using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

// Connect to Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    string? appConfigConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings::AppConfiguration");

    if (string.IsNullOrEmpty(appConfigConnectionString))
    {
        throw new InvalidOperationException("The App Configuration connection string is not set in the environment variables.");
    }

    Uri endpoint = new Uri(appConfigConnectionString);

    options.Connect(endpoint, new DefaultAzureCredential())
        .Select(KeyFilter.Any, LabelFilter.Null)
            .Select(KeyFilter.Any, $"type:{Environment.GetEnvironmentVariable("Transform::Type")}")
            .Select(KeyFilter.Any, $"environment:{Environment.GetEnvironmentVariable("Transform::Environment")}");

    options.ConfigureKeyVault(kv =>
    {
        kv.SetCredential(new DefaultAzureCredential());
    });
});

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService((options) =>
    {
        string? appInsightsConnectionString = builder.Configuration.GetConnectionString("ApplicationInsights");

        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            options.ConnectionString = appInsightsConnectionString;
        }
    })
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
