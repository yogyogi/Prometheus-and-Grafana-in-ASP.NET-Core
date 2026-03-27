using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProGraTutorial.Models;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<AppCustomMeter>();
var otel = builder.Services.AddOpenTelemetry();

var greeterMeter = new Meter("OtPrGrYa.Example", "1.0.0");
var countGreetings = greeterMeter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");

// Configure OpenTelemetry Resources with the application name
otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

/*otel.WithTracing(tracing =>
{
    tracing.AddSqlClientInstrumentation(); // dotnet add package OpenTelemetry.Instrumentation.SqlClient
    tracing.AddOtlpExporter(otlpOptions =>
     {
         otlpOptions.Endpoint = new Uri("http://localhost:9090/");
     });
});*/

// Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
otel.WithMetrics(metrics => metrics
    .AddAspNetCoreInstrumentation()
    .AddMeter(AppCustomMeter.name)
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
    .AddMeter("System.Net.Http")
    .AddMeter("System.Net.NameResolution")
    .AddPrometheusExporter());

var app = builder.Build();

// Configure the Prometheus scraping endpoint
app.MapPrometheusScrapingEndpoint();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
