using CoE.Assessment.Domain.Events;
using CoE.Assessment.Infrastructure.IoC;
using CoE.Assessment.Logs.Domain.EventHandlers;
using CoE.Assessment.Logs.Domain.Events;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/assessment.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddMicroRabbitServices();
//Subscriptions
builder.Services.AddTransient<LogErrorEventHandler>();
//Bus handlers
builder.Services.AddTransient<IEventHandler<LogErrorEvent>, LogErrorEventHandler>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/test", () =>
{
    Log.Information("Test log message");
});

var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.SubscribeEvent<LogErrorEvent, LogErrorEventHandler>();

app.Run();