using Ai_Lab.Models;

namespace Ai_Lab.Interfaces;

/// <summary>
/// Repository interface for movie-related database operations
/// </summary>
public interface IMovieRepository
{
    /// <summary>
    /// Gets all user movies from the database
    /// </summary>
    /// <param name="ct">Cancellation token for async operation</param>
    /// <returns>List of user movies</returns>
    Task<List<UserMovie>> GetUserMoviesAsync(CancellationToken ct = default);
}
