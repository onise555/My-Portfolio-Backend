using Portfolio.Asp.DTOS.User;
using Portfolio.Asp.Models.Users;
using Portfolio.Asp.Repositories;
using Portfolio.Asp.requests.User;

namespace Portfolio.Asp.Services.UserSer
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repo;

        public UserService(IRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task Create(CreateUserRequest request)
        {
            var user = new User
            {
                FullName = request.FullName,
                ProfileImage = request.ProfileImage,
                ProfileVideo = request.ProfileVideo,
            };

            await _repo.AddAsync(user);
        }

        public async Task<List<UserDTO>> GetAllUser()
        {
            var users = await _repo.GetAllAsync();

            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                ProfileImage = u.ProfileImage,
                ProfileVideo = u.ProfileVideo
            }).ToList();
        }

        public async Task<UserDTO?> GetById(int id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                ProfileImage = user.ProfileImage,
                ProfileVideo = user.ProfileVideo
            };
        }

        public async Task Update(UpdateUserRequest request)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user == null)
                return;

            user.FullName = request.FullName;
            user.ProfileImage = request.ProfileImage;
            user.ProfileVideo = request.ProfileVideo;

            await _repo.UpdateAsync(user);
        }

        public async Task Delete(int id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null)
                return;

            await _repo.DeleteAsync(user);
        }
    }
}