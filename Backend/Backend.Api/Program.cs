using Backend.Api.Middleware;
using Database;
using Login;
using Login.Services;
using Microsoft.EntityFrameworkCore;
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
        x.AddConsoleExporter();
    });
}

// Add services to the container
builder.Services.AddControllers(o =>
{
    o.UseRoutePrefix("api");
});

var connectionString = builder.Configuration.GetConnectionString("Database")
    ?? throw new InvalidOperationException("Connection string 'Database' not found.");

builder.Services.AddDbContext<UserContext>(options => 
    options.UseNpgsql(connectionString));

builder.Services.ConfigureLoginServices();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();