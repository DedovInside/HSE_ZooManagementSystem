using Application.EventHandling;
using Application.RepositoriesInterfaces;
using Application.Services;
using Domain.Events;
using Infrastructure.BackgroundServices;
using Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Добавление контроллеров
builder.Services.AddControllers();

// Настройка Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Zoo Management API", 
        Version = "v1",
        Description = "API для управления зоопарком"
    });
    
});

// Регистрация репозиториев
builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
builder.Services.AddSingleton<IFeedingRecordRepository, InMemoryFeedingRecordRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();
builder.Services.AddSingleton<ITransferRecordRepository, InMemoryTransferRecordRepository>();

// Регистрация обработчика доменных событий
builder.Services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddSingleton<IDomainEventHandler<AnimalMovedEvent>, AnimalMovedEventHandler>();
builder.Services.AddSingleton<IDomainEventHandler<FeedingTimeEvent>, FeedingTimeEventHandler>();

// Регистрация сервисов приложения
builder.Services.AddScoped<AnimalService>();
builder.Services.AddScoped<EnclosureService>();
builder.Services.AddScoped<AnimalTransferService>();
builder.Services.AddScoped<FeedingOrganizationService>();
builder.Services.AddScoped<ZooStatisticsService>();

// Регистрация фонового сервиса для проверки кормлений
builder.Services.AddHostedService<FeedingBackgroundService>();

WebApplication app = builder.Build();

// Настройка конвейера HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo Management API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();