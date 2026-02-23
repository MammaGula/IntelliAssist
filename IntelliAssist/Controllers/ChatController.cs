using IntelliAssist.DTOs;
using IntelliAssist.Interfaces;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")] // URL : /api/chat, every endpoint will start with /api/chat
public class ChatController : ControllerBase 
{
    private readonly IAiService _ai;
    private readonly ILogger<ChatController> _logger;

    //Controller uses AiService, ASP.NET will automatically inject AiService 
    public ChatController(IAiService ai, ILogger<ChatController> logger)
    {
        _ai = ai;
        _logger = logger;
    }

    // Endpoint1: Talk to AI (Send Msg to > AiService > Ollama > Controller > UI)
    [HttpPost("chat")]
    public async Task<IActionResult> Chat(ChatRequest req, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(req.Message))
            {
                return BadRequest(new { error = "Message cannot be empty" });
            }

            _logger.LogInformation("Received chat request");
            var result = await _ai.AskAsync(req.Message, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat request");
            return StatusCode(500, new { error = "Failed to process chat request" });
        }
    }

    // Endpoint2: Summarize text (Create propt to summarize information > AI > get ANSWER back)
    [HttpPost("summarize")]
    // [FromBody] : Attribute to tell ASP.NET, the data of parameter is read from the body of the http Request
    public async Task<IActionResult> Summarize(SummarizeRequest req, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(req.Text))
            {
                return BadRequest(new { error = "Text cannot be empty" });
            }

            _logger.LogInformation("Received summarize request");
            var prompt = $"Summarize the following text:\n\n{req.Text}";
            var result = await _ai.AskAsync(prompt, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing summarize request");
            return StatusCode(500, new { error = "Failed to process summarize request" });
        }
    }

    //Endpoint3: Translate sentence to Swedish (Create promt for translation > AiService > get answer from ai)
    [HttpPost("translate")]
    public async Task<IActionResult> Translate(TranslateRequest req, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(req.Sentence))
            {
                return BadRequest(new { error = "Sentence cannot be empty" });
            }

            _logger.LogInformation("Received translate request");
            var prompt = $"Translate this sentence to Swedish: {req.Sentence}";
            var result = await _ai.AskAsync(prompt, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing translate request");
            return StatusCode(500, new { error = "Failed to process translate request" });
        }
    }
}

// Blazor UI > ChatController > AiService > Ollama API > AiService > ChatController > Blazor UI

//  ✔ Simple types(string, int, bool, double)
//  → default = FromQuery
//  ✔ Complex types(class, object)
//  → default = FromBody

//  So, for the endpoints in ChatController:
//  ❌ string → Must use [FromBody]
//  ✔ class → No need [FromBody]
