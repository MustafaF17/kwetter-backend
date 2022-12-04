using Kwetter.KweetService.Data;
using Kwetter.KweetService.Model;
using Kwetter.KweetService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.KweetService.Repository
{
    public class FollowRepository : IFollowRepository
    {

        private readonly DataContext _context;

        public FollowRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Follow> GetFollowDetailsById(int followId)
        {
            return await _context.Follows.FirstOrDefaultAsync(p => p.Id == followId);
        }

        public async Task<bool> FollowUser(Follow follow)
        {
            await _context.Follows.AddAsync(follow);
            return await Save();
        }

        public async Task<bool> UnfollowUser(int followId)
        {
            Follow followDetails = _context.Follows.FirstOrDefault(x => x.Id == followId);
            if (followDetails != null)
            {
                _context.Follows.Remove(followDetails);
                return await Save();
            }

            return false;
        }

        public async Task<List<Follow>> GetFollowersByUser(Guid userId)
        {
            return await _context.Follows.Where(p => p.FollowingUserId == userId).ToListAsync();
        }

        public async Task<List<Follow>> GetFollowingByUser(Guid userId)
        {
            return await _context.Follows.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<bool> Save()
        {
            int saved = await _context.SaveChangesAsync();
            if (saved > 0) return true;
            return false;
        }

       
    }
}
