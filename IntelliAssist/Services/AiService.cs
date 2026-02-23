using IntelliAssist.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;


// for reading and parsing JSON responses from the Ollama API

// Register AiService in Program.cs(DI container)
public class AiService : IAiService
{
    private readonly HttpClient _http;
    private readonly ILogger<AiService> _logger;

    public AiService(HttpClient http, ILogger<AiService> logger)
    {
        _http = http; // Receive HttpClient from DI container
        _http.BaseAddress = new Uri("http://localhost:11434"); 
        // Set base address for Ollama API http://localhost:11434/api/generate
        // terminal : ollama serve > to check port and if server is running
        _logger = logger;
    }


    // Recieve mgs from UI/Controller > Send to AI > return to string
    public async Task<string> AskAsync(string prompt, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(prompt);

        _logger.LogDebug("Sending prompt to Ollama AI (length: {Length})", prompt.Length);

        try
        {
            // Body of request to Ollama API
            var body = new
            {
                model = "phi3",
                prompt = prompt, // The prompt is the message we want to send to the AI
                stream = false  // We want the finished full response at once, not a stream of tokens
            };

            // Send POST request to Ollama API with the body as JSON
            var response = await _http.PostAsJsonAsync("/api/generate", body, ct);
            response.EnsureSuccessStatusCode(); // Throw an exception if the response status code is not successful (e.g., 200 OK)


            // Recieve the response content as a string and parse it as JSON
            var json = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);


            // Check if the JSON response has a "response" property and return its value as a string,
            // otherwise return an empty string
            if (doc.RootElement.TryGetProperty("response", out var prop))
            {
                var result = prop.GetString() ?? "";
                _logger.LogInformation("Successfully received AI response (length: {Length})", result.Length);
                return result;
            }

            _logger.LogWarning("AI response did not contain expected 'response' property");
            return json; // If the "response" property is not found, return the raw JSON string (for debugging purposes)
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error calling Ollama API. Is Ollama running?");
            throw new InvalidOperationException("Failed to connect to Ollama API. Please ensure Ollama is running on localhost:11434", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Ollama AI service");
            throw;
        }
    }

    /// <summary>
    /// Test if Ollama is running and reachable
    /// </summary>
    public async Task<bool> IsOllamaRunningAsync()
    {
        try
        {
            _logger.LogDebug("Checking if Ollama is running");
            var response = await _http.GetAsync("/");
            var isRunning = response.IsSuccessStatusCode;
            _logger.LogInformation("Ollama running status: {IsRunning}", isRunning);
            return isRunning;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check Ollama status");
            return false;
        }
    }

    /// <summary>
    /// Get list of available models from Ollama
    /// </summary>
    public async Task<string> GetAvailableModelsAsync()
    {
        try
        {
            _logger.LogDebug("Fetching available Ollama models");
            var response = await _http.GetAsync("/api/tags");
            response.EnsureSuccessStatusCode();
            var models = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Successfully fetched Ollama models");
            return models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Ollama models");
            return $"Error fetching models: {ex.Message}";
        }
    }
}


//// AiService:
//1.Receive message from UI via parameter in AskAsync(string prompt)-
//UI/ controller will call this method 

//2. Create JSON body
//3. Send to Ollama 
//4. Wait for results
//5. Read JSON
//6. Extract only the answer message
//7. Send back to UI

// This method does not get http request , but UI call this method as usual function