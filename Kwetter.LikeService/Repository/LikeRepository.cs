using Kwetter.LikeService.Data;
using Kwetter.LikeService.Model;
using Kwetter.LikeService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.LikeService.Repository
{
    public class LikeRepository : ILikeRepository
    {

        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateLike(Like like)
        {
            await _context.AddAsync(like);
            return await Save();
        }

        public async Task<bool> DeleteByKweetId(int kweetId)
        {
            var likes = _context.Likes.Where(l => l.KweetId == kweetId);
            _context.Likes.RemoveRange(likes);
            return await Save();
        }

        public async Task<bool> DeleteLike(int likeId)
        {
            Like like = _context.Likes.FirstOrDefault(x => x.Id == likeId);
            if (like != null)
            {
                _context.Likes.Remove(like);
                return await Save();
            }

            return false;

        }

        public async Task<Like> GetLikeById(int likeId)
        {
            return await _context.Likes.FirstOrDefaultAsync(p => p.Id == likeId);
        }

        public async Task<List<Like>> GetLikesByUser(Guid userId)
        {
            return await _context.Likes.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<bool> Save()
        {
            int saved = await _context.SaveChangesAsync();
            if (saved > 0) return true;
            return false;
        }
    }
}
