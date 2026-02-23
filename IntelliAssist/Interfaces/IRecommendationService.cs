using IntelliAssist.DTOs;

namespace IntelliAssist.Interfaces;

/// <summary>
/// Service interface for generating movie recommendations
/// </summary>
public interface IRecommendationService
{
    /// <summary>
    /// Get the user's movie history from the database, create a prompt for the AI based on that history, 
    /// and get movie recommendations from the AI
    /// </summary>
    /// <param name="ct">Cancellation token for async operation</param>
    /// <returns>Movie recommendation response with AI-generated recommendations</returns>
    Task<MovieRecommendationResponse> GetMovieRecommendationsAsync(CancellationToken ct = default);
}
