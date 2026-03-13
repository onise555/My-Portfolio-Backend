using Microsoft.AspNetCore.Mvc;
using Portfolio.Asp.requests.User;
using Portfolio.Asp.Services.UserSer;

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
        return Created("", request);
    }

    [HttpGet("Get-All-Users")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllUser();
        return Ok(users);
    }

    [HttpGet("Get-User/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetById(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("Update-User/{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateUserRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        await _service.Update(request);

        return Ok();
    }

    [HttpDelete("User/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.Delete(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
}