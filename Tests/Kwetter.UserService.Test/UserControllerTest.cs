using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Kwetter.UserService.Data;
using Kwetter.UserService.Repository.Interface;
using Kwetter.UserService.Repository;
using Kwetter.UserService.Controllers;
using Kwetter.UserService.Model;

namespace Kwetter.LikeService.Test
{
    public class UserControllerTest
    {

        #region Private declarations

        private readonly UserController _userController;
        private readonly DataContext _context;

        #endregion

        #region Constructors

        public UserControllerTest()
        {
            ServiceProvider serviceProvider = BuildContainer();

            _context = serviceProvider.GetService<DataContext>();

            _userController = new UserController(serviceProvider.GetService<IUserRepository>());
        }


        #endregion


        #region Test methods

        [Fact]
        public async Task GetAllUsers_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Byte[] x = new Byte[2];

            _context.Add(new User { Id = Guid.NewGuid(), Username = "User 1" , Role = "Guest", PasswordHash = x, PasswordSalt = x });
            _context.Add(new User { Id = Guid.NewGuid(), Username = "User 2", Role = "Guest", PasswordHash = x, PasswordSalt = x });

            await _context.SaveChangesAsync();


            // Act
            var result = await _userController.GetAllUsers() as ObjectResult;
            var model = result.Value as List<User>;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<User>>(result.Value);

        }

        [Fact]
        public async Task GetUserById_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Byte[] x = new Byte[2];

            _context.Add(new User { Id = Guid.NewGuid(), Username = "User 1", Role = "Guest", PasswordHash = x, PasswordSalt = x });
            _context.Add(new User { Id = userId, Username = "User 2", Role = "Guest", PasswordHash = x, PasswordSalt = x });

            await _context.SaveChangesAsync();


            // Act
            var result = await _userController.GetUserById(userId) as ObjectResult;
            var model = result.Value as User;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<User>(result.Value);
            Assert.Equal(model.Username, "User 2");

        }

        [Fact]
        public async Task DeleteUser_ShouldReturn200Status()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Byte[] x = new Byte[2];

            _context.Add(new User { Id = userId, Username = "User 1", Role = "Guest", PasswordHash = x, PasswordSalt = x });
            await _context.SaveChangesAsync();


            // Act
            var result = await _userController.DeleteUser(userId) as ObjectResult;

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
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped(_ => contextOptions);

            return services.BuildServiceProvider();
        }

        #endregion
    }
}