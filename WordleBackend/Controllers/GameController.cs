using System;
using System.Threading.Tasks;
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

        [HttpPost("start")]
        public async Task<IActionResult> StartGame()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var game = await _gameService.StartNewGameAsync(userId);
                return Ok(new
                {
                    gameId = game.Id,
                    wordLength = game.Word.Text.Length
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("guess")]
        public async Task<IActionResult> MakeGuess([FromBody] GuessRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var game = await _gameService.MakeGuessAsync(request.GameId, userId, request.Guess);
                return Ok(new
                {
                    isWon = game.Status == "Won",
                    attempts = game.Attempts,
                    score = game.Score,
                    status = game.Status
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
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var history = await _gameService.GetUserGameHistoryAsync(userId, page, pageSize);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveGames()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var games = await _gameService.GetActiveGamesAsync(userId);
                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedGames()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var games = await _gameService.GetCompletedGamesAsync(userId);
                return Ok(games);
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
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var gameHistory = await _gameService.GetGameHistoryAsync(userId, DateTime.UtcNow);
                if (gameHistory == null)
                {
                    return Ok(new { hasPlayed = false });
                }

                return Ok(new
                {
                    hasPlayed = true,
                    isWon = gameHistory.Status == "Won",
                    attempts = gameHistory.Attempts,
                    score = gameHistory.Score,
                    status = gameHistory.Status
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
        public int GameId { get; set; }
        public string Guess { get; set; }
    }
} 