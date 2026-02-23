namespace IntelliAssist.DTOs;


// Response model for movie recommendations

public class MovieRecommendationResponse
{
    public string Recommendations { get; set; } = string.Empty;
    public int MovieCount { get; set; }
    public DateTime GeneratedAt { get; set; }
}
