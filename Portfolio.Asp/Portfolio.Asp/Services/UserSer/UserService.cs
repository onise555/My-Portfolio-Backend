using Portfolio.Asp.DTOS.User;
using Portfolio.Asp.Models.Users;
using Portfolio.Asp.Repositories;
using Portfolio.Asp.requests.User;

namespace Portfolio.Asp.Services.UserSer
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repo;
        private readonly S3Service _s3;

        public UserService(IRepository<User> repo, S3Service s3)
        {
            _repo = repo;
            _s3 = s3;
        }

        public async Task Create(CreateUserRequest request)
        {
            var imageUrl = await _s3.UploadFileAsync(request.ProfileImage, "users/images");
            var videoUrl = await _s3.UploadFileAsync(request.ProfileVideo, "users/videos");

            var user = new User
            {
                FullName = request.FullName,
                ProfileImage = imageUrl,
                ProfileVideo = videoUrl
            };
            await _repo.AddAsync(user);
        }

        public async Task Update(UpdateUserRequest request)
        {
            var user = await _repo.GetByIdAsync(request.Id);
            if (user == null) return;

            user.FullName = request.FullName;

            if (request.ProfileImage != null)
                user.ProfileImage = await _s3.UploadFileAsync(request.ProfileImage, "users/images");

            if (request.ProfileVideo != null)
                user.ProfileVideo = await _s3.UploadFileAsync(request.ProfileVideo, "users/videos");

            await _repo.UpdateAsync(user);
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
            return user == null ? null : new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                ProfileImage = user.ProfileImage,
                ProfileVideo = user.ProfileVideo
            };
        }

        public async Task Delete(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user != null) await _repo.DeleteAsync(user);
        }
    }
}