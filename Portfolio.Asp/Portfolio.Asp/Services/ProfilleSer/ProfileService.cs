using Amazon.Runtime;
using Portfolio.Asp.DTO_s.profile;
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

        public Task Create(CreateProfilerequest request)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
            throw new NotImplementedException();
        }

        public async Task CreateProfile(CreateProfilerequest request)
        {
            var profile = new Profile()
            {
                UserId = request.UserId,
                About = request.About,
            };

            if (profile == null)
            {
                profile = new Profile();
            }
        

            await _repo.AddAsync(profile);


        }


        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileDTO?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(UpdateProfilerequest request)
        {
            throw new NotImplementedException();
        }
    }
}
