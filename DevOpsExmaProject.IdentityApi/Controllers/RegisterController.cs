using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsExmaProject.IdentityApi.Controllers
{
    [Route("Register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("ok 200");
        }

    }
}
