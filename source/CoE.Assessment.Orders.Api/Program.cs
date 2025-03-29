using System.ComponentModel.DataAnnotations;
using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Infrastructure.IoC;
using CoE.Assessment.Orders.Application.Interfaces;
using CoE.Assessment.Orders.Application.Mapper;
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
builder.Services.AddScoped<IOrderService, OrderService>();
//Data
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("OrdersDbConnection"));
});

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors("AllowAll");

// GET /api/orders/{id}
app.MapGet("/orders/{id}", async (int id, IOrderService orderService) =>
{
    var order = await orderService.GetByIdAsync(id);
    if (order == null)
    {
        return Results.NotFound(new { Message = $"Order with ID {id} not found." });
    }
    return Results.Ok(order);
});

// POST /orders
app.MapPost("/orders",async (NewOrder newOrderDto, IOrderService orderService) =>
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

// DELETE /api/orders/{id}
app.MapDelete("/orders/{id}", async (int id, IOrderService orderService) =>
{
    await orderService.DeleteByIdAsync(id);    
    return Results.Ok();
});

// PUT /api/orders/{id}
app.MapPut("/orders/{id}", async (int id, UpdateOrder updateOrderDto, IOrderService orderService) =>
{
    var context = new ValidationContext(updateOrderDto);
    var results = new List<ValidationResult>();

    if (!Validator.TryValidateObject(updateOrderDto, context, results, true))
    {
        return Results.BadRequest(results.Select(r => r.ErrorMessage));
    }

    var order = await orderService.Update(id, updateOrderDto);
    return Results.Ok(order);
});

var commandBus = app.Services.GetRequiredService<ICommandBus>();
await commandBus.SubscribeCommand<CreateOrderCommand, CreateOrderCommandHandler>();

app.Run();
