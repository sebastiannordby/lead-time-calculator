using LeadTimeCalculator.Client.Components;
using LeadTimeCalculator.Client.Data;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddServiceDiscovery();

builder.Services.AddTransient<LeadTimeApiClient>();
builder.Services.AddHttpClient<LeadTimeApiClient>(options =>
{
    if (string.IsNullOrWhiteSpace(builder.Configuration["API_URI"]))
        throw new ArgumentNullException(
            "Configuration: a value must be provided API_URI");

    options.BaseAddress = new Uri(
        builder.Configuration["API_URI"]!);
}).AddServiceDiscovery();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
