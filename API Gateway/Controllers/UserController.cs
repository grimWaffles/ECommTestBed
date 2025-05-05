using API_Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserServiceGrpc;

namespace API_Gateway.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("test")]
        public ActionResult TestUserService()
        {
            return Ok("User service up and running!");
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            UserResponseSingle user = await _userService.GetUserById(id);

            return StatusCode(StatusCodes.Status200OK, new { data = user });
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> GetAllUsers()
        {
            UserResponseMultiple response = await _userService.GetAllUsers();

            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { data = "" });
            }

            return StatusCode(StatusCodes.Status200OK, new { data = response });
        }

        [HttpGet]
        [Route("all/stream")]
        public async Task<ActionResult> GetAllUsersStream()
        {
            UserResponseMultiple response = await _userService.GetAllUsersStream();

            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { data = "" });
            }

            return StatusCode(StatusCodes.Status200OK, new { data = response });
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LoginUser([FromForm] string username, [FromForm] string password)
        {
            UserLoginResponse response = await _userService.LoginUser(username, password);

            return StatusCode(StatusCodes.Status200OK, new { data = response });
        }
    }
}
