using Amazon.Runtime;
using Portfolio.Asp.DTO_s.profile;
using Portfolio.Asp.DTOS.User;
using Portfolio.Asp.Models.Users;
using Portfolio.Asp.Repositories;
using Portfolio.Asp.requests.Profile;

namespace Portfolio.Asp.Services.ProfilleSer
{

    public class ProfileService : IProfileService

    {
        private readonly IRepository<Profile> _repo;

        public ProfileService(IRepository<Profile> repo)
        {
            _repo = repo;
        }




        public async Task Create(CreateProfilerequest request)
        {
            var profile = new Profile
            {
                UserId = request.UserId,
                About = request.About,
            };

            await _repo.AddAsync(profile);
        }

  

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<UserProfileDTO?> GetByUserId(int userId)
        {
            var profiles = await _repo.GetAllAsync();
            var profile = profiles.FirstOrDefault(p => p.UserId == userId);

            if (profile == null)
                return null;

            return new UserProfileDTO
            {
                Id = profile.Id,
                About = profile.About,
                UserId = profile.UserId
            };
        }

        public Task<UserProfileDTO?> GetById(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task Update(int userId, UpdateProfilerequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var profile = await _repo.GetByIdAsync(userId);

            if (profile == null)
                throw new KeyNotFoundException($"Profile for user '{userId}' not found.");

            profile.About = request.About;

            await _repo.UpdateAsync(profile);
        }

    }
}
