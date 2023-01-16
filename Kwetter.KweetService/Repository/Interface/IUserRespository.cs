using Kwetter.KweetService.Dto;
using Kwetter.KweetService.Model;

namespace Kwetter.KweetService.Repository.Interface
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(UserDto userDto);
        Task<bool> Save();
    }
}
