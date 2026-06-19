using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnnxChatApi.Models;
using OnnxChatApi.Services;
using System.Text.Json;

namespace OnnxChatApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ChatController : ControllerBase {
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService) {
        _chatService = chatService;
    }

    private readonly SemaphoreSlim _semaphore = new(1, 1);

    [HttpPost("stream")]
    [ApiKey]
    [ProducesResponseType(typeof(ChatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("stream-ndjson")]
    public async Task StreamNdjson(ChatRequest request, CancellationToken ct) {
        Response.ContentType = "application/x-ndjson";

        await foreach (var token in GenerateTokens(request.Message, ct)) {
            var json = JsonSerializer.Serialize(new { token });
            await Response.WriteAsync(json + "\n", ct);
            await Response.Body.FlushAsync(ct);
        }
    }


    [HttpPost]
    [ApiKey]
    [ProducesResponseType(typeof(ChatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ChatResponse>> Chat(
        [FromBody] ChatRequest request,
        CancellationToken cancellationToken) {

        // TODO remove for production (development mode only)
        await _semaphore.WaitAsync();

        try {
            var reply = await _chatService.ChatAsync(request.Message, cancellationToken);

            if (reply.Contains(@"</think>")) {
                reply = reply.Replace(@"</think>", string.Empty);
            }

            reply = reply.Replace(@"\n", Environment.NewLine);

            return Ok(new ChatResponse {
                Reply = reply.Trim()
            });
        } finally {
            _semaphore.Release();
        }
    }
}