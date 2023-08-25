using Microsoft.EntityFrameworkCore;
using OrderWebApi.Context;
using JwtAuthManager;
using OrderWebApi.Services;
using Confluent.Kafka;
using OrderWebApi.Services.KafkaConsumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();

//kafka consumer
builder.Services.AddSingleton
    <IHostedService, KafkaConsumerService>();

//Register DB

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

//enabling Legacy Timestamp Behavior
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
