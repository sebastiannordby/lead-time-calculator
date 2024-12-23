using LeadTimeCalculator.Production.Application;
using LeadTimeCalculator.Production.Infrastructure;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metricsBuilder =>
    {
        metricsBuilder
            .SetResourceBuilder(
                ResourceBuilder
                .CreateDefault()
                .AddService("LeadTimeCalculator"))
            .AddAspNetCoreInstrumentation() // Tracks request counts, durations, etc.
            .AddPrometheusExporter(); // Tracks HTTP request/response metrics
    });


builder.Services.AddProductionInfrastructure();
builder.Services.AddProductionApplicationFeature();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

var apiEndpointGroup = app
    .MapGroup("api")
    .WithOpenApi();

apiEndpointGroup.AddProductionEndpoints();
app.MapGet("/", () => TypedResults.Redirect("/scalar/v1"));
app.Run();

public partial class Program { }