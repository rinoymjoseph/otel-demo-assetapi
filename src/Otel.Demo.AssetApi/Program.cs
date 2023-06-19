using Otel.Demo.AssetApi;
using Otel.Demo.AssetApi.Services;
using Otel.Demo.AssetApi.Services.Interfaces;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITelemetryService, TelemetryService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IUserService, UserService>();

string otel_exporter_url= builder.Configuration.GetValue<string>(AppConstants.URL_OTEL_EXPORTER);

builder.Services
    .AddLogging((loggingBuilder) => loggingBuilder
    .AddOpenTelemetry(options =>
        options
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(otel_exporter_url);
            })))
    .AddOpenTelemetry()
    .ConfigureResource(builder => builder
    .AddService(serviceName: AppConstants.OTEL_SERVCICE_NAME))
    .WithTracing(builder => builder
        .AddSource(AppConstants.OTEL_SERVCICE_NAME)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation() //Required for baggage
        .AddConsoleExporter()        
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otel_exporter_url);
        }))
    .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
        .ConfigureResource(resource => resource
        .AddService(AppConstants.OTEL_SERVCICE_NAME))
        .AddMeter(TelemetryService._meter.Name)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otel_exporter_url);
        }));
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
