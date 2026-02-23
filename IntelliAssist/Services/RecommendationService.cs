using IntelliAssist.DTOs;
using IntelliAssist.Interfaces;
using IntelliAssist.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IntelliAssist;

public class RecommendationService : IRecommendationService
{
    private readonly IMovieRepository _movieRepo;
    private readonly IAiService _ai;
    private readonly ILogger<RecommendationService> _logger;

    public RecommendationService(
        IMovieRepository movieRepo,
        IAiService ai,
        ILogger<RecommendationService> logger)
    {
        _movieRepo = movieRepo;
        _ai = ai;
        _logger = logger;
    }


    // Get the user's movie history from the database, create a prompt for the AI based on that history, and get movie recommendations from the AI
    public async Task<MovieRecommendationResponse> GetMovieRecommendationsAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting movie recommendation generation");

        try
        {
            // Fetch movies from repository
            var movies = await _movieRepo.GetUserMoviesAsync(ct);

            // Validate that user has movie history
            if (!movies.Any())
            {
                _logger.LogWarning("No movie history found for user");
                throw new InvalidOperationException("User has no movie history. Please add some movies first.");
            }

            // Create a string representation of the user's movie history to include in the AI prompt
            var movieHistory = BuildMovieHistoryText(movies);

            // Create a prompt for the AI that includes the user's movie history and asks for recommendations
            var prompt = $@"
Here is a user's movie history:
{movieHistory}

Based on their preferences, recommend 5 movies and explain why.
";

            _logger.LogDebug("Calling AI service with {MovieCount} movies", movies.Count);

            // Send the prompt to the AI service and return the AI's response, which should contain movie recommendations
            var recommendations = await _ai.AskAsync(prompt, ct);

            _logger.LogInformation("Successfully generated movie recommendations");

            return new MovieRecommendationResponse
            {
                Recommendations = recommendations,  
                MovieCount = movies.Count,
                GeneratedAt = DateTime.UtcNow
            };
        }
        catch (InvalidOperationException)
        {
            throw; // Re-throw validation exceptions
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate movie recommendations");
            throw;
        }
    }



    /// <summary>
    /// Build formatted text of movie history for AI prompt
    /// </summary>
    private static string BuildMovieHistoryText(List<UserMovie> movies)
    {
        var sb = new StringBuilder();
        foreach (var m in movies)
            sb.AppendLine($"{m.Title} ({m.Genre}) rating: {m.Rating}/5");
        return sb.ToString();
    }
}
