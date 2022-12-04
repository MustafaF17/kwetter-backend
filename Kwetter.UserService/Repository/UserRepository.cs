using Kwetter.UserService.Data;
using Kwetter.UserService.Model;
using Kwetter.UserService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.UserService.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            return await Save();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            User user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                return await Save();
            }

            return false;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(p => p.Username == username);
            return user;
        }

        public async Task<bool> Save()
        {
            int saved = await _context.SaveChangesAsync();
            if (saved > 0) return true;
            return false;
        }

       
    }
}
