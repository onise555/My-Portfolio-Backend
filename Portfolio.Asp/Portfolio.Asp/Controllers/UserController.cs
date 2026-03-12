using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Asp.requests.User;
using Portfolio.Asp.Services.UserSer;

namespace Portfolio.Asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;



        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("Add-User")]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest request)
        {
            await _service.Create(request);

            return Ok(request);


           
        }

        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllUser();

            return Ok(users);
        }

    }
}