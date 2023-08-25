using Microsoft.EntityFrameworkCore;
using ProductWebApi.Context;
using JwtAuthManager;
using ProductWebApi.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using ProductWebApi.Services.Producer;

var builder = WebApplication.CreateBuilder(args);

//Kafka services Producers
var producerConfig = new ProducerConfig();
builder.Configuration.Bind("producer", producerConfig);

builder.Services.AddSingleton<ProducerConfig>(producerConfig);

// Add services to the container.
builder.Services.AddScoped<IProductProducer, ProductProducer>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();

//Register DB

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
