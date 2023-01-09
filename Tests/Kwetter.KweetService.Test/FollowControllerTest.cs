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
    public class FollowControllerTest
    {

        #region Private declarations

        private readonly FollowController _followController;
        private readonly DataContext _context;

        #endregion



        public FollowControllerTest()
        {
            ServiceProvider serviceProvider = BuildContainer();

            _context = serviceProvider.GetService<DataContext>();

            _followController = new FollowController(serviceProvider.GetService<IFollowRepository>());
        }

        #region Constructors



        #endregion


        #region Test methods

        [Fact]
        public async Task GetFollowing_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            _context.Add(new Follow { Id = 1, FollowingUserId = Guid.NewGuid(), UserId = userId });
            _context.Add(new Follow { Id = 2, FollowingUserId = Guid.NewGuid(), UserId = userId });
            _context.Add(new Follow { Id = 3, FollowingUserId = Guid.NewGuid(), UserId = userId });


            await _context.SaveChangesAsync();

            // Act
            var result = await _followController.GetFollowing(userId) as ObjectResult;


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Follow>>(result.Value);


        }


        [Fact]
        public async Task GetFollowers_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            _context.Add(new Follow { Id = 1, FollowingUserId = userId, UserId = Guid.NewGuid() });
            _context.Add(new Follow { Id = 2, FollowingUserId = userId, UserId = Guid.NewGuid() });
            _context.Add(new Follow { Id = 3, FollowingUserId = userId, UserId = Guid.NewGuid() });
            await _context.SaveChangesAsync();

            // Act
            var result = await _followController.GetFollowers(userId) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Follow>>(result.Value);

        }

        [Fact]
        public async Task CreateFollow_ShouldReturn200Status()
        {
            // Arrange
            string followUserGuid = "3e2ebf40-4c2e-491c-95bc-0fcb4669bceb";

            Guid userId = Guid.NewGuid();
            Guid followUserId = new Guid(followUserGuid);

            _followController.ControllerContext.HttpContext = new DefaultHttpContext();
            _followController.ControllerContext.HttpContext.Request.Headers["claims_id"] = userId.ToString();

            // Act
            var result = await _followController.Follow(followUserId) as ObjectResult;
            var model = result.Value as Follow;
            var actual = model.FollowingUserId;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Follow>(result.Value);
            Assert.Equal(followUserId, actual);

        }

        [Fact]
        public async Task DeleteFollow_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid followUserId = Guid.NewGuid();
            _followController.ControllerContext.HttpContext = new DefaultHttpContext();
            _followController.ControllerContext.HttpContext.Request.Headers["claims_id"] = userId.ToString();

            _context.Add(new Follow { Id = 1, FollowingUserId = followUserId, UserId = userId});
            await _context.SaveChangesAsync();

            // Act
            var result = await _followController.Unfollow(1) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)result.Value);

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
            services.AddScoped<IFollowRepository, FollowRepository>();

            services.AddScoped(_ => contextOptions);

            return services.BuildServiceProvider();
        }

        #endregion
    }
}