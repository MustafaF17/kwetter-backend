using Kwetter.LikeService.Model;

namespace Kwetter.LikeService.Repository.Interface

{
    public interface ILikeRepository
    {
        Task<Like> GetLikeById(int likeId);
        Task<List<Like>> GetLikesByUser(Guid userId);
        Task<bool> CreateLike(Like like);
        Task<bool> DeleteLike(int likeId);
        Task<bool> DeleteByKweetId(int kweetId);
        Task<bool> Save();
    }
}
