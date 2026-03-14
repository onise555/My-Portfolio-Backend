using Amazon.Runtime;
using Portfolio.Asp.Models.Users;
using Portfolio.Asp.Repositories;
using Portfolio.Asp.requests.Profile;

namespace Portfolio.Asp.Services.ProfilleSer
{
    public class ProfileService
    {
        private readonly IRepository<Profile> _repo;

        public ProfileService(IRepository<Profile> repo)
        {
            _repo = repo;
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

    }
}
