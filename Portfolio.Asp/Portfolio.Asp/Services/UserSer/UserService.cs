using System;
using Portfolio.Asp.DTOS.User;
using Portfolio.Asp.Repositories;
using Portfolio.Asp.requests.User;
using Portfolio.Asp.Models.Users;
using UserModel = Portfolio.Asp.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;

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
            var user =new User()
            {
                FullName =request.FullName, 
                ProfileImage=request.ProfileImage,
                ProfileVideo=request.ProfileVideo,
            };
            await _repo.AddAsync(user);
            return Ok(user);

        }

        public async Task Create(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDTO>> GetAllUser()
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
