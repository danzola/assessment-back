using System.ComponentModel.DataAnnotations;
using CoE.Assessment.Infrastructure.IoC;
using CoE.Assessment.Orders.Application.Interfaces;
using CoE.Assessment.Orders.Application.Models;
using CoE.Assessment.Orders.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddMicroRabbitServices();

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

app.MapPost("/orders",async (NewOrderDto newOrderDto, IOrderService orderService) =>
{
    var context = new ValidationContext(newOrderDto);
    var results = new List<ValidationResult>();

    if (!Validator.TryValidateObject(newOrderDto, context, results, true))
    {
        return Results.BadRequest(results.Select(r => r.ErrorMessage));
    }

    await orderService.Create(newOrderDto);
    return Results.Ok(new { Message = "Order processed", newOrderDto });
});

app.Run();
