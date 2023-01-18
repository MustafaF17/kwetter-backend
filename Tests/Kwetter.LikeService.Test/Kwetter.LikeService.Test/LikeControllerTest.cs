using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Kwetter.LikeService.Data;
using Kwetter.LikeService.Controllers;
using Kwetter.LikeService.Repository.Interface;
using Kwetter.LikeService.Repository;
using Kwetter.LikeService.Model;

namespace Kwetter.LikeService.Test
{
    public class LikeControllerTest
    {

        #region Private declarations

        private readonly LikeController _likeController;
        private readonly DataContext _context;

        #endregion

        #region Constructors

        public LikeControllerTest()
        {
            ServiceProvider serviceProvider = BuildContainer();

            _context = serviceProvider.GetService<DataContext>();

            _likeController = new LikeController(serviceProvider.GetService<ILikeRepository>());
        }


        #endregion


        #region Test methods

        [Fact]
        public async Task GetLikeById_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            _context.Add(new Like { Id = 1, KweetId = 1, UserId = userId });
            _context.Add(new Like { Id = 2, KweetId = 2, UserId = userId });
            _context.Add(new Like { Id = 3, KweetId = 3, UserId = userId });
            await _context.SaveChangesAsync();


            // Act
            var result = await _likeController.GetLikeById(2) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Like>(result.Value);

        }

        [Fact]
        public async Task GetUsersLikes_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid random = Guid.NewGuid();

            _likeController.ControllerContext.HttpContext = new DefaultHttpContext();
            _likeController.ControllerContext.HttpContext.Request.Headers["claims_id"] = userId.ToString();

            _context.Add(new Like { Id = 1, KweetId = 1, UserId = userId });
            _context.Add(new Like { Id = 2, KweetId = 2, UserId = userId });
            _context.Add(new Like { Id = 3, KweetId = 3, UserId = random });
            await _context.SaveChangesAsync();


            // Act

            var result = await _likeController.GetUserLikes() as ObjectResult;
            var model = result.Value as List<Like>;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Like>>(result.Value);
            Assert.Equal(model.Count, 2);

        }

        [Fact]
        public async Task CreateLike_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            _likeController.ControllerContext.HttpContext = new DefaultHttpContext();
            _likeController.ControllerContext.HttpContext.Request.Headers["claims_id"] = userId.ToString();



            // Act

            var result = await _likeController.CreateLike(1) as ObjectResult;
            var model = result.Value as Like;
            var actual = model.KweetId;


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Like>(result.Value);
            Assert.Equal(1, actual);

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
            services.AddScoped<ILikeRepository, LikeRepository>();

            services.AddScoped(_ => contextOptions);

            return services.BuildServiceProvider();
        }

        #endregion
    }
}