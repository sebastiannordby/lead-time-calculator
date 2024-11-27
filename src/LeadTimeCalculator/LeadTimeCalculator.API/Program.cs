using LeadTimeCalculator.API.Application.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();
builder.Services.AddWorkdayCalendarApplicationFeature();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var apiEndpointGroup = app
    .MapGroup("api")
    .WithOpenApi();

apiEndpointGroup.AddWorkdayCalendarFeatureEndpoints();
app.MapGet("/", () => TypedResults.Redirect("/scalar/v1"));
app.Run();

public partial class Program { }