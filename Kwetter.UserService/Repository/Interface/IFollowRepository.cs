using Kwetter.UserService.Model;

namespace Kwetter.UserService.Repository.Interface
{
    public interface IFollowRepository
    {
        Task<Follow> GetFollowDetailsById(int followId);
        Task<bool> IsFollowing(Guid userId, Guid followingUser);
        Task<bool> UnfollowByUserGuid(Guid userId, Guid followingUser);
        Task<List<Follow>> GetFollowingByUser(Guid userId);
        Task<List<Follow>> GetFollowersByUser(Guid userId);
        Task<bool> FollowUser(Follow follow);
        Task<bool> UnfollowUserById(int followId);
        Task<bool> Save();
    }
}
