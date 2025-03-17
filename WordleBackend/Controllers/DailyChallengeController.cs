using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Models;
using WordleBackend.Services.Interfaces;

namespace WordleBackend.Controllers;

[ApiController]
[Route("api/dailychallenge")]
[Authorize]
public class DailyChallengeController : ControllerBase
{
    private readonly IDailyChallengeService _dailyChallengeService;

    public DailyChallengeController(IDailyChallengeService dailyChallengeService)
    {
        _dailyChallengeService = dailyChallengeService;
    }

    [AllowAnonymous]
    [HttpGet("today")]
    public async Task<ActionResult<DailyChallenge>> GetTodaysChallenge()
    {
        var challenge = await _dailyChallengeService.GetTodaysChallengeAsync();
        return Ok(challenge);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DailyChallenge>> CreateChallenge([FromBody] CreateChallengeRequest request)
    {
        try
        {
            var challenge = await _dailyChallengeService.CreateChallengeAsync(request.Date);
            return Ok(challenge);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("participate")]
    [Authorize]
    public async Task<ActionResult<DailyChallengeParticipation>> StartParticipation()
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        if (userId == 0)
            return Unauthorized("Invalid or expired token");

        var participation = await _dailyChallengeService.StartParticipationAsync(userId);
        return Ok(participation);
    }

    [HttpPost("guess")]
    [Authorize]
    public async Task<ActionResult<DailyChallengeParticipation>> MakeGuess(
        [FromQuery] int participationId,
        [FromBody] GuessRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized("Invalid or expired token");

            var participation = await _dailyChallengeService.MakeGuessAsync(participationId, request.Guess);
            return Ok(participation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpGet("leaderboard")]
    public async Task<ActionResult<IEnumerable<DailyChallengeParticipation>>> GetLeaderboard(
        [FromQuery] DateTime? date,
        [FromQuery] int limit = 10)
    {
        var targetDate = date ?? DateTime.UtcNow.Date;
        var leaderboard = await _dailyChallengeService.GetLeaderboardAsync(targetDate, limit);
        return Ok(leaderboard);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<DailyChallengeParticipation>> GetMyParticipation([FromQuery] DateTime? date)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized("Invalid or expired token");

            var targetDate = date?.Date ?? DateTime.UtcNow.Date;
            var participation = await _dailyChallengeService.GetUserParticipationAsync(userId, targetDate);
            
            if (participation == null)
                return NotFound();

            return Ok(participation);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpGet("past-challenges")]
    public async Task<ActionResult<IEnumerable<DailyChallenge>>> GetPastChallenges([FromQuery] int days = 7)
    {
        var challenges = await _dailyChallengeService.GetPastChallengesAsync(days);
        return Ok(challenges);
    }
}

public class GuessRequest
{
    public string Guess { get; set; } = string.Empty;
}

public class CreateChallengeRequest
{
    public DateTime Date { get; set; }
} 