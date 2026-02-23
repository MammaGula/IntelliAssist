namespace Ai_Lab.Interfaces;


// Service interface for AI operations using Ollama API

public interface IAiService
{
 
    // Send a prompt to the AI and get a response
    Task<string> AskAsync(string prompt, CancellationToken ct = default);


    // Test if Ollama is running and reachable
    Task<bool> IsOllamaRunningAsync();


    // Get list of available models from Ollama
    Task<string> GetAvailableModelsAsync();
}
