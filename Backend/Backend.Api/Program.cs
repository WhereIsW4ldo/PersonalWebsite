using Backend.Api.Middleware;
using Database;
using Login;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

const string allowedOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var logger = new LoggerConfiguration()
    .WriteTo.GrafanaLoki("http://localhost:3100", new []
    {
        new LokiLabel
        {
            Key = "Service",
            Value = "Backend.Api"
        }
    })
    .CreateLogger();

logger.Information("Program started");

builder.Host.UseSerilog(logger);

builder.Logging.ClearProviders();

// Add services to the container
builder.Services.AddControllers(o =>
{
    o.UseRoutePrefix("api");
});

builder.Services.AddDbContext<UserContext>();

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

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors(allowedOrigins);

app.Run();