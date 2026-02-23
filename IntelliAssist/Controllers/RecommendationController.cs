using IntelliAssist.DTOs;
using IntelliAssist.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class RecommendationController : ControllerBase
{
    private readonly IRecommendationService _service;
    private readonly ILogger<RecommendationController> _logger;

    public RecommendationController(
        IRecommendationService service,
        ILogger<RecommendationController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("movies")]
    public async Task<ActionResult<MovieRecommendationResponse>> RecommendMovies(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Received request for movie recommendations");

            var result = await _service.GetMovieRecommendationsAsync(ct);

            _logger.LogInformation("Successfully returned movie recommendations");
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate movie recommendations");
            return StatusCode(500, new { error = "Failed to generate recommendations. Please try again later." });
        }
    }
}