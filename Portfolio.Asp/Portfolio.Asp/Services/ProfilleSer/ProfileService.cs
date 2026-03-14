using Amazon.Runtime;
using Portfolio.Asp.DTO_s.profile;
using Portfolio.Asp.DTOS.User;
using Portfolio.Asp.Models.Users;
using Portfolio.Asp.Repositories;
using Portfolio.Asp.requests.Profile;

namespace Portfolio.Asp.Services.ProfilleSer
{
    public class ProfileService:IProfileService
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
        public async Task<UserDTO?> GetById(int id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.About,
      
            };
        }
        public Task Update(UpdateProfilerequest request)
        {
            throw new NotImplementedException();
        }
    }
}
