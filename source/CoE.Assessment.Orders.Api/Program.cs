using System.ComponentModel.DataAnnotations;
using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Infrastructure.IoC;
using CoE.Assessment.Orders.Application.Interfaces;
using CoE.Assessment.Orders.Application.Models;
using CoE.Assessment.Orders.Application.Services;
using CoE.Assessment.Orders.Data.Contexts;
using CoE.Assessment.Orders.Data.Repositories;
using CoE.Assessment.Orders.Domain.CommandHandlers;
using CoE.Assessment.Orders.Domain.Commands;
using CoE.Assessment.Orders.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddMicroRabbitServices();
//Subscriptions
builder.Services.AddTransient<CreateOrderCommandHandler>();
//Bus handlers
builder.Services.AddTransient<ICommandHandler<CreateOrderCommand>, CreateOrderCommandHandler>();
//Application Services
builder.Services.AddTransient<IOrderService, OrderService>();
//Data
builder.Services.AddTransient<IOrdersRepository, OrdersRepository>();
builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("OrdersDbConnection"));
});


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

var commandBus = app.Services.GetRequiredService<ICommandBus>();
await commandBus.SubscribeCommand<CreateOrderCommand, CreateOrderCommandHandler>();

app.Run();
