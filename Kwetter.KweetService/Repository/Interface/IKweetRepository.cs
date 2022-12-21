using Kwetter.KweetService.Model;

namespace Kwetter.KweetService.Repository.Interface
{
    public interface IKweetRepository
    {

        Task<Kweet> GetKweetById(int kweetId);
        Task<List<Kweet>> GetAllKweets();
        Task<List<Kweet>> GetFollowingKweets(Guid userId);
        Task<List<Kweet>> GetKweetsByUser(Guid userId);      
        Task<bool> CreateKweet(Kweet kweet);
        Task<bool> DeleteKweet(int kweetId);

        Task<bool> Save();
    }
}
