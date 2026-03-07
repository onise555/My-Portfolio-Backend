using Portfolio.Asp.DTOS.User;
using Portfolio.Asp.requests.User;

namespace Portfolio.Asp.Services.UserSer
{
    public interface IUserService
    {
        Task Create(CreateUserRequest request);
        Task Update(UpdateUserRequest request);
        Task Delete(int id);
        Task<UserDTO?> GetById(int id);
        Task<List<UserDTO>> GetAllUser();
    }
}