using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tiki_shop.Models.Request;
using tiki_shop.Services;

namespace tiki_shop.Controllers
{
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
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetUser()
        {
            var res = await _userServices.GetUser();
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        [HttpGet("byId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {
            var res = await _userServices.GetUserById(id);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var res = await _userServices.GetAllUsers();
            return Ok(res);
        }
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            var res = await _userServices.ChangePassword(req.PhoneNumber, req.OldPassword, req.NewPassword);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
    }
}
