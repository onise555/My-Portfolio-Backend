using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Asp.requests.Profile;
using Portfolio.Asp.Services.ProfilleSer;
using Portfolio.Asp.Services.UserSer;

namespace Portfolio.Asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _service;


        public ProfileController(IProfileService service)
        {
          _service = service; 
        }

        [HttpPost("Create-Profile")]
        public async Task<ActionResult> Create(CreateProfilerequest request)
        {
            await _service.Create(request);

            return Ok(request);


        }

    }
}
