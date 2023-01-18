using Consul;
using Kwetter.KweetService.Data;
using Kwetter.KweetService.DataService;
using Kwetter.KweetService.Events;
using Kwetter.KweetService.Messaging;
using Kwetter.KweetService.Repository;
using Kwetter.KweetService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var client = new ConsulClient();
var registration = new AgentServiceRegistration()
{
    ID = "kweetservice",
    Name = "Kwetter Kweet Service",
    Address = "http://localhost",
    Port = 5105,
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
builder.Services.AddScoped<IKweetRepository, KweetRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IMessageProducer, MessageProducer>();

builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

//Database configuration
builder.Services.AddDbContext<DataContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Kwetter"));
    });

//builder.Services.AddDbContext<DataContext>(
//    options =>
//    {
//        options.UseInMemoryDatabase("KwetterInMemory");
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

client.Agent.ServiceDeregister("kweetservice").Wait();
