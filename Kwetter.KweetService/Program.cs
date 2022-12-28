using Kwetter.KweetService.Data;
using Kwetter.KweetService.Messaging;
using Kwetter.KweetService.Repository;
using Kwetter.KweetService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services dependency
builder.Services.AddScoped<IKweetRepository, KweetRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IMessageProducer, MessageProducer>();

//Database configuration
builder.Services.AddDbContext<DataContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Kwetter"));
    });

//builder.Services.AddDbContext<DataContext>(
//    options =>
//    {
//        options.UseInMemoryDatabase("UserInMemory");
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
