using Backend.Api.Middleware;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

var otlpEndpointUrl = builder.Configuration["OTLP_ENDPOINT_URL"];
if (!string.IsNullOrEmpty(otlpEndpointUrl))
{
    builder.Logging.AddOpenTelemetry(x =>
    {
        x.AddOtlpExporter(a =>
        {
            a.Endpoint = new Uri(otlpEndpointUrl);
            a.Protocol = OtlpExportProtocol.HttpProtobuf;
        });
    });
}

// Add services to the container
builder.Services.AddControllers(o =>
{
    o.UseRoutePrefix("api");
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();