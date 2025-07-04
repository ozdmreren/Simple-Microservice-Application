using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using StockAPI.Consumer;
using StockAPI.Data;
using StockAPI.Interfaces;
using StockAPI.Models;
using StockAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Redis Configuration
var redisConnection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("REDIS"));
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

// Masstransit Configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreatedOrderEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("YOUR RABBIT MQ URI");
        cfg.ReceiveEndpoint("order-created-event", e =>
        {
            e.ConfigureConsumer<CreatedOrderEventConsumer>(context);
        });
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StockDbContext>(options =>
{
    options.UseMongoDB("mongodb://localhost:27017/", "YOUR DB NAME");
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/add-data", async (StockDbContext context,IRedisCacheService redisCacheService) =>
{
    var entityEntry  =  context.Products.Add(
       new Product()
       {
           ProductName = "Bilgisayar",
           Quantity = 500,
           Brand = "Monster",
           UnitPrice = 17_500f,
           Category = new Category() { CategoryName = "Techonlogy", Description = "Müq" }
       }
   );

    var product = entityEntry.Entity;

    await redisCacheService.SetProductAsync(product.Id.ToString(),JsonSerializer.Serialize<Product>(product));

    context.SaveChanges();
});

app.MapGet("/get-data/{id}", async (StockDbContext context,IRedisCacheService redisCacheService,string id) =>
{
    Guid productId = Guid.Parse(id);
    string key = productId.ToString();
    string? productString = await redisCacheService.GetProductAsync(key);
    if (!String.IsNullOrEmpty(productString))
    {
        Product? deserializedProduct = JsonSerializer.Deserialize<Product>(productString);
        return deserializedProduct;
    }

    else
    {
        Product? product = context.Products.Find(productId);
        //Console.WriteLine(JsonSerializer.Serialize<Product>(product));
        //Console.WriteLine(JsonSerializer.Deserialize<Product>(JsonSerializer.Serialize<Product>(product)));
        if (product is not null)
        {
            await redisCacheService.SetProductAsync(key,JsonSerializer.Serialize<Product>(product));
            return product;
        }
        else
        {
            throw new Exception("PRODUCT NULL");
        }
    }
});

app.Run();

