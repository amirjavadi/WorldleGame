using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Services;

namespace WordleBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodaysWord()
        {
            try
            {
                var word = await _gameService.GetTodaysWordAsync();
                return Ok(new { length = word.Text.Length });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("guess")]
        public async Task<IActionResult> SubmitGuess([FromBody] GuessRequest request)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "کاربر احراز هویت نشده است" });
                }

                var result = await _gameService.SubmitGuessAsync(userId, request.Guess);
                return Ok(new
                {
                    isWon = result.IsWon,
                    attempts = result.Attempts,
                    score = result.Score
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "کاربر احراز هویت نشده است" });
                }

                var history = await _gameService.GetUserGameHistoryAsync(userId, page, pageSize);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("today/status")]
        public async Task<IActionResult> GetTodayGameStatus()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "کاربر احراز هویت نشده است" });
                }

                var gameHistory = await _gameService.GetGameHistoryAsync(userId, DateTime.UtcNow);
                if (gameHistory == null)
                {
                    return Ok(new { hasPlayed = false });
                }

                return Ok(new
                {
                    hasPlayed = true,
                    isWon = gameHistory.IsWon,
                    attempts = gameHistory.Attempts,
                    score = gameHistory.Score
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class GuessRequest
    {
        public string Guess { get; set; } = string.Empty;
    }
} 