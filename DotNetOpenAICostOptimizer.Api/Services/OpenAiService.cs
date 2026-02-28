using DotNetOpenAICostOptimizer.Models;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace DotNetOpenAICostOptimizer.Services;

public class OpenAiService : IOpenAiService
{
    private readonly ChatClient _client;
    
    public OpenAiService(IOptions<OpenAiSettings> settings)
    {
        _client = new ChatClient(model: settings.Value.Model, apiKey: settings.Value.ApiKey);
    }
    
    public async Task<string> AskAsync(string prompt, CancellationToken token = default)
    {
        ChatCompletion completion = await _client.CompleteChatAsync(prompt);
        
        return completion != null && completion.Content.Count > 0 ? completion.Content[0].Text : string.Empty;
    }
}