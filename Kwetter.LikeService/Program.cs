using Consul;
using Kwetter.LikeService.Data;
using Kwetter.LikeService.DataService;
using Kwetter.LikeService.Events;
using Kwetter.LikeService.Repository;
using Kwetter.LikeService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var client = new ConsulClient();
var registration = new AgentServiceRegistration()
{
    ID = "likeservice",
    Name = "Kwetter Like Service",
    Address = "http://localhost",
    Port = 5019,
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
builder.Services.AddScoped<ILikeRepository, LikeRepository>();

builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor,EventProcessor>();


// Database configuration
builder.Services.AddDbContext<DataContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Kwetter"));
    });

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

client.Agent.ServiceDeregister("likeservice").Wait();
