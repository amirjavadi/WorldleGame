using WordleBackend.Models;

namespace WordleBackend.Services.Interfaces;

public interface IGameService
{
    Task<GameHistory> StartNewGameAsync(int userId);
    Task<GameHistory> MakeGuessAsync(int gameId, string guess);
    Task<IEnumerable<GameHistory>> GetUserGameHistoryAsync(int userId);
    Task<IEnumerable<GameHistory>> GetGameHistoryAsync();
    Task<List<GameHistory>> GetActiveGamesAsync(int userId);
    Task<List<GameHistory>> GetCompletedGamesAsync(int userId);
} 