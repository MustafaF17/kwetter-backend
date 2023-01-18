using Kwetter.KweetService.Controllers;
using Kwetter.KweetService.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Kwetter.KweetService.Repository.Interface;
using Kwetter.KweetService.Repository;
using Kwetter.KweetService.Messaging;
using Microsoft.AspNetCore.Mvc;
using Kwetter.KweetService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kwetter.KweetService.Test
{
    public class KweetControllerTest
    {

        #region Private declarations

        private readonly KweetController _kweetController;
        private readonly DataContext _context;

        #endregion

        #region Constructors

        public KweetControllerTest()
        {
            ServiceProvider serviceProvider = BuildContainer();

            _context = serviceProvider.GetService<DataContext>();

            _kweetController = new KweetController(serviceProvider.GetService<IKweetRepository>());
        }


        #endregion


        #region Test methods

        [Fact]
        public async Task GetKweets_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            _context.Add(new Kweet { Id = 1, Text = "Test1", UserId = userId, Username = "User1" });
            _context.Add(new Kweet { Id = 2, Text = "Test2", UserId = userId, Username = "User2" });
            await _context.SaveChangesAsync();


            // Act
            var result = await _kweetController.GetFeed() as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Kweet>>(result.Value);

        }


        [Fact]
        public async Task GetFollowingFeed()
        {


            // Arrange
            Guid userId = Guid.NewGuid();
            Guid followsUser = Guid.NewGuid();
            Guid randomUser = Guid.NewGuid();

            _kweetController.ControllerContext.HttpContext = new DefaultHttpContext();
            _kweetController.ControllerContext.HttpContext.Request.Headers["claims_id"] = userId.ToString();

            _context.Follows.Add(new Follow { Id = 1, FollowingUserId = followsUser, UserId = userId});
            _context.Kweets.Add(new Kweet { Id = 1, Text = "Random", UserId = randomUser, Username = "RandomUser" });
            _context.Kweets.Add(new Kweet { Id = 2, Text = "Following", UserId = followsUser, Username = "FollowingUser"});
            await _context.SaveChangesAsync();

            // Act
            var result = await _kweetController.FollowingFeed() as ObjectResult;
            var feed = result.Value as List<Kweet>;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Kweet>>(result.Value);
            Assert.Equal(feed.First().Text,"Following");

        }

        #endregion

        #region Private methods

        private ServiceProvider BuildContainer()
        {
            // Setup microsoft dependency injection.
            ServiceCollection services = new();

            DbContextOptions<DataContext> contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            services.AddEntityFrameworkInMemoryDatabase();
            services.AddDbContext<DataContext>(ServiceLifetime.Transient);
            services.AddScoped<IKweetRepository, KweetRepository>();
            services.AddScoped<IFollowRepository, FollowRepository>();

            services.AddScoped(_ => contextOptions);

            return services.BuildServiceProvider();
        }

        #endregion
    }
}