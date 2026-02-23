using IntelliAssist.DTOs;

namespace IntelliAssist.Interfaces;


// Service interface for generating movie recommendations

public interface IRecommendationService
{
    // Get the user's movie history from the database, create a prompt for the AI based on that history, 
    // and get movie recommendations from the AI
    // <returns>Movie recommendation response with AI-generated recommendations</returns>
    Task<MovieRecommendationResponse> GetMovieRecommendationsAsync(CancellationToken ct = default);
}
