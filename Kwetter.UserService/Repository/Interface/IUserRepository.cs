using Kwetter.UserService.Model;

namespace Kwetter.UserService.Repository.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(Guid id);
        Task<User> GetByUsername(string username);
        Task<bool> CreateUser(User user);
        Task<bool> DeleteUser(Guid id);
        Task<bool> Save();

    }
}
