using Kwetter.KweetService.Model;

namespace Kwetter.KweetService.Repository.Interface
{
    public interface IFollowRepository
    {
        Task<Follow> GetFollowDetailsById(int followId);
        Task<List<Follow>> GetFollowingByUser(Guid userId);
        Task<List<Follow>> GetFollowersByUser(Guid userId);
        Task<bool> FollowUser(Follow follow);
        Task<bool> UnfollowUser(int followId);
        Task<bool> Save();
    }
}
