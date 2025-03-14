using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Models;
using WordleBackend.Services;

namespace WordleBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // مدیریت کاربران
        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _adminService.GetAllUsersAsync(page, pageSize);
            return Ok(users);
        }

        [HttpPut("users/{userId}/role")]
        public async Task<ActionResult<User>> UpdateUserRole(int userId, [FromBody] string role)
        {
            try
            {
                var user = await _adminService.UpdateUserRoleAsync(userId, role);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("کاربر یافت نشد");
            }
        }

        [HttpDelete("users/{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var result = await _adminService.DeleteUserAsync(userId);
            if (!result)
                return NotFound("کاربر یافت نشد");
            return Ok();
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<User>> GetUserDetails(int userId)
        {
            try
            {
                var user = await _adminService.GetUserDetailsAsync(userId);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("کاربر یافت نشد");
            }
        }

        // مدیریت کلمات
        [HttpGet("words")]
        public async Task<ActionResult<List<Word>>> GetWords([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var words = await _adminService.GetAllWordsAsync(page, pageSize);
            return Ok(words);
        }

        [HttpPost("words")]
        public async Task<ActionResult<Word>> AddWord([FromBody] Word word)
        {
            var newWord = await _adminService.AddWordAsync(word);
            return CreatedAtAction(nameof(GetWordDetails), new { wordId = newWord.Id }, newWord);
        }

        [HttpPut("words/{wordId}")]
        public async Task<ActionResult<Word>> UpdateWord(int wordId, [FromBody] Word word)
        {
            try
            {
                var updatedWord = await _adminService.UpdateWordAsync(wordId, word);
                return Ok(updatedWord);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("کلمه یافت نشد");
            }
        }

        [HttpDelete("words/{wordId}")]
        public async Task<ActionResult> DeleteWord(int wordId)
        {
            var result = await _adminService.DeleteWordAsync(wordId);
            if (!result)
                return NotFound("کلمه یافت نشد");
            return Ok();
        }

        [HttpGet("words/{wordId}")]
        public async Task<ActionResult<Word>> GetWordDetails(int wordId)
        {
            try
            {
                var word = await _adminService.GetWordDetailsAsync(wordId);
                return Ok(word);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("کلمه یافت نشد");
            }
        }

        // گزارشات و آمار
        [HttpGet("dashboard")]
        public async Task<ActionResult<AdminDashboardStats>> GetDashboardStats()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return Ok(stats);
        }

        [HttpGet("recent-games")]
        public async Task<ActionResult<List<GameHistory>>> GetRecentGames([FromQuery] int count = 10)
        {
            var games = await _adminService.GetRecentGamesAsync(count);
            return Ok(games);
        }

        [HttpGet("settings")]
        public async Task<ActionResult<Models.SystemSettings>> GetSystemSettings()
        {
            var settings = await _adminService.GetSystemSettingsAsync();
            return Ok(settings);
        }

        [HttpPut("settings")]
        public async Task<ActionResult> UpdateSystemSettings([FromBody] Models.SystemSettings settings)
        {
            await _adminService.UpdateSystemSettingsAsync(settings);
            return Ok();
        }
    }
} 