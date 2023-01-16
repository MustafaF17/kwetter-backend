using Kwetter.KweetService.Data;
using Kwetter.KweetService.Dto;
using Kwetter.KweetService.Model;
using Kwetter.KweetService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.KweetService.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(UserDto userDto)
        {
            await _context.UserKweet.AddAsync(userDto);
            return await Save();
        }

        public async Task<bool> Save()
        {
            int saved = await _context.SaveChangesAsync();
            if (saved > 0) return true;
            return false;
        }
    }
}
