using IntelliAssist.Models;

namespace IntelliAssist.Interfaces;


// Repository interface for movie-related database operations

public interface IMovieRepository
{
    // Gets all user movies from the database, returning a list of UserMovie objects
    Task<List<UserMovie>> GetUserMoviesAsync(CancellationToken ct = default);
}
