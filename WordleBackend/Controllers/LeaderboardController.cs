using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Models;
using WordleBackend.Services.Interfaces;

namespace WordleBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpGet("daily")]
    public async Task<ActionResult<IEnumerable<Leaderboard>>> GetDailyLeaderboard([FromQuery] int limit = 10)
    {
        var leaderboard = await _leaderboardService.GetDailyLeaderboardAsync(limit);
        return Ok(leaderboard);
    }

    [HttpGet("weekly")]
    public async Task<ActionResult<IEnumerable<Leaderboard>>> GetWeeklyLeaderboard([FromQuery] int limit = 10)
    {
        var leaderboard = await _leaderboardService.GetWeeklyLeaderboardAsync(limit);
        return Ok(leaderboard);
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<IEnumerable<Leaderboard>>> GetMonthlyLeaderboard([FromQuery] int limit = 10)
    {
        var leaderboard = await _leaderboardService.GetMonthlyLeaderboardAsync(limit);
        return Ok(leaderboard);
    }

    [HttpGet("all-time")]
    public async Task<ActionResult<IEnumerable<Leaderboard>>> GetAllTimeLeaderboard([FromQuery] int limit = 10)
    {
        var leaderboard = await _leaderboardService.GetAllTimeLeaderboardAsync(limit);
        return Ok(leaderboard);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<Leaderboard>> GetUserStats(int userId, [FromQuery] string period = "all-time")
    {
        var stats = await _leaderboardService.GetUserStatsAsync(userId, period);
        return Ok(stats);
    }

    [HttpGet("me")]
    public async Task<ActionResult<Leaderboard>> GetMyStats([FromQuery] string period = "all-time")
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        if (userId == 0)
            return Unauthorized();

        var stats = await _leaderboardService.GetUserStatsAsync(userId, period);
        return Ok(stats);
    }
} 