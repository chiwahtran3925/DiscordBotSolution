using DiscordBotSolution.BotApi.Core.DTOs;
using DiscordBotSolution.BotApi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiscordBotSolution.BotApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IBotService _botService;

        public BotController(IBotService botService)
        {
            _botService = botService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login(LoginRequest loginRequest)
        {
            var result = await _botService.Login(loginRequest);
            if (!result)
                return Unauthorized();

            return Ok(result);
        }

        [HttpPost("createuser")]
        public async Task<ActionResult<UserResponse>> CreateUser(UserRequest userRequest)
        {
            var user = await _botService.CreateUser(userRequest);
            return CreatedAtAction(nameof(GetUserByUsername),
                                   new { username = user.Username },
                                   user);
        }

        [HttpPut("updateuser")]
        public async Task<ActionResult> UpdateUser(UserRequest userRequest)
        {
            var updated = await _botService.UpdateUser(userRequest);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpGet("getuserbyusername/{username}")]
        public async Task<ActionResult<UserResponse>> GetUserByUsername(string username)
        {
            var user = await _botService.GetUserByUsername(username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("getallusers")]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            var users = await _botService.GetAllUser();
            return Ok(users);
        }

        [HttpPost("createusertimer")]
        public async Task<ActionResult<UserTimerResponse>> CreateUserTimer(UserTimerRequest userTimerRequest)
        {
            var timer = await _botService.CreateUserTimer(userTimerRequest);
            return Ok(timer);
        }

        [HttpPut("updateusertimer")]
        public async Task<ActionResult> UpdateUserTimer(UserTimerRequest userTimerRequest)
        {
            var updated = await _botService.UpdateUserTimer(userTimerRequest);
            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
