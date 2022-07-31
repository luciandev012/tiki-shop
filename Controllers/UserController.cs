using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tiki_shop.Models.Request;
using tiki_shop.Services;

namespace tiki_shop.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        public UserController(IUserService userServices)
        {
            _userServices = userServices;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            var res = await _userServices.AddUser(request);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var res = await _userServices.Login(request.PhoneNumber, request.Password);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);   
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var res = await _userServices.GetAllUsers();
            return Ok(res);
        }
    }
}
