using System.ComponentModel.DataAnnotations;
using CoE.Assessment.Orders.Application.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapPost("/orders",(NewOrderDto newOrderDto) =>
{
    var context = new ValidationContext(newOrderDto);
    var results = new List<ValidationResult>();

    if (!Validator.TryValidateObject(newOrderDto, context, results, true))
    {
        return Results.BadRequest(results.Select(r => r.ErrorMessage));
    }

    return Results.Ok(new { Message = "Order processed", newOrderDto });
});

app.Run();
