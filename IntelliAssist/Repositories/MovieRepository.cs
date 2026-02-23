using IntelliAssist.Interfaces;
using IntelliAssist.Models;
using Microsoft.EntityFrameworkCore;

namespace IntelliAssist.Repositories;

// Repository for movie-related database operations
public class MovieRepository : IMovieRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<MovieRepository> _logger;

    public MovieRepository(AppDbContext db, ILogger<MovieRepository> logger)
    {
        _db = db;
        _logger = logger;
    }


    // Gets all user movies from the database, returning a list of UserMovie objects
    public async Task<List<UserMovie>> GetUserMoviesAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Fetching user movies from database");

        try
        {
            // Use AsNoTracking for read-only queries to improve performance,
            // dont track objects or cache them in memory, we just want to read them and return them to the caller 
            var movies = await _db.UserMovies
                .AsNoTracking()
                .OrderByDescending(m => m.Rating) // Order by rating for better AI analysis
                .ToListAsync(ct);

            _logger.LogInformation("Retrieved {Count} movies from database", movies.Count);
            return movies;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user movies from database");
            throw;
        }
    }
}
