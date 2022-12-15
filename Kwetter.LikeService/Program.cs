using Kwetter.LikeService.Data;
using Kwetter.LikeService.DataService;
using Kwetter.LikeService.Events;
using Kwetter.LikeService.Repository;
using Kwetter.LikeService.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Services dependency
builder.Services.AddScoped<ILikeRepository, LikeRepository>();

builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor,EventProcessor>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Database configuration
builder.Services.AddDbContext<DataContext>(
    options =>
    {
        options.UseInMemoryDatabase("LikeInMemory");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
