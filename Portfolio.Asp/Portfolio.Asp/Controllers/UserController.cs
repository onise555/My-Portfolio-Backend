using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Asp.DTOS.User;
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


        [HttpGet("Get-All-Users")]
        public async Task<ActionResult<List<UserDTO>>> GetAll()
        {
            var users = await _service.GetAllUser();
            return Ok(users);
        }

        [HttpGet("Get-User/{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user = await _service.GetById(id);
            if (user == null)
                return NotFound($"User with id {id} not found.");
            return Ok(user);
        }

        [HttpPost("Create-User")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            await _service.Create(request);
            return Ok("User created successfully.");
        }

        [HttpPut("Update-User")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            await _service.Update(request);
            return Ok("User updated successfully.");
        }


        [HttpDelete("Delete-User/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok("User deleted successfully.");
        }
    

}
}
