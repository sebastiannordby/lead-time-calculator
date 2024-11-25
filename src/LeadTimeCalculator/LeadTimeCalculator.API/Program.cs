using LeadTimeCalculator.API.Features.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();
builder.Services.AddWorkdayCalendarFeature();

var app = builder.Build();

// Configure the HTTP request pipeline.
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