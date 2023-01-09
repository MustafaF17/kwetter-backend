using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;
using Ocelot.Values;
using System.Text;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder
                            .AllowAnyOrigin() // specifying all origin
                            .AllowAnyMethod() // defining the allowed HTTP methods
                            .AllowAnyHeader(); // allowing any header to be sent
                      });
});

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot-local.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication")),
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false 

        };

    });

builder.Services.AddOcelot(builder.Configuration).AddCacheManager(settings => settings.WithDictionaryHandle()).AddKubernetes();


var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

await app.UseOcelot();

app.Run();
