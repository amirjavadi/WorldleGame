using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Services;

namespace WordleBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet("global")]
        public async Task<IActionResult> GetGlobalLeaderboard([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var leaderboard = await _leaderboardService.GetGlobalLeaderboardAsync(page, pageSize);
                return Ok(leaderboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyLeaderboard([FromQuery] DateTime? date = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var targetDate = date ?? DateTime.UtcNow.Date;
                var leaderboard = await _leaderboardService.GetDailyLeaderboardAsync(targetDate, page, pageSize);
                return Ok(leaderboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("rank")]
        public async Task<IActionResult> GetUserRank()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "کاربر احراز هویت نشده است" });
                }

                var rank = await _leaderboardService.GetUserRankAsync(userId);
                if (rank == null)
                {
                    return NotFound(new { message = "اطلاعات رتبه‌بندی یافت نشد" });
                }

                return Ok(rank);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetGameStatistics()
        {
            try
            {
                var statistics = await _leaderboardService.GetGameStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 