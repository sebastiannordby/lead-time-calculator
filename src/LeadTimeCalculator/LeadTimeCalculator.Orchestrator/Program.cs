var builder = DistributedApplication.CreateBuilder(args);

var api = builder
    .AddProject<Projects.LeadTimeCalculator_API>("leadtimecalculator-api");

var client = builder
    .AddProject<Projects.LeadTimeCalculator_Client>("leadtimecalculator-client")
    .WithReference(api)
    .WithEnvironment("API_URI", "https://leadtimecalculator-api");

builder.Build().Run();
