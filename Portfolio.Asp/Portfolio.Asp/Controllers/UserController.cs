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

        [HttpDelete("Delete-User/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.GetById(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            await _service.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }

    }
}