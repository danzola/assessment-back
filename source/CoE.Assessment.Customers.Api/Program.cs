using CoE.Assessment.Customers.Application.Interfaces;
using CoE.Assessment.Customers.Application.Mapper;
using CoE.Assessment.Customers.Application.Services;
using CoE.Assessment.Customers.Data.Contexts;
using CoE.Assessment.Customers.Data.Repositories;
using CoE.Assessment.Customers.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
//Application Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
//Data
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddDbContext<CustomersDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("CustomersDbConnection"));
});

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

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

// GET /api/customers/{id}
app.MapGet("/customers/{id}", async (int id, ICustomerService customerService) =>
{
    var customer = await customerService.GetByIdAsync(id);
    if (customer == null)
    {
        return Results.NotFound(new { Message = $"Customer with ID {id} not found." });
    }
    return Results.Ok(customer);
});

app.Run();