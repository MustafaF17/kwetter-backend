using Kwetter.KweetService.Data;
using Kwetter.KweetService.Model;
using Kwetter.KweetService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.KweetService.Repository
{
    public class KweetRepository : IKweetRepository
    {

        private readonly DataContext _context;

        public KweetRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateKweet(Kweet kweet)
        {
            await _context.Kweets.AddAsync(kweet);
            return await Save();
        }

        public async Task<bool> DeleteKweet(int kweetId)
        {
            Kweet kweet = _context.Kweets.FirstOrDefault(x => x.Id == kweetId);
            if (kweet != null)
            {
                _context.Kweets.Remove(kweet);
                return await Save();
            }

            return false;

        }


        public async Task<List<Kweet>> GetAllKweets()
        {
            return await _context.Kweets.OrderBy(p => p.Created).ToListAsync();
        }


        public async Task<List<Kweet>> GetFollowingKweets(Guid userId)
        {
            var followingUsers = await _context.Follows.Where(p => p.UserId == userId).ToListAsync();
            var kweets = new List<Kweet>();

            foreach (var item in followingUsers)
            {
                var tweets = await _context.Kweets.Where(x => x.UserId == item.FollowingUserId).ToListAsync();
                kweets.AddRange(tweets);
            }

            return kweets;
        }


        public async Task<Kweet> GetKweetById(int kweetId)
        {
            return await _context.Kweets.FirstOrDefaultAsync(p => p.Id == kweetId);
        }

        public async Task<List<Kweet>> GetKweetsByUser(Guid userId)
        {
            return await _context.Kweets.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<bool> Save()
        {
            int saved = await _context.SaveChangesAsync();
            if (saved > 0) return true;
            return false;
        }
    }
}
