using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Models;
using WordleBackend.Services;

namespace WordleBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            try
            {
                var profile = await _userProfileService.GetUserProfileAsync(userId);
                return Ok(profile);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("پروفایل یافت نشد");
            }
        }

        [HttpPut]
        public async Task<ActionResult<User>> UpdateProfile([FromBody] User updatedProfile)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            try
            {
                var profile = await _userProfileService.UpdateUserProfileAsync(userId, updatedProfile);
                return Ok(profile);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("پروفایل یافت نشد");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<UserStats>> GetStats()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            try
            {
                var stats = await _userProfileService.GetUserStatsAsync(userId);
                return Ok(stats);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("کاربر یافت نشد");
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<GameHistory>>> GetGameHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var history = await _userProfileService.GetUserGameHistoryAsync(userId, page, pageSize);
            return Ok(history);
        }

        [HttpPut("settings")]
        public async Task<ActionResult> UpdateSettings([FromBody] UserSettings settings)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            try
            {
                await _userProfileService.UpdateUserSettingsAsync(userId, settings);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("کاربر یافت نشد");
            }
        }
    }
} 