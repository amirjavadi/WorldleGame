using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Services.Interfaces;

namespace WordleBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("global")]
    public async Task<IActionResult> GetGlobalStatistics()
    {
        var stats = await _statisticsService.GetGlobalStatisticsAsync();
        return Ok(stats);
    }

    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserStatistics(int userId)
    {
        var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        if (currentUserId != userId && !User.IsInRole("Admin"))
            return Forbid();

        var stats = await _statisticsService.GetUserStatisticsAsync(userId);
        return Ok(stats);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyStatistics()
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        if (userId == 0)
            return Unauthorized();

        var stats = await _statisticsService.GetUserStatisticsAsync(userId);
        return Ok(stats);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetCategoryStatistics(int categoryId)
    {
        try
        {
            var stats = await _statisticsService.GetCategoryStatisticsAsync(categoryId);
            return Ok(stats);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GetDailyStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        var stats = await _statisticsService.GetDailyStatisticsAsync(start, end);
        return Ok(stats);
    }

    [HttpGet("words/most-played")]
    public async Task<IActionResult> GetMostPlayedWords([FromQuery] int limit = 10)
    {
        var stats = await _statisticsService.GetMostPlayedWordsAsync(limit);
        return Ok(stats);
    }

    [HttpGet("words/hardest")]
    public async Task<IActionResult> GetHardestWords([FromQuery] int limit = 10)
    {
        var stats = await _statisticsService.GetHardestWordsAsync(limit);
        return Ok(stats);
    }

    [HttpGet("words/easiest")]
    public async Task<IActionResult> GetEasiestWords([FromQuery] int limit = 10)
    {
        var stats = await _statisticsService.GetEasiestWordsAsync(limit);
        return Ok(stats);
    }
} 