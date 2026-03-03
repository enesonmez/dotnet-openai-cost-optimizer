using OpenAI.Chat;

namespace DotNetOpenAICostOptimizer.Services;

public class OpenAiService : IAiProvider
{
    private readonly ChatClient _client;
    
    public OpenAiService(ChatClient client)
    {
        _client = client;
    }
    
    public async Task<string> AskAsync(string prompt, CancellationToken token = default)
    {
        var messages = new List<ChatMessage>
        {
            new UserChatMessage(prompt)
        };

        ChatCompletion completion = await _client.CompleteChatAsync(
            messages, 
            cancellationToken: token
        );
        
        return completion is { Content.Count: > 0 } ? completion.Content[0].Text : string.Empty;
    }
}