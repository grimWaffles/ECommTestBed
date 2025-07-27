using API_Gateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserServiceGrpc;

namespace API_Gateway.Controllers
{
    [ApiController]
    [Route("api/users")]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceClient _userServiceClient;
        public UserController(IUserServiceClient userService)
        {
            _userServiceClient = userService;
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> TestUserService()
        {
            var response = await _userServiceClient.TestServiceAsync();
            return StatusCode(StatusCodes.Status200OK, new { message = response });
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> LoginUser([FromForm] string username, [FromForm] string password)
        {
            UserLoginResponse response = await _userServiceClient.LoginUserAsync(username, password);
            if (response == null || response.UserId == 0)
            {
                return Unauthorized();
            }

            return StatusCode(StatusCodes.Status200OK, new { data = response, message = "Login Successful" });
        }

        [HttpGet]
        [Route("get-all-users")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            List<CreateUserRequest> responseMultiple = await _userServiceClient.GetAllUsersAsync();

            return StatusCode(StatusCodes.Status200OK, new { data = responseMultiple, message = "" });
        }
    }
}
