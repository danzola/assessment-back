using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Infrastructure.IoC;
using CoE.Assessment.Products.Domain.CommandHandlers;
using CoE.Assessment.Products.Domain.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMicroRabbitServices();
//Subscriptions
builder.Services.AddTransient<VerifyProductCommandHandler>();
//Bus handlers
builder.Services.AddTransient<ICommandHandler<VerifyProductCommand>, VerifyProductCommandHandler>();

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

var commandBus = app.Services.GetRequiredService<ICommandBus>();
await commandBus.SubscribeCommand<VerifyProductCommand, VerifyProductCommandHandler>();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
