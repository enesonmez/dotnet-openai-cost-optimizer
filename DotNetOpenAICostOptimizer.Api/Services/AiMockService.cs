using DotNetOpenAICostOptimizer.Services;

namespace DotNetOpenAICostOptimizer.Services;

public class AiMockService : IAiProvider
{
    public async Task<string> AskAsync(string prompt, CancellationToken token = default)
    {
        await Task.Delay(2000, token);
        return $"[OpenAI Answer] Request: {prompt} - Process Time: {DateTime.Now:HH:mm:ss}";
    }
}