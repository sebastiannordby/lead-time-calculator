using LeadTimeCalculator.API.Application.ProductionScheduleFeature;
using LeadTimeCalculator.API.Application.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Infrastructure;
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


builder.Services.AddInfrastructure();
builder.Services.AddWorkdayCalendarApplicationFeature();
builder.Services.AddProductionScheduleApplicationFeature();

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

apiEndpointGroup.AddWorkdayCalendarFeatureEndpoints();
apiEndpointGroup.AddProductionScheduleFeatureEndpoints();
app.MapGet("/", () => TypedResults.Redirect("/scalar/v1"));
app.Run();

public partial class Program { }