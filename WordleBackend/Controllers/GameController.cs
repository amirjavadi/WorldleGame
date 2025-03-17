using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Models;
using WordleBackend.Services.Interfaces;
using System.Collections.Generic;

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
        public async Task<ActionResult<GameHistory>> StartGame()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized();

            try
            {
                var game = await _gameService.StartNewGameAsync(userId);
                return Ok(game);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{gameId}/guess")]
        public async Task<ActionResult<GameHistory>> MakeGuess(int gameId, [FromBody] GuessRequest request)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized();

            try
            {
                var game = await _gameService.MakeGuessAsync(gameId, request.Guess);
                return Ok(game);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<GameHistory>>> GetUserHistory()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized();

            var history = await _gameService.GetUserGameHistoryAsync(userId);
            return Ok(history);
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<GameHistory>>> GetActiveGames()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized();

            var games = await _gameService.GetActiveGamesAsync(userId);
            return Ok(games);
        }

        [HttpGet("completed")]
        public async Task<ActionResult<List<GameHistory>>> GetCompletedGames()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized();

            var games = await _gameService.GetCompletedGamesAsync(userId);
            return Ok(games);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GameHistory>>> GetAllGames()
        {
            var games = await _gameService.GetGameHistoryAsync();
            return Ok(games);
        }

        [HttpGet("today/status")]
        public async Task<IActionResult> GetTodayGameStatus()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized();

            var history = await _gameService.GetUserGameHistoryAsync(userId);
            var todayGame = history.FirstOrDefault(g => g.StartTime.Date == DateTime.UtcNow.Date);

            if (todayGame == null)
            {
                return Ok(new { hasPlayed = false });
            }

            return Ok(new
            {
                hasPlayed = true,
                isWon = todayGame.Status == "won",
                attempts = todayGame.Attempts,
                score = todayGame.Score,
                status = todayGame.Status
            });
        }
    }
}