using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Infrastructure.IoC;
using CoE.Assessment.Products.Application.Interfaces;
using CoE.Assessment.Products.Application.Mapper;
using CoE.Assessment.Products.Application.Services;
using CoE.Assessment.Products.Data.Contexts;
using CoE.Assessment.Products.Data.Repositories;
using CoE.Assessment.Products.Domain.CommandHandlers;
using CoE.Assessment.Products.Domain.Commands;
using CoE.Assessment.Products.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddMicroRabbitServices();
//Subscriptions
builder.Services.AddTransient<VerifyProductCommandHandler>();
//Bus handlers
builder.Services.AddTransient<ICommandHandler<VerifyProductCommand>, VerifyProductCommandHandler>();
//Application Services
builder.Services.AddScoped<IProductService, ProductService>();
//Data
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("ProductsDbConnection"));
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

// GET /api/products
app.MapGet("/products", async (IProductService productService) =>
{
    var products = await productService.GetAll();    
    return Results.Ok(products);
});

// GET /api/products/{id}
app.MapGet("/products/{id}", async (int id, IProductService productService) =>
{
    var product = await productService.GetByIdAsync(id);
    if (product == null)
    {
        return Results.NotFound(new { Message = $"Product with ID {id} not found." });
    }
    return Results.Ok(product);
});

var commandBus = app.Services.GetRequiredService<ICommandBus>();
await commandBus.SubscribeCommand<VerifyProductCommand, VerifyProductCommandHandler>();

app.Run();