using Consul;
using Kwetter.UserService.Data;
using Kwetter.UserService.Messaging;
using Kwetter.UserService.Repository;
using Kwetter.UserService.Repository.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var client = new ConsulClient();
var registration = new AgentServiceRegistration()
{
    ID = "userservice",
    Name = "Kwetter User Service",
    Address = "http://localhost",
    Port = 5079,
    Check = new AgentServiceCheck()
    {
        HTTP = "http://localhost:5019/health",
        Interval = TimeSpan.FromSeconds(30)
    }
};

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services dependency
builder.Services.AddScoped<IUserRepository, UserRepository>();
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

builder.Services.Configure<IdentityOptions>(options =>
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

var app = builder.Build();

// Populate database
//PrepDb.PrepPopulation(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

client.Agent.ServiceDeregister("userservice").Wait();
